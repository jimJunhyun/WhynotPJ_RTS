using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Preset",menuName = "Create Enemy Preset",order = 1)]
public class EnemySettings : ScriptableObject
{
    [Header("��ġ�� ���� ���� �ش� �ൿ�� �� Ȯ���� ������.")]

    [Header("���ݼ�")]
    public float initVioBias;
    public float violenceBias;
    public float vioIncreaseBias;
    [Header("�Ǽ���")]
    public float initConBias;
    public float constructBias;
    public float conIncreaseBias;
    [Header("�����")]
    public float initDefBias;
    public float defendBias;
    public float defIncreaseBias;
    [Header("������, �� ���� ���������� ���� ���� ����")]
    public float initRecBias;
    public float reconBias;
    public float recIncreaseBias;
    public float initReconIncreaseBias;

    [Header("���縦 �����ϴ� �Ӽ�.")]
    [Header("�������� ����")]
    public float warBias;
    [Header("�������� ����")]
    public float passiveBias;
}
