using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum OverMessage
{
	NONE = -1,
	Clear,
	TimeOver,
	Defeat,

}

public class GameOverUI : MonoBehaviour
{
	public static GameOverUI instance;

    Canvas canvas;
	TextMeshProUGUI timeText;
	TextMeshProUGUI rewText;

	TextMeshProUGUI reasonText;

	GameObject clearPanel;
	GameObject failPanel;

	private void Awake()
	{
		instance = this;

		canvas = GetComponent<Canvas>();
		timeText = GameObject.Find("ResultTimeText").GetComponent<TextMeshProUGUI>();
		rewText = GameObject.Find("Reward").GetComponent<TextMeshProUGUI>();
		reasonText = GameObject.Find("Reason").GetComponent<TextMeshProUGUI>();

		clearPanel = GameObject.Find("ClearBanner");
		failPanel = GameObject.Find("FailBanner");
		Off();
	}

	public void On(OverMessage message)
	{
		if (message == OverMessage.Clear)
		{
			timeText.text = ChangeTime.instance.leftTimetoTextMS;
			string str = StageManager.instance.Stage.additionalInfo;
			str = str.Remove(str.Length - 3, 3);
			rewText.text = str;
			StageManager.instance.NextStage();
			clearPanel.SetActive(true);
			failPanel.SetActive(false);
		}
		else
		{
			string str = "";
			switch (message)
			{
				case OverMessage.TimeOver:
					str = "시간 초과";
					break;
				case OverMessage.Defeat:
					str = "본진 파괴";
					break;
				default:
					break;
			}
			reasonText.text = str;
			clearPanel.SetActive(false);
			failPanel.SetActive(true);
		}
		canvas.enabled = true;
	}
	public void Off()
	{
		canvas.enabled = false;
	}
}
