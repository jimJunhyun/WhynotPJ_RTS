using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;




/// <summary>
/// 적을 섬멸하는 전술 (중요도 : 아군 유닛의 손실 억제 <= 전략에 걸리는 시간 감소)
///		우세 : 전면전 + 적극적 추격
///		동등 : 망치와 모루 
///		열세 : 전격전 (본진만 향해 돌격 = 적 무시)
///	적을 붙잡는 전술 (중요도 : 아군 유닛의 손실 억제 >= 전략에 걸리는 시간 감소)
///		우세 : ???(유닛의 소멸 시간까지 수행이 길어져 벌려둔 차이가 사라질 수 있음.)
///		동등 : 방어
///		열세 : 게릴라 (의도적인 장기전 --> 소멸 유도)
///	적을 속이는 전술 (전투 자체를 파악하기 어렵게 함.)
///		우세 : 험지로 유인 (지형상 역습의 가능성이 높음.)
///		동등 : 양동 공격
///		열세 : 무작위 전술 (다른 전술과 유사하여 플레이어를 속임.)
/// 
/// 섬멸, 저지, 기만책은 각각 공격, 수비, 기동 성향에 대응된다.
/// 
/// 만들기 쉬운 순서로 만들어보자
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

		if ((lst = prevFight.Except(EnemyBrain.instance.ongoingFights).ToList()).Count > 0) //전투 종료시
		{
			for (int i = 0; i < lst.Count; i++)
			{
				EnemyPosGen.instance.myControls.AddRange(lst[i].engagedAIUnits);
				//딕셔너리 기반으로 빼주기.
			}
		}
		if ((lst = EnemyBrain.instance.ongoingFights.Except(prevFight).ToList()).Count > 0) //전투 발생시
		{
			for (int i = 0; i < lst.Count; i++)
			{
				SetTactics(lst[i]);
				EnemyPosGen.instance.myControls = EnemyPosGen.instance.myControls.Except(lst[i].engagedAIUnits).ToList();
				//전략 기반으로 더해주기. settactic에서 시켜도 되고...
				//딕셔너리에도 추가..
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
								//섬멸-동등
								fight.useTactic = Tactics.HammerNAnvil;
							}
							else
							{
								//기만-동등
								fight.useTactic = Tactics.Feint;
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//저지-동등
								fight.useTactic = Tactics.Defend;
							}
							else
							{
								//기만-동등
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
								//섬멸-우세
								fight.useTactic = Tactics.AllOut;
							}
							else
							{
								//기만-우세
								fight.useTactic = Tactics.Encumber;
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//저지-우세
								fight.useTactic = Tactics.XXXXXXXXXX;
							}
							else
							{
								//기만-우세
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
								//섬멸-열세
							}
							else
							{
								fight.useTactic = (Tactics)UnityEngine.Random.Range(0, ((int)Tactics.Random));
								//기만-열세
							}
						}
						else
						{
							if (EnemyBrain.instance.set.defendBias > EnemyBrain.instance.set.reconBias)
							{
								//저지-열세
								fight.useTactic = Tactics.Guerilla;
							}
							else
							{
								//기만-열세
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
