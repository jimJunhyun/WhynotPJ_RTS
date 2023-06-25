using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public float moveGap;
    public float moveDist = 1;

	public int hp;
	

	bool movable = true;
	float prevMove;

	private void Awake()
	{
		prevMove = 0;
	}

	private void Update()
	{
		
		if(Time.time - prevMove >= moveGap)
		{
			prevMove = Time.time;
			movable = true;
		}
		if (movable)
		{
			if (Input.GetAxisRaw("Horizontal") > 0)
			{
				transform.Translate(new Vector3(moveDist, 0));
				movable = false;
			}
			else if (Input.GetAxisRaw("Horizontal") < 0)
			{
				transform.Translate(new Vector3(-moveDist, 0));
				movable = false;
			}
			else if (Input.GetAxisRaw("Vertical") > 0)
			{
				transform.Translate(new Vector3(0, moveDist));
				movable = false;
			}
			else if (Input.GetAxisRaw("Vertical") < 0)
			{
				transform.Translate(new Vector3(0, -moveDist));
				movable = false;
			}
			
		}
		if(hp <= 0)
		{
			Destroy(gameObject);
		}
		
	}
}
