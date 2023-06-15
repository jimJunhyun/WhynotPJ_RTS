using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTime : MonoBehaviour
{
	public static ChangeTime instance;

    public int maxMinute;

	TextMeshProUGUI timeText;

	float curTime;
	float prevTime;
	float useTime => maxMinute * 60 - curTime;

	public string timeToTextMS 
	{ 
		get => $"{(((int)prevTime / 60 < 10) ? 0 : null)}{(int)prevTime / 60} : {(((int)prevTime % 60 < 10) ? 0 : null)}{(int)prevTime % 60}";
	}

	public string leftTimetoTextMS
	{
		get => $"{(((int)useTime / 60 < 10) ? 0 : null)}{(int)useTime / 60} : {(((int)useTime % 60 < 10) ? 0 : null)}{(int)useTime % 60}";
	}

	private void Awake()
	{

		instance = this;

		curTime = maxMinute * 60;
		prevTime = curTime;
		timeText = GameObject.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
		timeText.text = timeToTextMS;
	}

	private void Update()
	{
		curTime -= Time.deltaTime;
		if((int)prevTime - (int)curTime >= 1.0f)
		{
			prevTime = curTime;
			timeText.text = timeToTextMS;
			if(prevTime <= 0.0f)
			{
				Time.timeScale = 0f;
				Debug.Log("³¡");
			}
		}
	}
}
