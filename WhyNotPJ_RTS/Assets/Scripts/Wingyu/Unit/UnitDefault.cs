using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour, ISelectable, IProducable
{
	public abstract string myName { get; }
	public abstract float produceTime { get; }
	public abstract GameObject prefab { get; }
	public abstract Element element { get; }
	public abstract Action onCompleted { get; }

	public abstract bool isPlayer { get; }
	public abstract float healthPoint { get; }

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
