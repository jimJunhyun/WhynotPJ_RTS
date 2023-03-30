using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Element
{
	public int vio;
	public int def;
	public int rec;

	public int this[int i]
	{
		get
		{
			if (i == 0)
			{
				return vio;
			}
			if (i == 1)
			{
				return def;
			}
			else
			{
				return rec;
			}
		}

		set 
		{
			if (i == 0)
			{
				vio = value;
			}
			if (i == 1)
			{
				def = value;
			}
			else
			{
				rec = value;
			}
		}
	}

	public Element(int v, int d, int r)
	{
		vio = v;
		def = d;
		rec = r;
	}
}

public interface IProducable
{
	public string myName { get;}
	public float produceTime{ get; }
	public GameObject prefab { get; }
	public Element element{ get;}

	public Action onCompleted{get; }

	public float this[int idx]
	{
		get
		{
			float ret = 0;
			if(idx == 0)
				ret = element.vio;
			if (idx == 1)
				ret = element.def;
			if (idx == 2)
				ret = element.rec;

			return ret;
		}
	}
}

public class ProdList<T> : List<T>
	where T : IProducable
{
	/// <summary>
	/// IProducable 내의 요소를 성향을 곱하여 내림차순 정렬한다.
	/// </summary>
	public void Sort(EnemySettings set)
	{
		for (int i = 0; i < Count; i++)
		{
			for (int j = i; j < Count; j++)
			{
				float iComp = 0, jComp = 0;
				iComp += this[i][0] * set[0];
				iComp += this[i][1] * set[1];
				iComp += this[i][2] * set[2];


				jComp += this[j][0] * set[0];
				jComp += this[j][1] * set[1];
				jComp += this[j][2] * set[2];
				if (iComp < jComp)
				{
					T temp = this[i];
					this[i] = this[j];
					this[j] = temp;
				}
			}
		}
	}
}

