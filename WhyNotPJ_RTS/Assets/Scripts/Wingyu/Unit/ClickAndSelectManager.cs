using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndSelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
	[SerializeField] private RectTransform dragRectangle;	// ���콺�� �巡���� ������ ����ȭ�ϴ� Image UI�� RectTransform

	private Rect dragRect;
	private Vector3 start = Vector2.zero;
	private Vector3 end = Vector2.zero;

    private Camera mainCam;
    private UnitSelectManager unitManager;	// ������ ���� & ������ ����ϴ� UnitManager Ŭ����

	public bool isDraging = false;
	private bool isUI = false;
	private bool canSelect = true;

	private void Awake()
	{
		mainCam = Camera.main;

		// ���ָŴ��� ����,, ���� ���� ��ġ ���� �ʿ�
		unitManager = GetComponent<UnitSelectManager>();

		DrawDragRectangle();
	}

	private void Update()
	{
		if (!canSelect && Input.touchCount == 0)
		{
			CameraController.camState = CameraState.NONE;
			canSelect = true;
		}

		if (Input.touchCount >= 2 || !canSelect)
		{
			canSelect = false;
			return;
		}


		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
			{
				isUI = true;
			}
			else
				DragSelect(touch);

			// �հ����� ���� ��
			if (touch.phase == TouchPhase.Ended)
			{
				if (isUI)
				{
					isUI = false;
					return;
				}
				if (CameraController.camState == CameraState.MOVING)
				{
					CameraController.camState = CameraState.NONE;
					return;
				}


				CheckUnit(Input.GetTouch(0).position);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (CameraController.camState == CameraState.MOVING)
			{
				CameraController.camState = CameraState.NONE;
				return;
			}

			if (!EventSystem.current.IsPointerOverGameObject() && CameraController.camState != CameraState.DRAGSELECTING)
				CheckUnit(Input.mousePosition);

			if (start - end == Vector3.zero || CameraController.camState != CameraState.DRAGSELECTING)
				return;

			CameraController.camState = CameraState.NONE;

			CalculateDragRect();
			SelectUnits();

			start = end = Vector2.zero;
			DrawDragRectangle();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			start = Input.mousePosition;
			dragRect = new Rect();
		}
		else if (Input.GetMouseButton(0))
		{
			DragSelect();
		}
	}

	private void CheckUnit(Vector3 screenPos)
	{
		RaycastHit hit;
		Ray ray = mainCam.ScreenPointToRay(screenPos);
		if (Physics.Raycast(ray, out hit, 300f))
		{
			if (hit.transform.TryGetComponent(out UnitController unit))
			{
				if (unit.isPlayer == true)
				{
					UnitSelectManager.Instance.ClickSelectUnit(unit);
				}
				else
				{
					UnitSelectManager.Instance.SelectedUnitList.ForEach(selected => {
						if (selected.UnitMove.SetTargetPosition(unit.transform))
                        {
							selected.InitTarget(unit);
                            selected.ChangeState(Core.State.Move);
                        }
					});
					UnitSelectManager.Instance.DeselectAll();
				}
			}
			else if (hit.transform.TryGetComponent(out MainCamp camp))
			{
				if (camp.isPlayer == true)
				{
					return;
				}
				UnitSelectManager.Instance.SelectedUnitList.ForEach(selected =>
				{
					selected.UnitMove.SetTargetPosition(hit.transform);
					selected.mainCamp = camp;
					if (selected.currentState != Core.State.Move)
						selected.ChangeState(Core.State.Move);
				});
				UnitSelectManager.Instance.DeselectAll();
			}
			else
			{
				UnitSelectManager.Instance.SelectedUnitList.ForEach(selected => {
                    if (selected.UnitMove.SetTargetPosition(hit.point))
                    {
						selected.InitTarget();
						selected.ChangeState(Core.State.Move);
                    }
				});
				UnitSelectManager.Instance.DeselectAll();
			}
		}
	}

	public void SetState()
	{
		CameraController.camState = CameraController.camState == CameraState.DRAGSELECTING ? CameraState.NONE : CameraState.DRAGSELECTING;
	}

	private void DragSelect()
	{
		end = Input.mousePosition;

		if (CameraController.camState != CameraState.DRAGSELECTING)
			return;

		DrawDragRectangle();
	}

	private void DragSelect(Touch touch)
	{
		if (CameraController.camState != CameraState.DRAGSELECTING)
			return;

		// �巡�� �̺�Ʈ - ����
		if (touch.phase == TouchPhase.Began)
		{
			start = touch.position;
			dragRect = new Rect();
		}

		// �巡�� �̺�Ʈ - �巡�� ��
		if (touch.phase == TouchPhase.Moved)
		{
			end = touch.position;

			DrawDragRectangle();
		}

		if (touch.phase == TouchPhase.Ended)
		{
			if (start - end == Vector3.zero)
				return;

			CalculateDragRect();
			SelectUnits();

			start = end = Vector2.zero;
			DrawDragRectangle();

			CameraController.camState = CameraState.NONE;
		}
	}

	private void DrawDragRectangle()
	{
		dragRectangle.position = (start + end) * 0.5f;
		dragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
	}

	private void CalculateDragRect()
	{
		Vector2 position;

		if (Input.touchCount > 0)
			position = Input.GetTouch(0).position;
		else
			position = Input.mousePosition;

		if (position.x < start.x)
		{
			dragRect.xMin = position.x;
			dragRect.xMax = start.x;
		}
		else
		{
			dragRect.xMin = start.x;
			dragRect.xMax = position.x;
		}

		if (position.y < start.y)
		{
			dragRect.yMin = position.y;
			dragRect.yMax = start.y;
		}
		else
		{
			dragRect.yMin = start.y;
			dragRect.yMax = position.y;
		}
	}

	/// <summary>
	/// dragRect�� ���� ���� �ִ� ������ �����ϴ� �Լ�
	/// </summary>
	private void SelectUnits()
	{
		UnitSelectManager.Instance.DeselectAll();
		foreach (UnitController unit in UnitSelectManager.Instance.unitList)
		{
			if (unit.isPlayer == false)
				continue;

			if (unit.CanDragSelect && dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
			{
				UnitSelectManager.Instance.DragSelectUnit(unit);
			}
		}
	}
}
