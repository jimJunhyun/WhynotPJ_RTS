using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInfo : MonoBehaviour
{
	public List<SetStage> stages;
    SetStage stage;
	public SetStage Stage => stage;

	TextMeshProUGUI stageName;
	Image mapImg;
	TextMeshProUGUI detailInfo;

	List<Image> visuals = new List<Image>();
	List<TextMeshProUGUI> txts = new List<TextMeshProUGUI>();

	int curSel = 1;

	private void Awake()
	{
		stageName = GameObject.Find("StageName").GetComponent<TextMeshProUGUI>();
		mapImg = GameObject.Find("MapPreviewImg").GetComponent<Image>();
		detailInfo = GameObject.Find("DetailInfo").GetComponent<TextMeshProUGUI>();
		GetComponentsInChildren(visuals);
		GetComponentsInChildren(txts);
		
		SetInfo(StageManager.instance.progressingStage == null || StageManager.instance.progressingStage == 0 ? 1 : (int)StageManager.instance.progressingStage);
		Debug.Log("Loaded");
	}

	void Off()
	{
		for (int i = 0; i < visuals.Count; i++)
		{
			visuals[i].enabled = false;
		}
		for (int i = 0; i < txts.Count; i++)
		{
			txts[i].enabled = false;
		}
	}
	void On()
	{
		for (int i = 0; i < visuals.Count; i++)
		{
			visuals[i].enabled = true;
		}
		for (int i = 0; i < txts.Count; i++)
		{
			txts[i].enabled = true;
		}
	}

	public void SetInfo(int order)
	{
		stage = stages[order - 1];
		stageName.text = stage.stageName;
		mapImg.sprite = stage.image;
		
		detailInfo.text = stage.additionalInfo;
		SceneChanger.instance.SetLoadImage(stage.image);
		ShowButtons.instance.SetSelected(order, stage);
		curSel = order;
		On();
	}

	public void StartGame()
	{
		if(stage == null)
			return;
		StageManager.instance.SetPlayingStage(curSel, stage);
		SceneChanger.instance.Change(stage.sceneName);
	}
}
