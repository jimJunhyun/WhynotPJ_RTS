using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyBrain : MonoBehaviour
{
	public static EnemyBrain instance;

    public EnemySettings set;
	public ProdList<IProducable> producable = new ProdList<IProducable>();
	IProducable product = null;
	List<Producer> produceActs = new List<Producer>(); //새로운 생산 시설이 생기면 해당 시설 생산 완료 부분에서 더해줌.
	List<int> myPriority = new List<int>();

	void Examine() //할 행동 목록 결정
	{
		myPriority = CalcRank(new List<float>(){ set.violenceBias, set.defendBias, set.constructBias, set.reconBias });
		producable.Sort(myPriority);
		Debug.Log("1 : " + producable[0].name);
		Debug.Log("2 : " + producable[1].name);
		product = producable[0];
	}
    
    public void Decide() //행동 실행
	{
		Examine();
		Producer p = produceActs.Find((x)=>{ return !x.isProducing;});
		if(p != null)
		{
			if(product.element.vio > 2)
			{
				set.violenceBias -= product.element.vio / set.vioIncreaseBias;
			}
			else
			{
				set.violenceBias += set.vioIncreaseBias * (product.element.vio + 1);
			}
			if(product.element.def > 2)
			{
				set.defendBias -= product.element.def / set.defIncreaseBias;
			}
			else
			{
				set.defendBias += set.defIncreaseBias * (product.element.def + 1);
			}
			if (product.element.con > 2)
			{
				set.constructBias -= product.element.con / set.conIncreaseBias;
			}
			else
			{
				set.constructBias += set.conIncreaseBias * (product.element.con + 1);
			}
			if (product.element.rec > 2)
			{
				set.reconBias -= product.element.rec / set.recIncreaseBias;
			}
			else
			{
				set.reconBias += set.recIncreaseBias * (product.element.rec + 1);
			}

			p.SetProduct(product);
		}
	}

	private void Awake()
	{
		instance = this;
		set.violenceBias = set.initVioBias;
		set.defendBias = set.initDefBias;
		set.constructBias = set.initConBias;
		set.reconBias = set.initRecBias;
		producable.Add(new Warrior());
		producable.Add(new ReconTower());
		produceActs = FindObjectsOfType<Producer>().OfType<Producer>().ToList().FindAll(x => !x.pSide);
	}

	public List<int> CalcRank(List<float> list)
	{
		List<int> rank = new List<int>();

		for (int i = 0; i < list.Count; i++)
		{
			int r = 1;
			for (int j = 0; j < list.Count; j++)
			{
				if(i == j)
				{
					continue;
				}
				if(list[i] < list[j])
				{
					++r;
				}
			}
			rank.Add(r);
		}
		return rank;
	}
}


