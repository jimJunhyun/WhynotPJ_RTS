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
		//Debug.Log(startPos);
		//Debug.Log(endPos);

		

		base.Gen(startPos, endPos,affectHeight, id);

		wallAmt = Mathf.CeilToInt(length / ConstructBuild.WALLXSCALE);
		wallAmtVert = Mathf.CeilToInt(startPos.y / ConstructBuild.WALLYSCALE);
		children[0].transform.position = transform.position;
		children[0].transform.localScale = new Vector3(wallAmt * 100, 100, 100);
		children[1].transform.position = new Vector3(transform.position.x, transform.position.y - (wallAmtVert - 1) * 0.8f, transform.position.z);
		children[1].transform.localScale = new Vector3(wallAmt * 100, 100, wallAmtVert * 100);

		for (int i = Mathf.CeilToInt(-(wallAmt / 2)); i < Mathf.FloorToInt( wallAmt / 2); ++i)
		{
			GameObject g = Instantiate(children[2], transform).gameObject;
			g.transform.localPosition = new Vector3(i * ConstructBuild.WALLXSCALE, startPos.y,0);
		}
		children[2].transform.localPosition = new Vector3(Mathf.CeilToInt(wallAmt / 2) * ConstructBuild.WALLXSCALE, startPos.y, 0);
	}
}
