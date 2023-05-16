using UnityEngine;

public class CameraSelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayer;
	[SerializeField] private RectTransform dragRectangle;	// 마우스로 드래그한 범위를 가시화하는 Image UI의 RectTransform

	private Rect dragRect;
	private Vector3 start = Vector2.zero;
	private Vector3 end = Vector2.zero;

    private Camera mainCam;
    private UnitSelectManager unitManager;	// 유닛의 선택 & 해제를 담당하는 UnitManager 클래스
	private UnitListUI unitListUI;			// 선택한 유닛의 정보를 출력하는 UI를 담당하는 클래스

	public bool isDraging = false;

	private void Awake()
	{
		mainCam = Camera.main;

		// 유닛매니저 생성,, 추후 선언 위치 변경 필요
		unitManager = new UnitSelectManager();
		unitListUI = GetComponent<UnitListUI>();

		DrawDragRectangle();
	}

	private void Update()
	{
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

				if (Physics.Raycast(ray, out hit, 100f, unitLayer))
				{
					if (hit.transform.TryGetComponent(out ISelectable unit))
					{
						UnitSelectManager.Instance.ClickSelectUnit(unit);
						print(UnitSelectManager.Instance.SelectedUnitList.Count);
						if (UnitSelectManager.Instance.SelectedUnitList.Count > 0)
						{
							unitListUI.ShowUnitInfo();
						}
						else if (!UnitSelectManager.Instance.IsSelecting)
						{
							unitListUI.HideUnitInfo();
						}
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

		if (UnitSelectManager.Instance.SelectedUnitList.Count > 0)
		{
			unitListUI.ShowUnitInfo();
		}
		else if (!UnitSelectManager.Instance.IsSelecting)
		{
			unitListUI.HideUnitInfo();
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

	/// <summary>
	/// dragRect의 범위 내에 있는 유닛을 선택하는 함수
	/// </summary>
	private void SelectUnits()
	{
		UnitSelectManager.Instance.DeselectAll();
		foreach (UnitController unit in UnitSelectManager.Instance.unitList)
		{
			if (unit.CanDragSelect && dragRect.Contains(mainCam.WorldToScreenPoint(unit.transform.position)))
			{
				UnitSelectManager.Instance.DragSelectUnit(unit);
			}
		}
	}
}
