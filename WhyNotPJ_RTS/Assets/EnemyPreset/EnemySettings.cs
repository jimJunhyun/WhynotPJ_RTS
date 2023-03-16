using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Preset",menuName = "Create Enemy Preset",order = 1)]
public class EnemySettings : ScriptableObject
{
    [Header("��ġ�� ���� ���� �ش� �ൿ�� �� Ȯ���� ������.")]

    [Header("���ݼ� - ���, ������, ���� ����")]
    public float initVioBias;
    public float violenceBias;
    
    public float vioSuddenBias;
    public float vioFightBias;
    public float vioDistractBias;
    public float vioIncreaseBias;
    [Header("�Ǽ��� - ������, ������ �ǹ�")]
    public float initConBias;
    public float constructBias;
    public float conProduceBias;
    public float conBuffBias;
    public float conIncreaseBias;
    [Header("����� - ��Ÿ�, �ܰŸ� ���")]
    public float initDefBias;
    public float defendBias;
    public float defRangeBias;
    public float defNearBias;
    public float defIncreaseBias;
    [Header("������ - ������, ��Ÿ�, ������ �켱 ���� + �纸Ÿ��")]
    public float initRecBias;
    public float reconBias;
    public float recLargeBias;
    public float recLongBias;
    public float recBaseBias;
    public float sabotageBias;
    public float recIncreaseBias;
}
