using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
	Wait = 0,
	Alert,
	Moving,
	Fight,

	Dead
}

public interface IControlable
{
	public void Move(Vector3 to)
	{
		objPos = to;
	}
	public void Move(Transform target)
	{
		this.target = target;
	}
	public UnitState state{ get; set; }
	public Vector3 objPos{
		get
		{ 
			if(target == null)
			{
				return objPos;
			}
			else
			{
				return target.position;
			}
		}
		set
		{
			if (target != null)
			{
				target = null;	
			}
			objPos = value;
		}
	}
	public Transform target { get; set;}
}
