using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IControlable
{
	public void Move(Vector3 to);
	public bool underControl{ get;set;}
	public Vector3 pos{get; }
}
