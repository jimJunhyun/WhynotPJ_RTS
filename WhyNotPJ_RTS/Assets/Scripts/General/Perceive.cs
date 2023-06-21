using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GroundState
{
	None = -1,
	Ground,
	Water
}

public struct MapData
{
	public int x;
	public int y;
	public int height;
	public static float GetDist(Vector3 from, Vector3 to) //일단은 일직선
	{
		from.y = 0;
		to.y = 0;
		Vector3 dist = to - from;
		return dist.magnitude;
	}
	public static float GetDistSqr(Vector3 from, Vector3 to) //일단은 일직선
	{
		from.y = 0;
		to.y = 0;
		Vector3 dist = to - from;
		return dist.sqrMagnitude;
	}

	//이거 지울거임.
	//public int visiblity; 
	// bool 에서 int 로 바꿈으로서 가만히 있는 놈의 시야 등을 관리하기 편해짐.
	//프레임도 괜찮음. 상식적인 속도로 움직인다는 가정 하에 현상황 기준 90 이상의 프레임을 내더라.
	//비상식적인 속도로 움직이면 애매해지는데, 일단 그렇게 움직이지는 않는다.

	int id;
	public int Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public GroundState info;
}

// 된다면 되는거다...

public struct MapUpdateBhv
{
	Perceive.UpdMaps updateDel;
	Vector3Int startPos;
	int distance;

	public MapUpdateBhv(Perceive.UpdMaps upd, Vector3Int pos, int dist)
	{
		updateDel = upd;
		startPos = pos;
		distance = dist;
	}

	public void Invoke()
	{
		updateDel(startPos, distance);
	}
}

//플레이어는 실시간 정보 획득 불가능.
//AI는 실시간 정보 획득 가능.


/// <summary>
/// 키고 끔의 순서가 없으면, 유닛 여럿이 뭉칠때 두 명 이상의 시야가 겹치는 곳에서는 깜빡거림이 발생함.
/// 끄기를 먼저 한 뒤 키기를 진행함으로서, 깜박거림을 없앰.
/// 그러기 위해서 맵 새로고침을 구조체로 만들고, 이것을 사용하여 킴과 끔의 리스트를 만듬.
/// 업데이트가 필요하면 그 킴과 끔을 실행시키는 것으로 함. = UpdateMap()
/// 
/// </summary>

public class Perceive
{
	public const int HEIGHTTHRESHOLD = 2;

	public const int MAPX = 200;
	public const int MAPY = 200;
	public static MapData[,,] fullMap = new MapData[MAPY, MAPX, 2]; //여기에 지형 관련 정보를 모두 저장.
	//public static float averageHeight;
	
	public bool isPlayer;
	public List<UnitController> founds 
	{
		get
		{
			if (isPlayer)
			{
				List<UnitManageData> enemyCont = EnemyPosGen.instance.myControls.FindAll(x => x.con.isSeen());
				List<UnitController> c = new List<UnitController>();
				for (int i = 0; i < enemyCont.Count; i++)
				{
					c.Add(enemyCont[i].con);
				}
				return c;
			}
			else
			{
				return UnitSelectManager.Instance.unitList.Cast<UnitController>().ToList().FindAll(x=>x.isSeen());
			}
		}
	}

	public const int GROUNDMASK =1 <<  8;
	public const int CONSTRUCTMASK =1 <<  10;


	
	public int[,,] map; //여기에는 보이는 상태 관련 정보만 저장.
	int[,,] prevMap; 

	public delegate void UpdMaps(Vector3Int startPos, int dist);

	List<MapUpdateBhv> ons = new List<MapUpdateBhv>();
	List<MapUpdateBhv> offs = new List<MapUpdateBhv>();


	public static void InitMap()
	{
		RaycastHit hit;
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				fullMap[y, x, 0].x = x;
				fullMap[y, x, 0].y = y;
				fullMap[y, x, 0].height = 0;
				fullMap[y, x, 0].Id = 0;
				fullMap[y, x, 1].x = x;
				fullMap[y, x, 1].y = y;
				fullMap[y, x, 1].height = 0;
				fullMap[y, x, 1].Id = 0;

				Vector3 pos = IdxVectorToPos(new Vector3Int(x, y));
				pos.y = 100;
				
				if(Physics.SphereCast(pos, 0.1f, Vector3.down, out hit, 200f, GROUNDMASK))
				{
					fullMap[y, x, 0].height = (int)hit.point.y;
					if (hit.transform.CompareTag("WATER"))
					{
						fullMap[y, x, 0].info = GroundState.Water;
					}
					else
					{
						fullMap[y, x, 0].info = GroundState.Ground;
						//averageHeight += fullMap[y, x, 0].height;
					}
					
				}
			}
		}
		//averageHeight /= 40000;
		
	}

	public void ResetMap(bool isP)
	{
		map = new int[MAPY, MAPX, 2];
		prevMap = new int[MAPY, MAPX, 2];
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x, 0] = 0;
				map[y, x, 1] = 0;
				prevMap[y, x, 1] = 0;
				prevMap[y, x, 1] = 0;
			}
		}
		isPlayer = isP;
	}

	public void AllEnableTmp()
	{
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x, 0] = 1;
				prevMap[y, x, 0] = 1;
			}
		}
	}

	#region OnAndOffManagement

	
	public void UpdateMap() //이걸 Eye에서 지속적으로 업데이트함.
	{
		if(offs.Count > 0)
		{
			UpdateOffs();
		}
		if(ons.Count > 0)
		{
			UpdateOns();
		}
	}

	public void AddOnUpd(Vector3Int startPos, int dist)
	{
		MapUpdateBhv h = new MapUpdateBhv(UpdateMapRecurOn, startPos, dist);
		ons.Add(h);
	}

	public void AddOffUpd(Vector3Int startPos, int dist)
	{
		MapUpdateBhv h = new MapUpdateBhv(UpdateMapRecurOff, startPos, dist);
		offs.Add(h);
	}
	

	void UpdateOns()
	{
		for (int i = 0; i < ons.Count; i++)
		{
			ons[i].Invoke();
		}
		ons.Clear();
	}

	void UpdateOffs()
	{
		for (int i = 0; i < offs.Count; i++)
		{
			offs[i].Invoke();
		}
		offs.Clear();
	}

	void UpdateMapRecurOn(Vector3Int startPos, int distance)
	{
		prevMap = (int[,,])map.Clone();
		List<GroundBreak> strts = new List<GroundBreak>();
		for (int i = 0; i < 360; ++i)
		{
			List<GroundBreak> l = UpdateMapRayRecur(startPos, distance, i, fullMap[startPos.y, startPos.x, startPos.z].height, true);
			for (int j = 0; j < l.Count; ++j)
			{
				if (!strts.Contains(l[j]))
				{
					strts.Add(l[j]);
				}
			}
		}

		if (isPlayer)
		{
			
			for (int i = 0; i < strts.Count; i++)
			{
				if(strts[i] != null)
				{
					strts[i].See(true);
					strts[i].CheckVis();
				}
				
			}
		}
	}

	void UpdateMapRecurOff(Vector3Int startPos, int distance)
	{
		prevMap = (int[,,])map.Clone();
		List<GroundBreak> strts = new List<GroundBreak>();

		for (int i = 0; i < 360; ++i)
		{
			List<GroundBreak> l = UpdateMapRayRecur(startPos, distance, i, fullMap[startPos.y, startPos.x, startPos.z].height, false);
			for (int j = 0; j < l.Count; ++j)
			{
				if (!strts.Contains(l[j]))
				{
					strts.Add(l[j]);
				}
				
			}
		}
		
		
		if (isPlayer)
		{
			for (int i = 0; i < strts.Count; i++)
			{
				strts[i].See(false);
			}
		}
	}
	#endregion

	List<GroundBreak> UpdateMapRayRecur(Vector3Int pos, int distance, int angle, int height, bool isOn) //둥그렇게 시야 밝히기
	{
		float xAccumulate = 0;
		float yAccumulate = 0;
		float xInc = Mathf.Cos(angle * Mathf.Deg2Rad);
		float yInc = Mathf.Sin(angle * Mathf.Deg2Rad);
		int xIdx = 0, yIdx = 0;
		List<GroundBreak> foundStrts = new List<GroundBreak>();
		for (int i = 0; i < distance; i++)
		{
			yIdx = pos.y + (int)yAccumulate;
			xIdx = pos.x + (int)xAccumulate;
			xIdx = xIdx < 0 ? 0 : xIdx >= MAPX ? MAPX - 1 : xIdx;
			yIdx = yIdx < 0 ? 0 : yIdx >= MAPY ? MAPY - 1 : yIdx;
			if (fullMap[yIdx, xIdx, 0].height <= height + HEIGHTTHRESHOLD)
			{
				if (isOn)
				{
					map[yIdx, xIdx, 0] += 1;
				}
				else
				{
					map[yIdx, xIdx, 0] -= 1;
				}
				if (fullMap[yIdx, xIdx, 1].Id != 0 && fullMap[yIdx, xIdx, 1].height <= height + HEIGHTTHRESHOLD)
				{
					if (isOn)
					{
						map[yIdx, xIdx, 1] += 1;
					}
					else
					{
						map[yIdx, xIdx, 1] -= 1;
					}
					foundStrts.Add(ConstructBuild.instance.strtIdPair[fullMap[yIdx, xIdx, 1].Id]);
				}
			}
			else
			{
				break;
			}
			
				
			yAccumulate += yInc;
			xAccumulate += xInc;
		}
		return foundStrts;
	}
	

	public static Vector3Int PosToIdxVector(Vector3 pos) //층은 따로 할당
	{
		int x = (int)pos.x + MAPX / 2;
		int y = -(int)pos.z + MAPY / 2;
		x = x < 0 ? 0 : x >= MAPX ? MAPX - 1 : x;
		y = y < 0 ? 0 : y >= MAPY ? MAPY - 1 : y;
		
		Vector3Int idx = new Vector3Int(y, x);
		return idx;
	}
	public static Vector3 IdxVectorToPos(Vector3Int idx) // Y는 필요할 경우 따로 할당.
	{
		float x = idx.y - MAPX / 2;
		//float y = fullMap[idx.y, idx.x, idx.z].height;
		float z =  - idx.x + MAPY / 2;
		Vector3 pos = new Vector3(x, 0, z);
		return pos;
	}
	
	
}
