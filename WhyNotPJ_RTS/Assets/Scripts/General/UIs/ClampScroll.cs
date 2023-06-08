using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampScroll : MonoBehaviour
{

	RectTransform rtT;
	public float minPos = -35;
	float maxPos;
	private void Awake()
	{
		rtT = GetComponent<RectTransform>();

		maxPos = rtT.childCount * 105f + 10f;
	}

	public void ClampPos()
	{
		
		rtT.localPosition=  new Vector3(transform.localPosition.x, 
			Mathf.Clamp(transform.localPosition.y, minPos, maxPos), 
			transform.localPosition.z);
	}
}
