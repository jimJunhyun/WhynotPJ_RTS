using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndSelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
	[SerializeField] private RectTransform dragRectangle;	// ���콺�� �巡���� ������ ����ȭ�ϴ� Image UI�� RectTransform

	public RectTransform debug1;
	public RectTransform debug2;

	private Rect dragRect;
	private Vector3 start = Vector2.zero;
	private Vector3 end = Vector2.zero;

    private Camera mainCam;
    private UnitSelectManager unitManager;	// ������ ���� & ������ ����ϴ� UnitManager Ŭ����

	public bool isDraging = false;

	private void Awake()
	{
		mainCam = Camera.main;

		// ���ָŴ��� ����,, ���� ���� ��ġ ���� �ʿ�
		unitManager = GetComponent<UnitSelectManager>();

		DrawDragRectangle();
	}

	private void Update()
	{
		// Ŭ�� �̺�Ʈ
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			//�巡�� ����
			DragSelect(touch);

			// �հ����� ���� ��
			if (touch.phase == TouchPhase.Ended)
			{
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
		if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, 300f))
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
				}
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
