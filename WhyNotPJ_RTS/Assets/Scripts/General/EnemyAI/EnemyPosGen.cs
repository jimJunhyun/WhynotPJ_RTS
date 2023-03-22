using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public EnemySettings set;
	public Transform basePos;
	public float randomAmount;

	public List<IControlable> myControls = new List<IControlable>();

	private void Awake()
	{
		instance = this;
	}

	public Vector3 SamplePos(Vector3 fromPos)
	{
		Vector3 res = Vector3.zero;

		//적 본진이 있다면 그 근처로 이동
		//적 본진이 있다면

		return res;
	}

	public void FindPlaying()
	{
		if(myControls.Count > 0)
		{
			IControlable curC = myControls.Find((x) => { return !x.underControl; });
			if (curC != null)
			{
				Vector3 randoms = Random.insideUnitSphere * randomAmount;
				randoms.y = 0;
				Vector3 dir = SamplePos(curC.pos) + randoms;
				curC.Move(dir);
				
			}
			
		}
		
	}
}
