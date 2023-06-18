using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
	public int preparedStageCount = 2;

	
	//도전 가능한 최고 스테이지
	int curMaxStage = 1;
	//현재 플레이중인 스테이지
	int? curProgressingStage = null;
	public SetStage Stage
	{
		get;
		set;
	}

	public int? progressingStage
	{ 
		get => curProgressingStage;
		set => curProgressingStage = value;
	} 
	public int maxStage => curMaxStage;

	private void Awake()
	{
		if(instance == null)
			instance = this;
			DontDestroyOnLoad(this);
	}

	public void NextStage()
	{
		if(curMaxStage == curProgressingStage && preparedStageCount >= curProgressingStage + 1)
		{
			Debug.Log("NEXTSTAGE");
			curMaxStage += 1;
		}
		
	}

	public void SetPlayingStage(int s, SetStage stgInfo)
	{
		curProgressingStage = s;
		Stage = stgInfo;
	}
}
