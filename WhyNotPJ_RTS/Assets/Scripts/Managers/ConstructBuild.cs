//#define MOBILE

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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
	public const float WALLYSCALE = 2.4f;//벽 가운데
	public const float WALLBASEYSCALE = 1.66f;//벽 밑
	public const float WALLTOPYGAP = 4.55f;//벽 위 빈 부분 넓이

	const float RAYDIST = 1.5f; //모델 키
	const float RAYGAP = 1.2f; //모델 지름

	static int StrtNumber = 12;

	public float bridgeYErr = 0.5f;
	public GroundBreak bridge;
	public GroundBreak wall;
	public Dictionary<int, GroundBreak> strtIdPair = new Dictionary<int, GroundBreak>();
	

	Vector3 sPos;
	Vector3 ePos;
	bool valid = false;

	public UnityEvent OnStartConstruction = null;

	private void Awake()
	{
		instance = this;
	}

	public IEnumerator BuildInp(Buildables t)
	{
		OnStartConstruction?.Invoke();

		valid = false;
		yield return StartCoroutine(DelayGetInput((vec, b) => {
			valid = b;
			sPos = vec;
		}));
		yield return null;
		yield return StartCoroutine(DelayGetInput((vec, b) => {
			valid = b;
			ePos = vec;
		}));

		Construct(t);
	}

	void Construct(Buildables type)
	{
		


		if (!valid)
		{
			return;
		}

		Vector3Int sIdx = Perceive.PosToIdxVector(sPos);
		Vector3Int eIdx = Perceive.PosToIdxVector(ePos);
		if(Perceive.fullMap[sIdx.y, sIdx.x, 0].info == GroundState.Water || Perceive.fullMap[eIdx.y, eIdx.x, 0].info == GroundState.Water)
		{
			Debug.Log("물");
			return;
		}
		
		Vector3 pos = new Vector3();
		GroundBreak b = null;
		switch (type)
		{
			case Buildables.Bridge:
				if(!BridgeExamine(sPos, ePos, (ePos - sPos).magnitude))
					return;
				b = Instantiate(bridge);
				
				pos = (sPos + ePos) / 2;
				b.transform.position = pos;
				b.transform.LookAt(ePos);
				break;
			case Buildables.Wall:
				float highest = sPos.y > ePos.y ? sPos.y : ePos.y;
				highest += WALLBASEYSCALE + WALLYSCALE;
				sPos.y = highest;
				ePos.y = highest;
				float lowest;
				if(!WallExamine(sPos, ePos, (ePos - sPos).magnitude, out lowest, out highest))
					return;
				b = Instantiate(wall);
				highest += WALLBASEYSCALE + WALLYSCALE;
				((WallRender)b).lowestPoint = lowest;
				((WallRender)b).highestPoint = highest;
				if (sPos.y < highest)
				{
					sPos.y = highest;
					ePos.y = highest;
				}
				pos = (sPos + ePos) / 2;
				b.transform.position = pos;
				b.transform.LookAt(ePos);
				b.transform.Rotate(0, 90, 0);
				pos.y = lowest;
				b.transform.position = pos;
				break;
			default:
				break;
		}

		//Instantiate(ePos, startPos, Quaternion.identity);
		//Instantiate(ePos, endPos, Quaternion.identity);
		
		strtIdPair.Add(StrtNumber, b);

		b.Gen(sPos, ePos, true, StrtNumber++);
		
	}


	bool BridgeExamine(Vector3 startPos, Vector3 endPos, float length)
	{
		Vector3 dir = endPos - startPos;
		Vector3 boxOrigin = startPos;
		boxOrigin.y += BRIDGEYSCALE / 2 + bridgeYErr;
		//Debug.DrawRay(boxOrigin, dir, Color.red, 1000f);
		if (Physics.BoxCast(boxOrigin, new Vector3(BRIDGEXSCALE / 2, BRIDGEYSCALE / 2, 0.5f), dir.normalized, Quaternion.LookRotation(dir.normalized), length, Perceive.GROUNDMASK | Perceive.CONSTRUCTMASK))
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
				return true; 
			}
		}
		Debug.Log("키보다 작음.");
		return false;
	}
	
	bool WallExamine(Vector3 startPos, Vector3 endPos, float length, out float lowestPoint, out float highestPoint)
	{
		Vector3 p = startPos;
		Vector3 dir = endPos - startPos;
		RaycastHit h;
		lowestPoint = int.MaxValue;
		highestPoint = int.MinValue;
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
				Debug.DrawLine(new Vector3(p.x, p.y ,p.z ), h.point, Color.red, 1000f);
				if(lowestPoint > h.point.y)
				{
					lowestPoint = h.point.y;
				}
				if(highestPoint < h.point.y)
				{
					highestPoint = h.point.y;
				}
			}
			else
			{
				Debug.DrawRay(p,Vector3.down * 100, Color.white, 1000f);
				Debug.Log("더 높은 무언가 발견됨.");
				return false;
			}
			p += dir.normalized * RAYGAP;
		}


		return true;
	}

	public static bool Approximate(float a, float b, float err)
	{
		return Mathf.Abs(a - b) < err;
	}

	public static bool Approximate(Vector3 a, Vector3 b, float err)
	{
		return Approximate(a.x, b.x, err) && Approximate(a.y, b.y , err) && Approximate(a.z, b.z, err);
	}

	IEnumerator DelayGetInput(System.Action<Vector3, bool> callback)
	{

		Vector3 ret = new Vector3();
		bool isValid = true;
		yield return new WaitUntil(()=>{ 
#if MOBILE
			return Input.touchCount == 1;
#else
			//Debug.Log("!");
			return Input.GetMouseButtonDown(0);
#endif
		});

#if MOBILE
		Touch t = Input.GetTouch(0);
		yield return new WaitUntil(() => {

			return t.phase == TouchPhase.Ended;

			});
#endif
		RaycastHit hit;
#if MOBLIE
		Ray r = Camera.main.ScreenPointToRay(t.position);
#else
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
		if (isValid = Physics.Raycast(r,out hit, Camera.main.farClipPlane, Perceive.GROUNDMASK))
		{
			ret = hit.point;
		}
		callback(ret, isValid);
	}
}
