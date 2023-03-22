using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Preset",menuName = "Create Enemy Preset",order = 1)]
public class EnemySettings : ScriptableObject
{
    [Header("수치가 높을 수록 해당 행동을 할 확률이 높아짐.")]

    [Header("공격성")]
    public float initVioBias;
    public float violenceBias;
    public float vioIncreaseBias;
    [Header("건설적")]
    public float initConBias;
    public float constructBias;
    public float conIncreaseBias;
    [Header("방어적")]
    public float initDefBias;
    public float defendBias;
    public float defIncreaseBias;
    [Header("정찰적, 적 본진 정찰까지의 정찰 증가 편향")]
    public float initRecBias;
    public float reconBias;
    public float recIncreaseBias;
    public float initReconIncreaseBias;

    [Header("병사를 지휘하는 속성.")]
    [Header("공격적인 지휘")]
    public float warBias;
    [Header("수비적인 지휘")]
    public float passiveBias;
}
