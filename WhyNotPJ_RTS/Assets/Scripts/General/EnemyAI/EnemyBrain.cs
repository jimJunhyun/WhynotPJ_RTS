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

	public ProdList<IProducable> producable = new ProdList<IProducable>(); // ���⿡ ���� ������ ��� ������Ʈ�� ���ؾ���.
	IProducable product = null;
	Producer producer; 
	[HideInInspector]
	public Transform playerBase;

	void Examine() //�� �ൿ ��� ����  
	{
		//��� ���� �ν��� �Ŀ� �װſ� �°� ���� ������ ���� ����
		producable.Sort(set);
		product = producable[0];
	}
    
    public void Decide() //�ൿ ����
	{
		if(producable.Count > 0)
		{
			Examine();

			if (producer != null)
			{
				if (product._element.vio > biasMiddle)
				{
					set.violenceBias -= (product._element.vio - biasMiddle) / set.vioIncreaseBias / dynamicity;
				}
				else
				{
					set.violenceBias += set.vioIncreaseBias * (biasMiddle - product._element.vio) * dynamicity;
				}
				if (product._element.def > biasMiddle)
				{
					set.defendBias -= (product._element.def - biasMiddle) / set.defIncreaseBias / dynamicity;
				}
				else
				{
					set.defendBias += (biasMiddle - product._element.def) * set.defIncreaseBias * dynamicity;
				}
				if (product._element.rec > biasMiddle)
				{
					if (playerBase == null)
					{
						set.reconBias -= (product._element.rec - biasMiddle) / set.initReconIncreaseBias / dynamicity;
					}
					else
					{
						set.reconBias -= (product._element.rec - biasMiddle) / set.recIncreaseBias / dynamicity;
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
				if (producer.produceQueue.Count == 0)
				{

					producer.AddProduct(product);

				}

			}
		}
		
	}

	private void Awake()
	{
		instance = this;
		set.violenceBias = set.initVioBias;
		set.defendBias = set.initDefBias;
		set.reconBias = set.initRecBias;
		
		producer = FindObjectsOfType<Producer>().ToList<Producer>().Find((p)=>{ return !p.pSide; });

		producable.AddRange(Resources.LoadAll<UnitDefault>("Prefabs/"));

		//�ǹ��� �߰��ϱ�.
	}

}


