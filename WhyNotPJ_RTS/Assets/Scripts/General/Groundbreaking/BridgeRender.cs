using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRender : MonoBehaviour
{
    List<Renderer> bridgeUnder = new List<Renderer>(); //다리 -> 시작점 -> 끝점
	//BoxCollider valueChanger;
	public Material mat;
	public Vector3Int pos;
	public float angleRad;

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

	public void Gen(float leng, Vector3Int p, float rad, int id)
	{
		pos = p;
		length = leng;
		angleRad = rad;

		mat.SetTextureScale("_MainTex", new Vector2(1, leng / 10));
		
		bridgeUnder[0].transform.localScale = new Vector3(100, 35 * leng / 3, 100);
		bridgeUnder[1].transform.localPosition = Vector3.forward * (leng / 2);
		bridgeUnder[2].transform.localPosition = -Vector3.forward * (leng / 2);

		Vector3Int idx = Perceive.PosToIdxVector(pos);

		for (int y = -(int)(length / 2); y < (int)(length / 2); y++)
		{
			for (int x = -3; x < 3; x++)
			{
				Vector3Int v = new Vector3Int(x, y, 0);
				v += idx;
				v = new Vector3Int((int)((v.x - idx.x) * Mathf.Cos(angleRad) - (v.y - idx.y) * Mathf.Cos(angleRad) + idx.x),
				   (int)((v.x - idx.x) * Mathf.Sin(angleRad) + (v.y - idx.y) * Mathf.Cos(angleRad) + idx.y),
				   0);

				Vector3 rayPos = Perceive.IdxVectorToPos(v);
				
				rayPos.y = 100f;
				
				RaycastHit hit;
				Physics.Raycast(rayPos, Vector3.down, out hit, 200f, Perceive.GROUNDMASK);
				Perceive.fullMap[v.y, v.x, 1].height = (int)hit.point.y;
				Perceive.fullMap[v.y, v.x, 1].Id = id;
			}
		}
		
	}
}
