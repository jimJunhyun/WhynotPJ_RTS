using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
	public int preparedStageCount = 2;

	//도전 가능한 최고 스테이지
	int curProgressingStage = 1;

	public int progressingStage => curProgressingStage;

	private void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);
	}

	public void NextStage()
	{
		if(preparedStageCount < curProgressingStage)
		{

			curProgressingStage += 1;
		}
		
	}
}
