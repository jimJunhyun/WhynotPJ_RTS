using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buildables
{
	Bridge,
	Wall,


}

public class ConstructBuild : MonoBehaviour
{ 

	const float BRIDGEXSCALE = 4.5f;
	const float BRIDGEYSCALE = 0.5f;
	const int GROUNDLAYERMASK = 1 << 8;

	const float RAYGAP = 1.2f; //�� ����


	public float bridgeYErr = 0.5f;
	public BridgeRender bridge;
	//public GameObject wall;

	//test
	public Transform sPos;
	public Transform ePos;
	//test

	private void Start()
	{
		Construct(sPos.position, ePos.position, Buildables.Bridge);
	}

	public void Construct(Vector3 startPos, Vector3 endPos, Buildables type)
	{
		// Ŭ���ý��۰� ���� �� ����
		Vector2Int sIdx = Perceive.PosToIdxVector(startPos);
		Vector2Int eIdx = Perceive.PosToIdxVector(endPos);
		startPos.y = PlayerEye.instance.perceived.map[sIdx.y, sIdx.x].height;
		endPos.y = PlayerEye.instance.perceived.map[eIdx.y, eIdx.x].height;
		// Ŭ���ý��۰� ���� �� ����
		Vector3 dir = endPos - startPos;
		Vector3 pos = (startPos + endPos) / 2;
		float dist = dir.magnitude;
		if(type == Buildables.Bridge)
		{
			if(BridgeExamine(startPos, endPos, dist))
			{
				BridgeRender b = Instantiate(bridge);
				b.transform.position = pos;
				b.transform.LookAt(endPos);
				b.Gen(dist);
			}
			else
			{
				Debug.Log("���� �� ����.");
			}
		}
		
	}
	//��� �ȵɵ�?
	//public void ConstructWithIdx(Vector2Int startPos, Vector2Int endPos, Buildables type)
	//{
	//	Vector3 sPos = Perceive.IdxVectorToPos(startPos);
	//	Vector3 ePos = Perceive.IdxVectorToPos(endPos);
	//	sPos.y = PlayerEye.instance.perceived.map[startPos.y, startPos.x].height;
	//	ePos.y = PlayerEye.instance.perceived.map[endPos.y, endPos.x].height;
	//	Vector3 dir = ePos - sPos;
	//	Vector3 pos = (sPos + ePos) / 2;
	//	float dist = dir.magnitude;
	//	if (type == Buildables.Bridge)
	//	{
	//		BridgeRender b = Instantiate(bridge);
	//		b.transform.position = pos;
	//		b.transform.LookAt(ePos);
	//		b.Gen(dist);
	//	}
	//}

	bool BridgeExamine(Vector3 startPos, Vector3 endPos, float length)
	{
		Vector3 dir = endPos - startPos;
		startPos.y -= BRIDGEYSCALE / 2 - bridgeYErr;
		if(Physics.BoxCast(startPos, new Vector3(BRIDGEXSCALE / 2, BRIDGEYSCALE / 2, 0.5f), dir.normalized, Quaternion.LookRotation(dir.normalized), length, GROUNDLAYERMASK))
		{
			return false;
		}
		for (int i = 0; i < length; i++)
		{

		}
		return true;
	}
}
