using UnityEngine;

public class CameraSelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
	[SerializeField] private RectTransform dragRectangle;	// ���콺�� �巡���� ������ ����ȭ�ϴ� Image UI�� RectTransform

	private Rect dragRect;
	private Vector3 start = Vector2.zero;
	private Vector3 end = Vector2.zero;

    private Camera mainCam;
    private UnitSelectManager unitManager;

	private void Awake()
	{
		mainCam = Camera.main;

		// ���ָŴ��� ����,, ���� ���� ��ġ ���� �ʿ�
		unitManager = new UnitSelectManager();

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

				RaycastHit hit;
				Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
				{
					if (hit.transform.TryGetComponent(out ISelectable unit))
					{
						unitManager.ClickSelectUnit(unit);
					}
				}
			}
		}
	}

	public void SetState()
	{
		CameraController.camState = CameraController.camState == CameraState.DRAGSELECTING ? CameraState.NONE : CameraState.DRAGSELECTING;
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
		Touch touch = Input.GetTouch(0);

		if (touch.position.x < start.x)
		{
			dragRect.xMin = touch.position.x;
			dragRect.xMax = start.x;
		}
		else
		{
			dragRect.xMin = start.x;
			dragRect.xMax = touch.position.x;
		}

		if (touch.position.y < start.y)
		{
			dragRect.yMin = touch.position.y;
			dragRect.yMax = start.y;
		}
		else
		{
			dragRect.yMin = start.y;
			dragRect.yMax = touch.position.y;
		}
	}

	private void SelectUnits()
	{
		foreach (UnitController unit in unitManager.unitList)
		{
			if (unit.CanDragSelect && dragRect.Contains(mainCam.WorldToScreenPoint(unit.WorldPos)))
			{
				unitManager.DragSelectUnit(unit);
			}
		}
	}
}
