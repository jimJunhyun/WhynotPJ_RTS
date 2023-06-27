using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum AnomalyIndex
{
    None = -1,
    Dizzy,
    Empower,
    Vital,
    Revive,
    Charge,
}

class InflictedAnomaly
{
    public Anomaly info;
    public int stacks;

}

[System.Serializable]
class Anomaly
{
    public int Id;
    public string name;
    public bool isBuff;
    public int minActivate;
    public int maxActivate;
}

[CreateAssetMenu(fileName = "Status Anomaly Table")]
class Anomalies : ScriptableObject
{
    public List<Anomaly> allAnomalies;
}
