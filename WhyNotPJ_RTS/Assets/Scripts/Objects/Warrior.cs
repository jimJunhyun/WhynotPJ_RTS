using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour, IProducable
{
    [SerializeField]
    private string myName;
    [SerializeField]
    private float produceTime;
    [SerializeField]
    private int vio, def, rec;
    private Action onCompleted;

    string IProducable._myName => myName;
    float IProducable._produceTime => produceTime;
    public Element _element => new Element(vio, def, rec);
    Action IProducable._onCompleted => onCompleted;
}
