using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Result
{
    Draw,
    PlayerWin, 
    EnemyWin,
}

public enum Tactics
{
    None = -1,
    AllOut,
    HammerNAnvil,
    Blitzkrieg,
    XXXXXXXXXX,
    Defend,
    Guerilla,
    Encumber,
    Feint, 
    Random
}

/// <summary>
/// 전투 데이터를 담는 클래스
/// 
/// 적군에게만 쓰이도록 할 예정.
/// 플레이어에게 정보 제공하는데도 쓸 수 있겠지만, 수정이 필요함.
/// </summary>
public class Fight
{
    const float NEARPOINTSTANDARD = 20f;

    public List<UnitController> engagedPlayerUnits;
    public List<UnitController> engagedAIUnits;

    public float pCostEstime;
    public float eCostEstime;
    
    public Vector3 predictedPos;

    public Tactics useTactic;

    public bool IsInvalidFight
	{
        get => engagedAIUnits.Count == 0 || engagedPlayerUnits.Count == 0;
	}

    float err = 10f;

    public Result ResultEstimate()
	{
        return ConstructBuild.Approximate(pCostEstime, eCostEstime, err) ? Result.Draw : pCostEstime > eCostEstime ? Result.PlayerWin : Result.EnemyWin;
	}

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="predPos">
    /// 전투가 벌어진 곳. 최초 타격 지점임
    /// </param>
    /// <param name="pUnits">
    /// 플레이어 유닛 목록.
    /// 넣지 않으면 근처유닛
    /// </param>
    /// <param name="aiUnits">
    /// AI 유닛 목록, accumulatedUnit --> myContorl로 옮길 때 담고 넣어줄 수 있음.
    /// 안넣으면 그냥 근처 유닛
    /// </param>
    public Fight(Vector3 predPos, List<UnitController> pUnits = null, List<UnitController> aiUnits = null)
	{
        predictedPos = predPos;

        Collider[] c=  Physics.OverlapSphere(predictedPos, NEARPOINTSTANDARD, 1 << 12); //UnitLayer const 로 하나 해서 넣기.

        engagedAIUnits = aiUnits;
        engagedPlayerUnits = pUnits;

        useTactic = Tactics.None;

        if(engagedAIUnits == null)
		{
            engagedAIUnits = new List<UnitController>();
            for (int i = 0; i < c.Length; i++)
            {
                UnitController unitCont;
                if (unitCont = c[i].GetComponent<UnitController>())
                {
                    if (!unitCont.isPlayer)
                        engagedAIUnits.Add(unitCont);

                }
            }
        }

        if (engagedPlayerUnits == null)
        {
            engagedPlayerUnits = new List<UnitController>();
            for (int i = 0; i < c.Length; i++)
            {
                UnitController unitCont;
                if (unitCont = c[i].GetComponent<UnitController>())
                {
                    if (unitCont.isPlayer && unitCont.isSeen())
                        engagedPlayerUnits.Add(unitCont);

                }
            }
        }

        for (int i = 0; i < engagedAIUnits.Count; ++i)
		{
            eCostEstime += engagedAIUnits[i].produceTime;
		}
        for (int i = 0; i < engagedPlayerUnits.Count; ++i)
        {
            pCostEstime += engagedPlayerUnits[i].produceTime;
        }
    }
}
