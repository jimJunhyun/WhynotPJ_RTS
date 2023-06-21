using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitCounter : MonoBehaviour
{
	TextMeshProUGUI unitCountTxt;
    int unitNum = 0;

	private void Awake()
	{
		unitCountTxt = GameObject.Find("UnitCountTxt").GetComponent<TextMeshProUGUI>();
		unitCountTxt.text = unitNum.ToString();
	}

	public void AddUnit()
	{
		unitNum += 1;
		unitCountTxt.text = unitNum.ToString();
	}

	public void DecreaseUnit()
	{
		unitNum -= 1;
		unitCountTxt.text = unitNum.ToString();
	}
}
