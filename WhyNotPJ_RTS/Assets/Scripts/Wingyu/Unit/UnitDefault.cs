using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour, ISelectable, IProducable
{
	public abstract string _myName { get; }
	public abstract float _produceTime { get; }
	public abstract GameObject _prefab { get; }
	public abstract Element _element { get; }
	public abstract Action _onCompleted { get; }

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
