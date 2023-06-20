using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;




/// <summary>
/// ���� �����ϴ� ���� (�߿䵵 : �Ʊ� ������ �ս� ���� <= ������ �ɸ��� �ð� ����)
///		�켼 : ������ + ������ �߰�
///		���� : ��ġ�� ��� 
///		���� : ������ (������ ���� ���� = �� ����)
///	���� ����� ���� (�߿䵵 : �Ʊ� ������ �ս� ���� >= ������ �ɸ��� �ð� ����)
///		�켼 : ???(������ �Ҹ� �ð����� ������ ����� ������ ���̰� ����� �� ����.)
///		���� : ���
///		���� : �Ը��� (�ǵ����� ����� --> �Ҹ� ����)
///	���� ���̴� ���� (���� ��ü�� �ľ��ϱ� ��ư� ��.)
///		�켼 : ������ ���� (������ ������ ���ɼ��� ����.)
///		���� : �絿 ����
///		���� : ������ ���� (�ٸ� ������ �����Ͽ� �÷��̾ ����.)
/// 
/// ����, ����, �⸸å�� ���� ����, ����, �⵿ ���⿡ �����ȴ�.
/// 
/// ����� ���� ������ ������
/// </summary>
public class EnemyHandleFight : MonoBehaviour
{
	List<Fight> prevFight;

	Action fightDetailTactic;
	Dictionary<Fight, Action> fightTacticPair;


	private void Awake()
	{
		
	}

	private void Start()
	{
        prevFight = EnemyBrain.instance.ongoingFights;
        EnemyBrain.instance.AddFightUpdate(UpdateFightStatus);
	}

	public void UpdateFightStatus()
	{
		List<Fight> lst;

		if ((lst = prevFight.Except(EnemyBrain.instance.ongoingFights).ToList()).Count > 0) //���� �����
		{
			for (int i = 0; i < lst.Count; i++)
			{
				EnemyPosGen.instance.myControls.AddRange(lst[i].engagedAIUnits);
				//��ųʸ� ������� ���ֱ�.
			}
		}
		if ((lst = EnemyBrain.instance.ongoingFights.Except(prevFight).ToList()).Count > 0) //���� �߻���
		{
			for (int i = 0; i < lst.Count; i++)
			{
				SetTactics(lst[i]);
				EnemyPosGen.instance.myControls = EnemyPosGen.instance.myControls.Except(lst[i].engagedAIUnits).ToList();
				//���� ������� �����ֱ�. settactic���� ���ѵ� �ǰ�...
				//��ųʸ����� �߰�..
			}
		}
	}

	void SetTactics(Fight fight)
	{
		if (fight != null)
		{
			switch (fight.ResultEstimate())
			{
				case Result.Draw:
					{
						if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.defendBias)
						{
							if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.reconBias)
							{
								//����-����
								fight.useTactic = Tactics.HammerNAnvil;
							}
							else
							{
								//�⸸-����
								fight.useTactic = Tactics.Feint;
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//����-����
								fight.useTactic = Tactics.Defend;
							}
							else
							{
								//�⸸-����
								fight.useTactic = Tactics.Feint;
							}
						}
					}
					break;
				case Result.EnemyWin:
					{
						if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.defendBias)
						{
							if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.reconBias)
							{
								//����-�켼
								fight.useTactic = Tactics.AllOut;
							}
							else
							{
								//�⸸-�켼
								fight.useTactic = Tactics.Encumber;
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//����-�켼
								fight.useTactic = Tactics.XXXXXXXXXX;
							}
							else
							{
								//�⸸-�켼
								fight.useTactic = Tactics.Encumber;
							}
						}
					}
					break;
				case Result.PlayerWin:
					{
						if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.defendBias)
						{
							if (EnemyBrain.instance.set.violenceBias > EnemyBrain.instance.set.reconBias)
							{
								fight.useTactic = Tactics.Blitzkrieg;
								//����-����
							}
							else
							{
								fight.useTactic = (Tactics)UnityEngine.Random.Range(0, ((int)Tactics.Random));
								//�⸸-����
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//����-����
								fight.useTactic = Tactics.Guerilla;
							}
							else
							{
								//�⸸-����
								fight.useTactic = (Tactics)UnityEngine.Random.Range(0, ((int)Tactics.Random));
							}
						}
					}
					break;
			}
		}
	}

	private void Update()
	{
		fightDetailTactic?.Invoke();
	}
}
