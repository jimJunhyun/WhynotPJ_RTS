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
			delSec = 1f;
		}
		else if (difficulty == Diff.Normal)
		{
			delSec = 0.4f;
		}
		else if (difficulty == Diff.Hard)
		{
			delSec = 0.07f;
		}
		StartCoroutine(Nominate());
	}
	IEnumerator Nominate()
	{
		while (!breakFlag)
		{
			yield return new WaitForSeconds(delSec);
			EnemyBrain.instance.Decide();
		}
	}
}
