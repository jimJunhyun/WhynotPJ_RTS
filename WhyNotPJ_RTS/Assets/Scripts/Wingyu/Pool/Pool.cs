using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
	private Stack<T> pools = new Stack<T>();
	private T prefab;
	private Transform parent;

	public Pool(T prefab, Transform parent, int count)
	{
		this.prefab = prefab;
		this.parent = parent;

		if (prefab == null)
			Debug.Log("Prefab NULL");
		if (parent == null)
			Debug.Log("prefab NULL");

		for (int i = 0; i < count; i++)
		{
			T obj = GameObject.Instantiate(prefab, parent);
			obj.name = obj.name.Replace("(Clone)", "");
			obj.gameObject.SetActive(false);
			pools.Push(obj);
		}
	}

	public T Pop()
	{
		T obj = null;

		if (pools.Count <= 0)
		{
			obj = GameObject.Instantiate(prefab, parent);
			obj.name = obj.name.Replace("(Clone)", "");
		}
		else
		{
			obj = pools.Pop();
			obj.gameObject.SetActive(true);
		}

		return obj;
	}

    public void Push(T obj)
	{
		obj.gameObject.SetActive(false);
		pools.Push(obj);
	}
}
