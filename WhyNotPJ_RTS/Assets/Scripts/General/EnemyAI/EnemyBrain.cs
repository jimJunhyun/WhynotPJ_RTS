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
	//[HideInInspector]
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
			if (producer != null && producer.produceQueue.Count == 0)
			{
				Examine();
				if (product.element.vio > biasMiddle)
				{
					set.violenceBias -= (product.element.vio - biasMiddle) / set.vioIncreaseBias / dynamicity;
				}
				else
				{
					set.violenceBias += set.vioIncreaseBias * (biasMiddle - product.element.vio) * dynamicity;
				}
				if (product.element.def > biasMiddle)
				{
					set.defendBias -= (product.element.def - biasMiddle) / set.defIncreaseBias / dynamicity;
				}
				else
				{
					set.defendBias += (biasMiddle - product.element.def) * set.defIncreaseBias * dynamicity;
				}
				if (product.element.rec > biasMiddle)
				{
					if (playerBase == null)
					{
						set.reconBias -= (product.element.rec - biasMiddle) / set.initReconIncreaseBias / dynamicity;
					}
					else
					{
						set.reconBias -= (product.element.rec - biasMiddle) / set.recIncreaseBias / dynamicity;
					}

				}
				else
				{
					if (playerBase == null)
					{
						set.reconBias += set.initReconIncreaseBias * (biasMiddle - product.element.rec) * dynamicity;
					}
					else
					{
						set.reconBias += set.recIncreaseBias * (biasMiddle - product.element.rec) * dynamicity;
					}
				}
				producer.AddProduct(product);
			}
		}
		
	}

	/// <summary>
	/// �����ŷ� �ǴܵǺ��� 1~10�ϰŶ�
	/// 5�α��ϵ�?
	/// 
	/// ���������������� �������.
	/// </summary>
	/// <param name="info"></param>
	public void ReactTo(List<UnitController> info)
	{
		if(info.Count == 0)
			return;

		float vioAvg = 0, defAvg = 0, recAvg = 0;

		for (int i = 0; i < info.Count; i++)
		{
			vioAvg += info[i]._element[0];
			defAvg += info[i]._element[1];
			recAvg += info[i]._element[2];
		}
		vioAvg /= info.Count;
		defAvg /= info.Count;
		recAvg /= info.Count;
		if(vioAvg >= set.fxblStandard)
		{
			set[0] += set.fxblIncrement * set.vioIncreaseBias;
		}
		if (defAvg >= set.fxblStandard)
		{
			set[1] += set.fxblIncrement * set.defIncreaseBias;
		}
		if (recAvg >= set.fxblStandard)
		{
			set[2] += set.fxblIncrement * set.recIncreaseBias;
		}
	}

	private void Awake()
	{
		instance = this;
		set.violenceBias = set.initVioBias;
		set.defendBias = set.initDefBias;
		set.reconBias = set.initRecBias;
		
		producer = FindObjectsOfType<Producer>().ToList<Producer>().Find((p)=>{ return !p.pSide; });

		producable.AddRange(Resources.LoadAll<UnitController>("Prefabs/"));

		//�ǹ��� �߰��ϱ�.
	}

}


