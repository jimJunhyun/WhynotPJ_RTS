using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVisuals : MonoBehaviour
{
    GameObject visual;
	UnitController unitCtrl;

	bool curSeenState = false;

	private void Awake()
	{
		unitCtrl = GetComponent<UnitController>();
		
		visual = GetComponentInChildren<Animator>().gameObject;
	}

	private void Start()
	{
		curSeenState = unitCtrl.isSeen();
	}

	private void Update()
	{
		if (!unitCtrl.isPlayer &&unitCtrl.isSeen() != curSeenState)
		{
			curSeenState = unitCtrl.isSeen();
			visual.SetActive(curSeenState);
		}
	}

}
