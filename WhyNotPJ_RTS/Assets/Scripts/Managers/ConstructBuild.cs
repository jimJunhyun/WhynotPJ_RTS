//#define MOBILE

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

	const float RAYDIST = 1.5f; //�� Ű
	const float RAYGAP = 1.2f; //�� ����

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
		Construct(Buildables.Wall);
	}

	public bool TouchDetect(out Vector3 sPos, out Vector3 ePos)
	{
		Vector3 v = new Vector3();
		bool valid = true;
		StartCoroutine(DelayGetInput((vec, b) => {
			valid = b;
			v = vec;
			
			
		}));
		Debug.Log("��ġ1");
		if (valid)
			sPos = v;
		else
		{
			sPos = new Vector3();
			ePos = new Vector3();
			return false;
		}
			
		StartCoroutine(DelayGetInput((vec, b) => {
			valid = b;
			v = vec;

		}));
		Debug.Log("��ġ2");
		if (valid)
			ePos = v;
		else
		{
			sPos = new Vector3();
			ePos = new Vector3();
			return false;
		}

		return true;
	}

	public void Construct(/*Vector3 startPos, Vector3 endPos, */Buildables type)
	{
		Vector3 startPos, endPos;
		if(!TouchDetect(out startPos, out endPos))
		{
			return;
		}
		

		Vector3Int sIdx = Perceive.PosToIdxVector(startPos);
		Vector3Int eIdx = Perceive.PosToIdxVector(endPos);
		if(Perceive.fullMap[sIdx.y, sIdx.x, 0].info == GroundState.Water || Perceive.fullMap[eIdx.y, eIdx.x, 0].info == GroundState.Water)
		{
			Debug.Log("��");
			return;
		}
		
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
				float highest = startPos.y > endPos.y ? startPos.y : endPos.y;
				highest += WALLYSCALE;
				startPos.y = highest;
				endPos.y = highest;
				float lowest;
				if(!WallExamine(startPos, endPos, (endPos - startPos).magnitude, out lowest))
					return;
				b = Instantiate(wall);

				
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

		//Instantiate(ePos, startPos, Quaternion.identity);
		//Instantiate(ePos, endPos, Quaternion.identity);
		
		strtIdPair.Add(StrtNumber, b);

		b.Gen(startPos, endPos, false, StrtNumber++);
		
	}


	bool BridgeExamine(Vector3 startPos, Vector3 endPos, float length)
	{
		Vector3 dir = endPos - startPos;
		Vector3 boxOrigin = startPos;
		boxOrigin.y += BRIDGEYSCALE / 2 + bridgeYErr;
		//Debug.DrawRay(boxOrigin, dir, Color.red, 1000f);
		if (Physics.BoxCast(boxOrigin, new Vector3(BRIDGEXSCALE / 2, BRIDGEYSCALE / 2, 0.5f), dir.normalized, Quaternion.LookRotation(dir.normalized), length, Perceive.GROUNDMASK | Perceive.CONSTRUCTMASK))
		{
			Debug.Log("�ɸ��� �� �߰ߵ�.");
			return false;
		}
		startPos.y += BRIDGEYSCALE / 2;
		while (!Approximate(startPos, endPos, RAYGAP / 2)) 
			//���� ���ƾƲ� �ƹ��� ������ ���������� �ٸ��� ��������ִ�. U�����϶�
			//���� ���Ʋ� �ٸ� �ٸ��� ��ĥ �� �ִ�. �Ÿ��� �����Ҷ�
			//ū ������ �� ��� Ž�� �ɵ��� ���̱��.
		{
			startPos += dir.normalized * RAYGAP;
			Vector3Int idx = Perceive.PosToIdxVector(startPos);
			if(Perceive.fullMap[idx.x, idx.y, 1].Id != 0)
			{
				Debug.Log("�ٸ� ��ü �߰ߵ�.");
				return false;
			}
			if (!Physics.Raycast(startPos, Vector3.down, RAYDIST, Perceive.GROUNDMASK))
			{
				return true; 
			}
		}
		Debug.Log("Ű���� ����.");
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
				Debug.Log("�ٸ� ��ü �߰ߵ�");
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
				Debug.DrawRay(p,Vector3.down * 100, Color.white, 1000f);
				Debug.Log("�� ���� ���� �߰ߵ�.");
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
		Debug.Log("�ٱ�ٸ�.");

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
