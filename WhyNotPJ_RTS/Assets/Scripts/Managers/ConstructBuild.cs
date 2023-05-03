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
	public static ConstructBuild instance;

	const float BRIDGEXSCALE = 4.5f;
	const float BRIDGEYSCALE = 0.5f;
	const int GROUNDLAYERMASK = 1 << 8;

	const float RAYDIST = 1.5f; //모델 키
	const float RAYGAP = 1.2f; //모델 지름

	static int BridgeNumber = 12;

	public float bridgeYErr = 0.5f;
	public BridgeRender bridge;
	public Dictionary<int, BridgeRender> bridgeIdPair = new Dictionary<int, BridgeRender>();
	//public GameObject wall;

	//test
	public Transform sPos;
	public Transform ePos;
	//test

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Construct(sPos.position, ePos.position, Buildables.Bridge);
	}

	public void Construct(Vector3 startPos, Vector3 endPos, Buildables type)
	{
		// 클릭시스템과 병합 시 제거 #
		Vector3Int sIdx = Perceive.PosToIdxVector(startPos);
		Vector3Int eIdx = Perceive.PosToIdxVector(endPos);
		startPos.y = Perceive.fullMap[sIdx.y, sIdx.x,0].height;
		endPos.y = Perceive.fullMap[eIdx.y, eIdx.x, 0].height;
		// 클릭시스템과 병합 시 제거 #
		
		Vector3 dir = endPos - startPos;
		Vector3 pos = (startPos + endPos) / 2;
		float dist = dir.magnitude;
		if(type == Buildables.Bridge)
		{
			if(BridgeExamine(startPos, endPos, dist))
			{
				BridgeRender b = Instantiate(bridge);
				bridgeIdPair.Add(BridgeNumber, b);
				b.transform.position = pos;
				b.transform.LookAt(endPos);
				Vector3Int p = new Vector3Int((int)startPos.x, (int)startPos.y, 0);
				float rad = Mathf.Atan2(dir.x, dir.z);
				b.Gen(dist, p, rad, BridgeNumber++);
				
			}
			else
			{
				Debug.Log("지을 수 없다.");
			}
		}
		
	}
	//사용 안될듯?
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
		Vector3 boxOrigin = startPos;
		boxOrigin.y += BRIDGEYSCALE / 2 + bridgeYErr;
		//Debug.DrawRay(boxOrigin, dir, Color.red, 1000f);
		if (Physics.BoxCast(boxOrigin, new Vector3(BRIDGEXSCALE / 2, BRIDGEYSCALE / 2, 0.5f), dir.normalized, Quaternion.LookRotation(dir.normalized), length, GROUNDLAYERMASK))
		{
			Debug.Log("걸리는 것 발견됨.");
			return false;
		}
		startPos.y += BRIDGEYSCALE / 2;
		while(!Approximate(startPos, endPos, 0.5f))
		{
			startPos += dir.normalized * RAYGAP;
			Vector3Int idx = Perceive.PosToIdxVector(startPos);
			if (!Physics.Raycast(startPos, Vector3.down, RAYDIST, GROUNDLAYERMASK) && Perceive.fullMap[idx.x, idx.y, 0].Id == 0)
			{
				Debug.DrawRay(startPos, Vector3.down * RAYDIST, Color.blue,1000f);
				return true; 
			}
			else
			{
				Debug.DrawRay(startPos, Vector3.down * RAYDIST, Color.red, 1000f);
			}
		}
		Debug.Log("키보다 작음.");
		return false;
	}

	//여기에 높이를 판단해서 맵에 넣어줘야함.
	//우선 맵의 데이터 저장시스템을 변경할 필요가 있어보임.

	bool Approximate(float a, float b, float err)
	{
		return Mathf.Abs(a - b) < err;
	}

	bool Approximate(Vector3 a, Vector3 b, float err)
	{
		return Approximate(a.x, b.x, err) && Approximate(a.y, b.y , err) && Approximate(a.z, b.z, err);
	}
}
