using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampScroll : MonoBehaviour
{

	RectTransform rtT;
	public float minPos = -35;

	private void Awake()
	{
		rtT = GetComponent<RectTransform>();
	}

	public void ClampPos()
	{
		
		rtT.localPosition=  new Vector3(transform.localPosition.x, 
			Mathf.Max(transform.localPosition.y, minPos), 
			transform.localPosition.z);
	}
}
