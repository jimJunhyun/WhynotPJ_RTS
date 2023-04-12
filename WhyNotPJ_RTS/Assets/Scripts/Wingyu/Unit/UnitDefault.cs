using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour, IUnit, ISelectable
{
	public abstract string myName { get; }
	public abstract float produceTime { get; }
	public abstract GameObject prefab { get; }
	public abstract Element element { get; }
	public abstract Action onCompleted { get; }

	public abstract UnitState state { get; set; }
	public abstract Vector3 objPos { get; set; }
	public abstract Transform target { get; set; }

	public abstract bool CanDragSelect { get; }
	public Vector3 WorldPos => transform.position;

	public virtual void OnSelectUnit()
	{
		return;
	}

	public virtual void OnDeselectUnit()
	{
		return;
	}
}
