using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public List<IUnit>  nearUnit { get; set;}

    public Vector3 scale { get; }
    public Vector3 pos { get;set;} // ����ġ. ���߿� �ǹ� �ű�Ⱑ ���ü���?
}
