using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ProduceableList")]
public class ProduceableListSO : ScriptableObject
{
	public List<UnitController> list = new List<UnitController>();
}
