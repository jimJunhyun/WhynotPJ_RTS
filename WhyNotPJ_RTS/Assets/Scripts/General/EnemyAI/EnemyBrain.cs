using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyBrain : MonoBehaviour
{
	public static EnemyBrain instance;

    public EnemySettings set;
	public float biasMiddle = 5f;
	public float dynamicity = 2f;

	public ProdList<IProducable> producable = new ProdList<IProducable>();
	IProducable product = null;
	List<Producer> produceActs = new List<Producer>(); //새로운 생산 시설이 생기면 해당 시설 생산 완료 부분에서 더해줌.
	//List<int> myPriority = new List<int>();
	[HideInInspector]
	public Transform playerBase;

	void Examine() //할 행동 목록 결정
	{
		//myPriority = CalcRank(new List<float>(){ set.violenceBias, set.defendBias, set.constructBias, set.reconBias });
		producable.Sort(set);
		product = producable[0];
	}
    
    public void Decide() //행동 실행
	{
		Examine();
		Producer p = produceActs.Find((x)=>{ return !x.isProducing;});
		if(p != null)
		{
			if (product._element.vio > biasMiddle)
			{
				set.violenceBias -= ( product._element.vio - biasMiddle) / set.vioIncreaseBias / dynamicity;
			}
			else
			{
				set.violenceBias += set.vioIncreaseBias * (biasMiddle - product._element.vio) * dynamicity;
			}
			if (product._element.def > biasMiddle)
			{
				set.defendBias -= (product._element.def - biasMiddle ) / set.defIncreaseBias / dynamicity;
			}
			else
			{
				set.defendBias += (biasMiddle - product._element.def ) * set.defIncreaseBias * dynamicity;
			}
			if (product._element.rec > biasMiddle)
			{
				if(playerBase == null)
				{
					set.reconBias -= ( product._element.rec - biasMiddle ) / set.initReconIncreaseBias / dynamicity;
				}
				else
				{
					set.reconBias -= (product._element.rec - biasMiddle ) / set.recIncreaseBias / dynamicity;
				}
				
			}
			else
			{
				if (playerBase == null)
				{
					set.reconBias += set.initReconIncreaseBias * (biasMiddle - product._element.rec) * dynamicity;
				}
				else
				{
					set.reconBias += set.recIncreaseBias * (biasMiddle - product._element.rec) * dynamicity;
				}
			}
			p.SetProduct(product);
		}
	}

	private void Awake()
	{
		instance = this;
		set.violenceBias = set.initVioBias;
		set.defendBias = set.initDefBias;
		set.reconBias = set.initRecBias;
		producable.Add(new ReconTower());
		producable.Add(new Barricade());
		producable.Add(new Scout());
		produceActs = FindObjectsOfType<Producer>().OfType<Producer>().ToList().FindAll(x => !x.pSide);
	}

	//public List<int> CalcRank(List<float> list)
	//{
	//	List<int> rank = new List<int>();

	//	for (int i = 0; i < list.Count; i++)
	//	{
	//		int r = 1;
	//		for (int j = 0; j < list.Count; j++)
	//		{
	//			if(i == j)
	//			{
	//				continue;
	//			}
	//			if(list[i] < list[j])
	//			{
	//				++r;
	//			}
	//		}
	//		rank.Add(r);
	//	}
	//	return rank;
	//}
}


