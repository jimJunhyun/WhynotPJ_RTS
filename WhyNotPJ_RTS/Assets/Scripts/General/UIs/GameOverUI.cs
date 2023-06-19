using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    Canvas canvas;
	TextMeshProUGUI timeText;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		timeText = GameObject.Find("ResultTimeText").GetComponent<TextMeshProUGUI>();
		Off();
	}

	public void On()
	{
		timeText.text = ChangeTime.instance.leftTimetoTextMS;
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
