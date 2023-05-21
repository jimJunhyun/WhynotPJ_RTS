using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolingObject
{
    public PoolableMono prefab;
    public int count;
}

[CreateAssetMenu(menuName = "SO/PoolList")]
public class PoolListSO : ScriptableObject
{
    public List<PoolingObject> list;
}
