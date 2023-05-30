using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager Instance;

	[SerializeField] private PoolListSO poolingList;

    private Dictionary<string, Pool<PoolableMono>> pools = new Dictionary<string, Pool<PoolableMono>>();

	private void Awake()
	{
		Instance = this;
		foreach (PoolingObject po in poolingList.list)
		{
			CreatePool(po.prefab, transform, po.count);
		}
	}

	public void CreatePool(PoolableMono prefab, Transform parent, int count)
	{
		pools.Add(prefab.gameObject.name, new Pool<PoolableMono>(prefab, parent, count));
	}

	public PoolableMono Pop(string name)
	{
		if (!pools.ContainsKey(name))
		{
			Debug.LogError($"No key about {name}");
			return null;
		}

		PoolableMono obj = pools[name].Pop();
		return obj;
	}

	public void Push(PoolableMono obj)
	{
		pools[obj.gameObject.name].Push(obj);
	}
}
