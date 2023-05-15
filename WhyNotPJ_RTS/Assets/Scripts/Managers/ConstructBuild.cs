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
	public const float WALLXSCALE = 7.5f;
	public const float WALLYSCALE = 5.5f;

	const float RAYDIST = 1.5f; //모델 키
	const float RAYGAP = 1.2f; //모델 지름

	static int StrtNumber = 12;

	public float bridgeYErr = 0.5f;
	public GroundBreak bridge;
	public GroundBreak wall;
	public Dictionary<int, GroundBreak> strtIdPair = new Dictionary<int, GroundBreak>();
	

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
		if(Perceive.fullMap[sIdx.y, sIdx.x, 0].info == GroundState.Water || Perceive.fullMap[eIdx.y, eIdx.x, 0].info == GroundState.Water)
		{
			Debug.Log("물");
			return;
		}
		startPos.y = Perceive.fullMap[sIdx.y, sIdx.x,0].height;
		endPos.y = Perceive.fullMap[eIdx.y, eIdx.x, 0].height;

		

		// 클릭시스템과 병합 시 제거 #
		
		Vector3 pos = new Vector3();
		GroundBreak b = null;
		switch (type)
		{
			case Buildables.Bridge:
				if(!BridgeExamine(startPos, endPos, (endPos - startPos).magnitude))
					return;
				b = Instantiate(bridge);
				
				pos = (startPos + endPos) / 2;
				b.transform.position = pos;
				b.transform.LookAt(endPos);
				break;
			case Buildables.Wall:
				float lowest;
				if(!WallExamine(startPos, endPos, (endPos - startPos).magnitude, out lowest))
					return;
				b = Instantiate(wall);

				float highest = startPos.y > endPos.y ? startPos.y : endPos.y;
				highest += WALLYSCALE;
				startPos.y = highest;
				endPos.y = highest;
				pos = (startPos + endPos) / 2;
				b.transform.position = pos;
				b.transform.LookAt(endPos);
				b.transform.Rotate(0, 90, 0);
				pos.y = lowest;
				b.transform.position = pos;
				break;
			default:
				break;
		}
		
		strtIdPair.Add(StrtNumber, b);

		Instantiate(ePos, startPos, Quaternion.identity);
		Instantiate(ePos, endPos, Quaternion.identity);
		

		b.Gen(startPos, endPos, false, StrtNumber++);
		
	}


	bool BridgeExamine(Vector3 startPos, Vector3 endPos, float length)
	{
		Vector3 dir = endPos - startPos;
		Vector3 boxOrigin = startPos;
		boxOrigin.y += BRIDGEYSCALE / 2 + bridgeYErr;
		//Debug.DrawRay(boxOrigin, dir, Color.red, 1000f);
		if (Physics.BoxCast(boxOrigin, new Vector3(BRIDGEXSCALE / 2, BRIDGEYSCALE / 2, 0.5f), dir.normalized, Quaternion.LookRotation(dir.normalized), length, Perceive.GROUNDMASK | Perceive.BRIDGEMASK))
		{
			Debug.Log("걸리는 것 발견됨.");
			return false;
		}
		startPos.y += BRIDGEYSCALE / 2;
		while (!Approximate(startPos, endPos, RAYGAP / 2)) 
			//아주 가아아끔 아무도 밑으로 못지나가는 다리가 생길수도있다. U자형일때
			//아주 가아끔 다른 다리와 겹칠 수 있다. 거리가 절묘할때
			//큰 문제가 될 경우 탐색 심도를 높이기로.
		{
			startPos += dir.normalized * RAYGAP;
			Vector3Int idx = Perceive.PosToIdxVector(startPos);
			if(Perceive.fullMap[idx.x, idx.y, 1].Id != 0)
			{
				Debug.Log("다른 물체 발견됨.");
				return false;
			}
			if (!Physics.Raycast(startPos, Vector3.down, RAYDIST, Perceive.GROUNDMASK))
			{
				//Debug.DrawRay(startPos, Vector3.down * RAYDIST, Color.blue,1000f);
				return true; 
			}
			//else
			//{
			//	Debug.DrawRay(startPos, Vector3.down * RAYDIST, Color.red, 1000f);
			//}
		}
		Debug.Log("키보다 작음.");
		return false;
	}
	
	bool WallExamine(Vector3 startPos, Vector3 endPos, float length, out float lowestPoint)
	{
		Vector3 p = startPos;
		Vector3 dir = endPos - startPos;
		RaycastHit h;
		lowestPoint = 0;
		while(!Approximate(p, endPos, RAYGAP / 2))
		{
			Vector3Int v = Perceive.PosToIdxVector(p);
			if (Perceive.fullMap[v.y, v.x, 1].Id != 0)
			{
				Debug.Log("다른 물체 발견됨");
				return false;
			}
			if(Physics.Raycast(p, Vector3.down, out h , 100f, Perceive.GROUNDMASK))
			{
				if(lowestPoint > h.point.y)
				{
					lowestPoint = h.point.y;
				}
				
			}
			else
			{
				Debug.Log("더 높은 무언가 발견됨.");
				return false;
			}
			p += dir.normalized * RAYGAP;
		}


		return true;
	}

	bool Approximate(float a, float b, float err)
	{
		return Mathf.Abs(a - b) < err;
	}

	bool Approximate(Vector3 a, Vector3 b, float err)
	{
		return Approximate(a.x, b.x, err) && Approximate(a.y, b.y , err) && Approximate(a.z, b.z, err);
	}
}
