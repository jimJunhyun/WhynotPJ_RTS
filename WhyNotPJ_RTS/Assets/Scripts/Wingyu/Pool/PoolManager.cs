using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager Instance;

	[SerializeField] private PoolListSO poolingList;

    private Dictionary<string, Pool<MonoBehaviour>> pools = new Dictionary<string, Pool<MonoBehaviour>>();

	private void Awake()
	{
		Instance = this;
		foreach (PoolingObject po in poolingList.list)
		{
			CreatePool(po.prefab, transform, po.count);
		}
	}

	public void CreatePool(MonoBehaviour prefab, Transform parent, int count)
	{
		pools.Add(prefab.gameObject.name, new Pool<MonoBehaviour>(prefab, parent, count));
	}

	public MonoBehaviour Pop(string name)
	{
		if (!pools.ContainsKey(name))
		{
			Debug.LogError($"No key about {name}");
			return null;
		}

		MonoBehaviour obj = pools[name].Pop();
		return obj;
	}

	public void Push(MonoBehaviour obj)
	{
		pools[obj.gameObject.name].Push(obj);
	}
}
