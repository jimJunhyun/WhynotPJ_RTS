using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitCounter : MonoBehaviour
{
	public static UnitCounter instance;

	TextMeshProUGUI unitCountTxt;
    int unitNum = 0;

	private void Awake()
	{
		instance = this;
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
