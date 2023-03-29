using UnityEngine;

public class CameraUnitSelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
	[SerializeField] private RectTransform dragRectangle;	// 마우스로 드래그한 범위를 가시화하는 Image UI의 RectTransform

	private Rect dragRect;
	private Vector3 start = Vector2.zero;
	private Vector3 end = Vector2.zero;

    private Camera mainCam;
    private UnitController unitController;

	private void Awake()
	{
		mainCam = Camera.main;
		unitController = GetComponent<UnitController>();

		DrawDragRectangle();
	}

	private void Update()
	{
		print(CameraController.camState.ToString());

		// 클릭 이벤트
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			//드래그 연산
			DragSelect(touch);

			// 손가락을 뗐을 때
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
					if (hit.transform.TryGetComponent(out TestUnit unit))
					{
						unitController.ClickSelectUnit(unit);
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

		// 드래그 이벤트 - 시작
		if (touch.phase == TouchPhase.Began)
		{
			start = touch.position;
			dragRect = new Rect();
		}

		// 드래그 이벤트 - 드래그 중
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
		foreach (TestUnit unit in unitController.unitList)
		{
			if (dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
			{
				unitController.DragSelectUnit(unit);
			}
		}
	}
}
