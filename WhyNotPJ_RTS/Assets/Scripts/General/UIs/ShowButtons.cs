using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowButtons : MonoBehaviour
{
	public static ShowButtons instance;

    List<Button> stageButtons = new List<Button>();

	public Sprite onSelectedBtnImg;
	public Sprite onDeselectedBtnImg;

	[HideInInspector]
	public SetStage curStage;


	int preSel = 0;

	private void Awake()
	{
		instance = this;
		GetComponentsInChildren(stageButtons);
	}

	private void Start()
	{
		for (int i = 0; i < stageButtons.Count; i++)
		{
			if(stageButtons[i].onClick.GetPersistentEventCount() == 0 || StageManager.instance.maxStage <= i)
			{
				stageButtons[i].interactable = false;
			}
			else
			{
				stageButtons[i].interactable = true;
			}
		}
	}

	public void SetSelected(int stgDeg, SetStage stg)
	{
		if(stgDeg != preSel)
		{
			if(preSel != 0)
				stageButtons[preSel - 1].image.sprite = onDeselectedBtnImg;
			stageButtons[stgDeg - 1].image.sprite = onSelectedBtnImg;
			preSel = stgDeg;
		}
		curStage = stg;
	}
}
