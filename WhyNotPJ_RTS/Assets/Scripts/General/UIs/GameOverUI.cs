using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    Canvas canvas;
	TextMeshProUGUI timeText;
	TextMeshProUGUI rewText;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		timeText = GameObject.Find("ResultTimeText").GetComponent<TextMeshProUGUI>();
		rewText = GameObject.Find("Reward").GetComponent<TextMeshProUGUI>();
		Off();
	}

	public void On()
	{
		timeText.text = ChangeTime.instance.leftTimetoTextMS;
		string str = StageManager.instance.Stage.additionalInfo;
		str = str.Remove(str.Length - 3, 3);
		rewText.text = str;
		StageManager.instance.NextStage();
		canvas.enabled = true;
	}
	public void Off()
	{
		canvas.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			On();
		}
	}
}
