using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BridgeRender : GroundBreak
{

	public override void Awake()
	{
		base.Awake();
	}

	public override void Update()
	{
		base.Update();
		
	}

	public override void Gen(Vector3 startPos, Vector3 endPos, bool affectHeight, int id)
	{
		base.Gen(startPos, endPos, affectHeight, id);
		mat.SetTextureScale("_MainTex", new Vector2(1, length / 10));
		
		children[0].transform.localScale = new Vector3(100, 35 * length / 3, 100);
		children[1].transform.localPosition = Vector3.forward * (length / 2);
		children[2].transform.localPosition = -Vector3.forward * (length / 2);

		//Debug.Log("!");

		
		
	}
}
