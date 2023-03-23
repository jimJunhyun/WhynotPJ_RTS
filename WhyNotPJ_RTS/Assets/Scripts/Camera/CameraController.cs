using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
	NONE,
	MOVING,
	DRAGSELECTING,
}

public class CameraController : MonoBehaviour
{
	public static CameraState camState;

	[Header("Reference")]
	[SerializeField] private Transform cameraTransform;
	private Camera mainCam;

	[Header("Movement")]
	[Range(0.01f, 1f)]
	[SerializeField] private float movementMultiply;		//기본 속도에 곱하기
	[Range(0.01f, 1f)]
	[SerializeField] private float movementTime;			//움직임 러프 값

	[Header("Zoom")]
	[Range(0.01f, 1f)]
	[SerializeField] private float zoomTime;
	[SerializeField] private int currentZoomValue;
	[SerializeField] private int minZoomValue;
	[SerializeField] private int maxZoomValue;
	private Vector3 zoomAmount;                             //카메라의 각도에 따라 자동으로 지정
	private float zoom = 0;

	[Header("Target Values")]
	public float cameraYOffset = 0;
	public Vector3 targetPosition;
	public Vector3 targetZoomPosition;

	private Plane plane;

	private void Start()
	{
		plane.SetNormalAndPosition(Vector3.up, transform.position);

		mainCam = Camera.main;

		//카메라의 목표 위치 & 줌 값을 현재 카메라의 위치 & 줌 값으로 초기화
		targetPosition = transform.position;
		targetZoomPosition = cameraTransform.localPosition;

		//카메라 각도에 맞추어 줌 증가값을 계산 후 초기화
		zoomAmount = new Vector3(0, -Mathf.Tan(cameraTransform.localEulerAngles.x * Mathf.Deg2Rad), 1);

		//zoomValue가 할당되어 있을 시에는 zoomValue에 값에 맞게 카메라 위치를 지정
		targetZoomPosition += zoomAmount * currentZoomValue;
		cameraTransform.localPosition = targetZoomPosition;
	}

	private void Update()
	{
		if (camState == CameraState.DRAGSELECTING) return;

		if (Input.touchCount >= 1)
		{
			plane.SetNormalAndPosition(transform.up, transform.position);
		}

		Vector3 delta1 = Vector3.zero;

		if (Input.touchCount == 1)
		{
			delta1 = PlanePositionDelta(Input.GetTouch(0));
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
				targetPosition += delta1 * movementMultiply;
		}

		//목표값 설정
		ZoomInOut();

		//목표값으로 이동
		targetPosition.y = cameraYOffset;
		transform.position = Vector3.Lerp(transform.position, targetPosition, movementTime);
		cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetZoomPosition, zoomTime);
	}

	//줌인 & 줌아웃
	private void ZoomInOut()
	{

		if (Input.touchCount >= 2)
		{
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			Vector3 pos1 = PlanePosition(touch1.position);
			Vector3 pos2 = PlanePosition(touch2.position);
			Vector3 pos1b = PlanePosition(touch1.position - touch1.deltaPosition);
			Vector3 pos2b = PlanePosition(touch2.position - touch2.deltaPosition);

			Vector3 zoomPos1 = touch1.position;
			Vector3 zoomPos2 = touch2.position;
			Vector3 zoomPos1b = touch1.position - touch1.deltaPosition;
			Vector3 zoomPos2b = touch2.position - touch2.deltaPosition;

			zoom += 1 - Vector3.Distance(zoomPos1, zoomPos2) / Vector3.Distance(zoomPos1b, zoomPos2b);

			currentZoomValue -= (int)zoom;

			if ((int)zoom != 0)
				zoom = 0;

			currentZoomValue = Mathf.Clamp(currentZoomValue, minZoomValue, maxZoomValue);
			targetZoomPosition = zoomAmount * currentZoomValue;

			if (pos2b != pos2)
				transform.RotateAround(transform.position, Vector3.up, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal) * 0.1f);
		}
		else 
			zoom = 0;
	}

	private Vector3 PlanePositionDelta(Touch touch)
	{
		if (touch.phase != TouchPhase.Moved)
			return Vector3.zero;

		Ray rayBefore = mainCam.ScreenPointToRay(touch.position - touch.deltaPosition);
		Ray rayNow = mainCam.ScreenPointToRay(touch.position);
		if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
			return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

		return Vector3.zero;
	}

	protected Vector3 PlanePosition(Vector2 screenPos)
	{
		var rayNow = mainCam.ScreenPointToRay(screenPos);
		if (plane.Raycast(rayNow, out var enterNow))
			return rayNow.GetPoint(enterNow);

		return Vector3.zero;
	}
}
