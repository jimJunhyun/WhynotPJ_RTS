using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public float moveGap;
    public float moveDist = 1;

	public float rayDist = 0.5f;

	public int hp;
	public int CurHp
	{
		get
		{
			int sum = 0;
			for (int i = 0; i < attackedBy.Count; i++)
			{
				sum += attackedBy[i].atk;
			}
			return hp - sum;
		}
	}

	public bool controlable = false;

	public List<AttackRange> attackedBy;

	bool movable = true;
	float prevMove;

	private void Awake()
	{
		prevMove = 0;
	}

	private void Update()
	{
		if (controlable)
		{
			if (Time.time - prevMove >= moveGap)
			{
				movable = true;
			}
			if (movable)
			{
				RaycastHit2D hit;
				if (Input.GetAxisRaw("Horizontal") > 0)
				{
					if (!Physics2D.Raycast(transform.position + Vector3.right * (transform.localScale.x / 1.8f), Vector3.right, rayDist))
					{
						transform.eulerAngles = new Vector3(0, 0, 0);
						transform.Translate(new Vector3(moveDist, 0), Space.Self);
						movable = false;
						prevMove = Time.time;
					}

				}
				else if (Input.GetAxisRaw("Horizontal") < 0)
				{
					if (!(hit = Physics2D.Raycast(transform.position - Vector3.right * (transform.localScale.x / 1.8f), Vector3.left, rayDist)))
					{
						transform.eulerAngles = new Vector3(0, 0, 180);
						transform.Translate(new Vector3(moveDist, 0), Space.Self);
						movable = false;
						prevMove = Time.time;
					}
				}
				else if (Input.GetAxisRaw("Vertical") > 0)
				{
					if (!Physics2D.Raycast(transform.position + Vector3.up * (transform.localScale.y / 1.8f), Vector3.up, rayDist))
					{
						transform.eulerAngles = new Vector3(0, 0, 90);
						transform.Translate(new Vector3(moveDist, 0), Space.Self);
						movable = false;
						prevMove = Time.time;
					}
				}
				else if (Input.GetAxisRaw("Vertical") < 0)
				{
					if (!Physics2D.Raycast(transform.position - Vector3.up * (transform.localScale.y / 1.8f), Vector3.down, rayDist))
					{
						transform.eulerAngles = new Vector3(0, 0, 270);
						transform.Translate(new Vector3(moveDist, 0), Space.Self);
						movable = false;
						prevMove = Time.time;
					}
				}

			}
		}
		
		
		
	}

	private void LateUpdate()
	{
		if (CurHp <= 0)
		{
			Destroy(gameObject);
		}
	}
}
