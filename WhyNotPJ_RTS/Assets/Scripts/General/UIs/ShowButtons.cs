using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowButtons : MonoBehaviour
{
    List<Button> stageButtons = new List<Button>();

	private void Start()
	{
		GetComponentsInChildren(stageButtons);
		for (int i = 0; i < stageButtons.Count; i++)
		{
			if(stageButtons[i].onClick.GetPersistentEventCount() == 0 || StageManager.instance.progressingStage <= i)
			{
				stageButtons[i].interactable = false;
			}
			else
			{
				stageButtons[i].interactable = true;
			}
		}
	}
}
