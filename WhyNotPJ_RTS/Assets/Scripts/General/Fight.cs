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
/// ���� �����͸� ��� Ŭ����
/// 
/// �߻��� ������ �����ϴ� ���� ���ȴ�.
/// �������Ը� ���̵��� �� ����.
/// �÷��̾�� ���� �����ϴµ��� �� �� �ְ�����, ������ �ʿ���.
/// </summary>
public class Fight
{
    const float NEARPOINTSTANDARD = 20f;

    public List<UnitController> engagedPlayerUnits;
    public List<UnitController> engagedAIUnits;

    public float pCostEstime;
    public float eCostEstime;

    public Vector3 predictedPos;

    float err = 5f;

    public Result ResultEstimate()
	{
        return ConstructBuild.Approximate(pCostEstime, eCostEstime, err) ? Result.Draw : pCostEstime > eCostEstime ? Result.PlayerWin : Result.EnemyWin;
	}

    /// <summary>
    /// ������.
    /// </summary>
    /// <param name="predPos">
    /// ������ ������ �� ���� ��.
    /// �� �� ���� ������ ���� �������� ����
    /// </param>
    /// <param name="pUnits">
    /// �÷��̾� ���� ���.
    /// ���� ������ AI�� ����� ������ ������
    /// </param>
    /// <param name="aiUnits">
    /// AI ���� ���, accumulatedUnit --> myContorl�� �ű� �� ��� �־��� �� ����.
    /// ���� ������ �÷��̾��� ����� ������ ������ (��������.)
    /// </param>
    public Fight(Vector3 predPos, List<UnitController> pUnits = null, List<UnitController> aiUnits = null)
	{
        predictedPos = predPos;

        Collider[] c=  Physics.OverlapSphere(predictedPos, NEARPOINTSTANDARD, 1 << 12); //UnitLayer const �� �ϳ� �ؼ� �ֱ�.

        bool isPSide = aiUnits == null;

        if(isPSide)
            engagedPlayerUnits = pUnits;
        else
            engagedAIUnits = aiUnits;

		for (int i = 0; i < c.Length; i++)
		{
            UnitController unitCont;
            if(unitCont = c[i].GetComponent<UnitController>())
			{
				if (!isPSide)
				{
                    engagedPlayerUnits.Add(unitCont);
				}
				else
				{
                    engagedAIUnits.Add(unitCont);
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
