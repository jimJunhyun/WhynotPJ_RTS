using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ������ ���� �������̽�
/// </summary>
public interface ISelectable
{
	public bool CanDragSelect { get; }
	public Vector3 WorldPos { get; }
	
	public void OnSelectUnit();
	public void OnDeselectUnit();
}
