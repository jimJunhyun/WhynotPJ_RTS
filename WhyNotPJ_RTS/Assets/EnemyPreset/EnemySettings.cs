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
    [Header("�����")]
    public float initDefBias;
    public float defendBias;
    public float defIncreaseBias;
    [Header("������, �� ���� ���������� ���� ���� ����")]
    public float initRecBias;
    public float reconBias;
    public float recIncreaseBias;
    public float initReconIncreaseBias;

	public float this[int idx] 
    {
        get {
            float ret = 0;
            if(idx == 0)
			{
                ret = violenceBias;
			}
            else if(idx == 1)
			{
                ret = defendBias;
			}
            else if (idx == 2)
            {
                ret = reconBias;
            }
            return ret;
		}
		set
		{
            if (idx == 0)
            {
                violenceBias = value;
            }
            else if (idx == 1)
            {
                defendBias = value;
            }
            else if (idx == 2)
            {
                reconBias = value;
            }
        }
    }

    [Space(10)]
    [Header("��� ������ ������ �Ǵ��ϴ� ô��")]
    public float fxblStandard;
    [Header("�Ǵܿ� ���� �����Ǵ� ������ ����")]
    public float fxblIncrement;

    [Space(10)]

	[Header("���縦 �����ϴ� �Ӽ�.")]
    [Header("�������� ����")]
    public float warBias;
    [Header("�������� ����")]
    public float passiveBias;
    [Header("����� ���� �Ǵ� ����")]
    public int adequateSoldier;

    [Header("�������� ��ġ - ���� �߽ü�")]
    public float heightBias;
    [Header("�������� ��ġ - �Ÿ� �߽ü�")]
    public float distBias;
}
