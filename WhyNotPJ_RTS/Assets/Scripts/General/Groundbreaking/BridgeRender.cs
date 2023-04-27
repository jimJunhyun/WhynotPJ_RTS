using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRender : MonoBehaviour
{
    List<Renderer> bridgeUnder = new List<Renderer>(); //다리 -> 시작점 -> 끝점
	//BoxCollider valueChanger;
	public Material mat;
	public Vector3Int pos;

	public float length = 0;
	public const float UNITHEIGHT = 1.5f;

	private void Awake()
	{
		GetComponentsInChildren<Renderer>(bridgeUnder);
		//List<BoxCollider> bs = new List<BoxCollider>();
		//bridgeUnder[0].GetComponentsInChildren<BoxCollider>(bs);
		//bs.Remove(bridgeUnder[0].GetComponent<BoxCollider>());
		//valueChanger = bs[0];


		//valueChanger.transform.position -= new Vector3(0, UNITHEIGHT, 0);

		mat = new Material(bridgeUnder[0].material);

		for (int i = 0; i < bridgeUnder.Count; i++)
		{
			bridgeUnder[i].material = mat;
		}
	}

	public void Gen(float leng, Vector3Int p)
	{
		pos = p;
		length = leng;

		mat.SetTextureScale("_MainTex", new Vector2(1, leng / 10));
		
		bridgeUnder[0].transform.localScale = new Vector3(100, 35 * leng / 3, 100);
		bridgeUnder[1].transform.localPosition = Vector3.forward * (leng / 2);
		bridgeUnder[2].transform.localPosition = -Vector3.forward * (leng / 2);

		FogOfWar.instance.UpdateBridgeTexture(this,new Vector2Int(100, 100), 78);
	}
}
