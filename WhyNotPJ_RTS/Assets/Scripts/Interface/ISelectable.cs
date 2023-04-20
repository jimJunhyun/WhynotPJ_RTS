using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 선택 가능한 유닛을 위한 인터페이스
/// </summary>
public interface ISelectable
{
	public bool CanDragSelect { get; }
	public Vector3 WorldPos { get; }
	
	public void OnSelectUnit();
	public void OnDeselectUnit();
}
