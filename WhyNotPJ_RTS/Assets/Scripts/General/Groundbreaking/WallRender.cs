using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRender : GroundBreak
{ 
	//B --> M -->  T
	int wallAmt;
	int wallAmtVert;

	public override void Awake()
	{
		base.Awake();
	}

	public override void Gen(Vector3 startPos, Vector3 endPos, bool affectHeight, int id)
	{
		float highest = startPos.y > endPos.y ? startPos.y : endPos.y;
		highest += ConstructBuild.WALLYSCALE;

		startPos.y = highest;
		endPos.y = highest;
		
		Vector3 dir = endPos - startPos;

		base.Gen(startPos, endPos,affectHeight, id);

		wallAmt = (int)(length / ConstructBuild.WALLXSCALE);
		wallAmtVert = (int)(highest / ConstructBuild.WALLYSCALE);
		children[0].transform.position = pos;
		children[0].transform.localScale = new Vector3(wallAmt * 100, 100, 100);
		children[1].transform.position = new Vector3(pos.x, pos.y - (wallAmtVert - 1) * 0.8f, pos.z);
		children[1].transform.localScale = new Vector3(wallAmt * 100, 100, wallAmtVert * 100);

		for (int i = Mathf.CeilToInt(-(wallAmt / 2)); i < Mathf.CeilToInt( wallAmt / 2); ++i)
		{
			GameObject g = Instantiate(children[2], transform).gameObject;
			g.transform.localPosition = new Vector3(i * ConstructBuild.WALLXSCALE, wallAmtVert * ConstructBuild.WALLYSCALE,0);
		}
		children[2].transform.localPosition = new Vector3(Mathf.CeilToInt(wallAmt / 2) * ConstructBuild.WALLXSCALE, wallAmtVert * ConstructBuild.WALLYSCALE, 0);
	}
}
