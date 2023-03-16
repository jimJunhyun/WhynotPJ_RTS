using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Element
{
	public int vio;
	public int def;
	public int con;
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
			if (i == 2)
			{
				return con;
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
			if (i == 2)
			{
				con = value;
			}
			else
			{
				rec = value;
			}
		}
	}

	public Element(int v, int d, int c, int r)
	{
		vio = v;
		def = d;
		con = c;
		rec = r;
	}
}

public interface IProducable
{
	public string name { get;}
	public float produceTime{ get; }
	public Element element{ get;}

	public Action onCompleted{get; }
}

public class ProdList<T> : List<T>
	where T : IProducable
{
	/// <summary>
	/// IProducable ���� ��Ҹ� ������ �Ӽ� ������ ���� �������� �����Ѵ�.
	/// </summary>
	/// <param name="sortPrior">0 : ���ݼ� 1 : ���� 2 : �Ǽ��� 3 : ������, 0000 = 0 = �켱�� X, 1111 = 15 = ��ü ����</param>
	public void Sort(List<int> sortPrior)
	{
		for (int i = 0; i < Count; i++)
		{
			for (int j = i; j < Count; j++)
			{
				float iComp = 0, jComp = 0;
				for (int k = 0; k < sortPrior.Count; k++)
				{
					if(sortPrior[k] == 1)
					{
						iComp += this[i].element[k] * 2;
						jComp += this[j].element[k] * 2;
					}
					if (sortPrior[k] == 2)
					{
						iComp += this[i].element[k] * 1.5f;
						jComp += this[j].element[k] * 1.5f;
					}
					if (sortPrior[k] == 3)
					{
						iComp += this[i].element[k] * 1;
						jComp += this[j].element[k] * 1;
					}
					if (sortPrior[k] == 4)
					{
						iComp += this[i].element[k] * 0;
						jComp += this[j].element[k] * 0;
					}
				}
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

