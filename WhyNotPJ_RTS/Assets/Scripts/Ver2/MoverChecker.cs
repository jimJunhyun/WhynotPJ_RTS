using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public class AttackRanges
{
    public Dir direction;
    public int attack;
}

public class MoverChecker : MonoBehaviour
{
    public float rayDist = 0.5f;

    public List<AttackRanges> ranges;

	private void Update()
	{
		for (int i = 0; i < ranges.Count; i++)
		{
            Vector2 direction = Vector2.zero;
			RaycastHit2D hit;
			switch (ranges[i].direction)
			{
				case Dir.Up:
					direction = Vector2.up;
					break;
				case Dir.Down:
					direction = Vector2.down;
					break;
				case Dir.Left:
					direction = Vector2.left;
					break;
				case Dir.Right:
					direction = Vector2.right;
					break;
			}


			if(hit = Physics2D.Raycast(transform.position, direction, rayDist))
			{
				UnitMover unit;
				if (unit = hit.collider.GetComponent<UnitMover>())
				{
					unit.hp -= ranges[i].attack;
				}
			}
		}
	}
}
