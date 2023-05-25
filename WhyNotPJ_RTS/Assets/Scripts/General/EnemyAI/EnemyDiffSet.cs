using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Diff
{
	None = -1,
	Easy,
	Normal,
	Hard,
}

public class EnemyDiffSet : MonoBehaviour
{
	public Diff difficulty;
	public bool breakFlag = false;
	
	
	float delSec = 0.2f;
	private void Awake()
	{ 
		if(difficulty == Diff.Easy)
		{
			delSec = 0.09f;
		}
		else if (difficulty == Diff.Normal)
		{
			delSec = 0.045f;
		}
		else if (difficulty == Diff.Hard)
		{
			delSec = 0;
		}
		StartCoroutine(Nominate());
	}
	IEnumerator Nominate()
	{
		while (!breakFlag)
		{
			if(delSec == 0)
				yield return null;
			else
				yield return new WaitForSeconds(delSec);
			EnemyBrain.instance.Decide();
			EnemyPosGen.instance.FindPlaying();
		}
	}
}
