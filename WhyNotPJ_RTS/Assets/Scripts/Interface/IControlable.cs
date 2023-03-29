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
	public Vector3 objPos{get; set;}
	public Transform target { get; set;}
}
