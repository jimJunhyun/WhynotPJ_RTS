using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Result
{
    Draw,
    PlayerWin, 
    EnemyWin,
}

/// <summary>
/// 전투 데이터를 담는 클래스
/// 
/// 발생할 전투를 예상하는 데에 사용된다.
/// </summary>
public class Fight
{
    public List<UnitController> engagedPlayerUnits;
    public List<UnitController> engagedAIUnits;

    public float pCostEstime;
    public float eCostEstime;

    float err = 5f;

    public Result ResultEstimate()
	{
        return ConstructBuild.Approximate(pCostEstime, eCostEstime, err) ? Result.Draw : pCostEstime > eCostEstime ? Result.PlayerWin : Result.EnemyWin;
	}

    public Fight(List<UnitController> ais, List<UnitController> players)
	{
        engagedAIUnits = new List<UnitController>(ais);
        engagedPlayerUnits = new List<UnitController>(players);

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
