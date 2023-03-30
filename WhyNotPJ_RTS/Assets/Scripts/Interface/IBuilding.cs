using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public List<UnitController>  nearUnit { get; set;}

    public Vector3 scale { get; }
    public Vector3 pos { get;set;} // 현위치. 나중에 건물 옮기기가 나올수도?
}
