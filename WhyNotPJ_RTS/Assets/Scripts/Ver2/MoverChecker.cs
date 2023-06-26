using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class AttackRange
{
    public int xDistance;
    public int yDistance;
    public int atk;
}

public class MoverChecker : MonoBehaviour
{
    public List<AttackRange> ranges;
	Dictionary<AttackRange, UnitMover> rangeAttackingPair = new Dictionary<AttackRange, UnitMover>();




	private void Update()
	{
		for (int i = 0; i < ranges.Count; i++)
		{
			if (CheckCell(ranges[i], out UnitMover foundUnit))
			{
				if (!rangeAttackingPair.ContainsKey(ranges[i]))
				{
					foundUnit.attackedBy.Add(ranges[i]);
					rangeAttackingPair.Add(ranges[i], foundUnit);
				}
			}
			else
			{
				if (rangeAttackingPair.ContainsKey(ranges[i]))
				{
					rangeAttackingPair[ranges[i]].attackedBy.Remove(ranges[i]);
					rangeAttackingPair.Remove(ranges[i]);
				}
			}
		}
	}

	bool CheckCell(AttackRange rng, out UnitMover mover)
	{
		mover = null;
		Vector3 dest = transform.position + (transform.right * rng.xDistance) + (transform.up * rng.yDistance);
		Collider2D c;
		if(c = Physics2D.OverlapBox(dest, Vector2.one * 0.8f, 0))
		{
			if (mover = c.GetComponent<UnitMover>())
			{
				return true;
			}
		}
		
		return false;
		
	}

	public void OnDrawGizmos()
	{
		for (int i = 0; i < ranges.Count; i++)
		{
			Vector3 dest = transform.position + (transform.right * ranges[i].xDistance) + (transform.up * ranges[i].yDistance);
			Gizmos.DrawWireCube(dest, Vector3.one * 0.9f);
		}
	}
}
