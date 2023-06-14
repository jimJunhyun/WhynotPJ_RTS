using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeUnitCnt : MonoBehaviour
{
    TextMeshProUGUI unitText;
	int prevUnitAmt;

	private void Awake()
	{
		unitText = GameObject.Find("UnitCountTxt").GetComponent<TextMeshProUGUI>();
		prevUnitAmt = 0;
	}

	private void Update()
	{
		if(prevUnitAmt != UnitSelectManager.Instance.unitList.Count)
		{

		}
	}
}
