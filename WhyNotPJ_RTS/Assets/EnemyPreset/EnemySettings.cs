using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Preset",menuName = "Create Enemy Preset",order = 1)]
public class EnemySettings : ScriptableObject
{
    [Header("수치가 높을 수록 해당 행동을 할 확률이 높아짐.")]

    [Header("공격성 - 기습, 전면전, 별동 전략")]
    public float initVioBias;
    public float violenceBias;
    
    public float vioSuddenBias;
    public float vioFightBias;
    public float vioDistractBias;
    public float vioIncreaseBias;
    [Header("건설적 - 생산형, 보조형 건물")]
    public float initConBias;
    public float constructBias;
    public float conProduceBias;
    public float conBuffBias;
    public float conIncreaseBias;
    [Header("방어적 - 장거리, 단거리 방어")]
    public float initDefBias;
    public float defendBias;
    public float defRangeBias;
    public float defNearBias;
    public float defIncreaseBias;
    [Header("정찰적 - 광범위, 장거리, 적본진 우선 정찰 + 사보타지")]
    public float initRecBias;
    public float reconBias;
    public float recLargeBias;
    public float recLongBias;
    public float recBaseBias;
    public float sabotageBias;
    public float recIncreaseBias;
}
