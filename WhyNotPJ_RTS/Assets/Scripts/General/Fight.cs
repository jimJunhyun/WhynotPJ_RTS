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
    /// ���� ������ ��ó����
    /// </param>
    /// <param name="aiUnits">
    /// AI ���� ���, accumulatedUnit --> myContorl�� �ű� �� ��� �־��� �� ����.
    /// �ȳ����� �׳� ��ó ����
    /// </param>
    /// <param name="possiblity">
    /// ���� ������ �Ͼ ���ɼ��� �ִ���
    /// true�� �浹�� ����Ǵ°Ű�
    /// �ƴϸ� �� ���� ��� ���ž��� ���
    /// </param>
    public Fight(Vector3 predPos, out bool possiblity, List<UnitController> pUnits = null, List<UnitController> aiUnits = null)
	{
        predictedPos = predPos;
        possiblity = true;

        Collider[] c=  Physics.OverlapSphere(predictedPos, NEARPOINTSTANDARD, 1 << 12); //UnitLayer const �� �ϳ� �ؼ� �ֱ�.

        engagedAIUnits = aiUnits;
        engagedPlayerUnits = pUnits;

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

       if(engagedAIUnits == null || engagedPlayerUnits == null || engagedAIUnits.Count == 0 || engagedPlayerUnits.Count == 0)
            possiblity = false;

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
