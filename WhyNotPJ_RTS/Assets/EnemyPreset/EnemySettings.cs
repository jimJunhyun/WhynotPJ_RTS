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
    [Header("방어적")]
    public float initDefBias;
    public float defendBias;
    public float defIncreaseBias;
    [Header("정찰적, 적 본진 정찰까지의 정찰 증가 편향")]
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
    [Header("상대 유닛의 성향을 판단하는 척도")]
    public float fxblStandard;
    [Header("판단에 따라 증감되는 성향의 정도")]
    public float fxblIncrement;

    [Space(10)]

	[Header("병사를 지휘하는 속성.")]
    [Header("공격적인 지휘")]
    public float warBias;
    [Header("수비적인 지휘")]
    public float passiveBias;
    [Header("충분한 물량 판단 기준")]
    public int adequateSoldier;

    [Header("수비적인 위치 - 높이 중시성")]
    public float heightBias;
    [Header("수비적인 위치 - 거리 중시성")]
    public float distBias;
}
