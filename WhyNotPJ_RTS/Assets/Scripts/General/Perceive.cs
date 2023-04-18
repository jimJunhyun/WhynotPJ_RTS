using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public int visiblity; // bool 에서 int 로 바꿈으로서 가만히 있는 놈의 시야 등을 관리하기 편해짐.
	//프레임도 괜찮음. 상식적인 속도로 움직인다는 가정 하에 현상황 기준 90 이상의 프레임을 내더라.
	//비상식적인 속도로 움직이면 애매해지는데, 일단 그렇게 움직이지는 않는다.
}

public struct MapUpdateBhv
{
	Perceive.UpdMaps updateDel;
	Vector2Int startPos;
	int distance;

	public MapUpdateBhv(Perceive.UpdMaps upd, Vector2Int pos, int dist)
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
/// </summary>

public class Perceive
{
	const int HEIGHTTHRESHOLD = 2;

	public const int MAPX = 200;
	public const int MAPY = 200;

	public bool isPlayer;

	public int ground = 8;

	public MapData[,] map;
	MapData[,] prevMap;

	public delegate void UpdMaps(Vector2Int startPos, int dist);

	List<MapUpdateBhv> ons = new List<MapUpdateBhv>();
	List<MapUpdateBhv> offs = new List<MapUpdateBhv>();


	public void InitMap(bool isPlayer)
	{
		RaycastHit hit;
		map = new MapData[MAPY, MAPX];
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x].x = x;
				map[y, x].y = y;
				map[y, x].height = 0;
				map[y, x].visiblity = 0;

				Vector3 pos = IdxVectorToPos(new Vector2Int(x, y));
				pos.y = 100;

				Physics.Raycast(pos, Vector3.down, out hit, 200f, 1 << ground);
				map[y, x].height = (int)hit.point.y;
			}
		}
		prevMap = (MapData[,])map.Clone();
		this.isPlayer = isPlayer;
	}

	public void AllEnableTmp()
	{
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x].visiblity = int.MaxValue / 2;
				prevMap[y, x].visiblity = int.MaxValue / 2;
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

	public void AddOnUpd(Vector2Int startPos, int dist)
	{
		MapUpdateBhv h = new MapUpdateBhv(UpdateMapRecurOn, startPos, dist);
		ons.Add(h);
	}

	public void AddOffUpd(Vector2Int startPos, int dist)
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

	void UpdateMapRecurOn(Vector2Int startPos, int distance)
	{
		prevMap = (MapData[,])map.Clone();

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, map[startPos.y, startPos.x].height, true);
		}

		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	void UpdateMapRecurOff(Vector2Int startPos, int distance)
	{
		prevMap = (MapData[,])map.Clone();

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, map[startPos.y, startPos.x].height, false);
		}

		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	void UpdateMapRayRecur(Vector2Int pos, int distance, int angle, int height, bool isOn) //둥그렇게 시야 밝히기
	{
		float xAccumulate = 0;
		float yAccumulate = 0;
		float xInc = Mathf.Cos(angle * Mathf.Deg2Rad);
		float yInc = Mathf.Sin(angle * Mathf.Deg2Rad);
		int xIdx = 0, yIdx = 0;
		for (int i = 0; i < distance; i++)
		{
			yIdx = pos.y + (int)yAccumulate;
			xIdx = pos.x + (int)xAccumulate;
			xIdx = xIdx < 0 ? 0 : xIdx >= MAPX ? MAPX - 1 : xIdx;
			yIdx = yIdx < 0 ? 0 : yIdx >= MAPY ? MAPY - 1 : yIdx;
			if(map[yIdx, xIdx].height > height + HEIGHTTHRESHOLD)
				break;
			if(isOn)
				map[yIdx, xIdx].visiblity += 1;
			else
				map[yIdx, xIdx].visiblity -= 1;

			yAccumulate += yInc;
			xAccumulate += xInc;
		}
	}
	#endregion

	public static Vector2Int PosToIdxVector(Vector3 pos)
	{
		int x = (int)pos.x + MAPX / 2;
		int y = -(int)pos.z + MAPY / 2;
		x = x < 0 ? 0 : x >= MAPX ? MAPX - 1 : x;
		y = y < 0 ? 0 : y >= MAPY ? MAPY - 1 : y;
		Vector2Int idx = new Vector2Int(y, x);
		return idx;
	}
	public static Vector3 IdxVectorToPos(Vector2Int idx) // Y는 필요할 경우 따로 할당.
	{
		float x = idx.y - MAPX / 2;
		float z =  - idx.x + MAPY / 2;
		Vector3 pos = new Vector3(x, 0, z);
		return pos;
	}
	
	
}
