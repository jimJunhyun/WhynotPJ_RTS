using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
	public int x;
	public int y;
	public int height;
	public float GetDist(int x, int y, Vector3 pos)
	{
		pos.y = 0;
		Vector3 dist = pos - new Vector3(x, 0, y);
		return dist.magnitude;
	}
	public bool visiblity;
}

//플레이어는 실시간 정보 획득 불가능.
//AI는 실시간 정보 획득 가능.

public class Perceive
{
	public const int MAPX = 200;
	public const int MAPY = 200;

	public bool isPlayer;

	public int ground = 8;

	public MapData[,] map;
	MapData[,] prevMap;

	public void InitMap(bool isPlayer)
	{
		map = new MapData[MAPY, MAPX];
		RaycastHit hit;
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x].x = x;
				map[y, x].y = y;
				map[y, x].height = 0; //ray 쏴
				map[y, x].visiblity = false;

				Vector3 pos = IdxVectorToPos(new Vector2Int(x, y));
				pos.y = 100;

				Physics.Raycast(pos, Vector3.down, out hit, 200f, 1 << ground);
				map[y, x].height = (int)hit.point.y;
				Debug.Log($"{x} , {y} height : {map[y, x].height}");
			}
		}
		prevMap = (MapData[,])map.Clone();
		this.isPlayer = isPlayer;
	}

	public void UpdateMap(int startX, int startY, int rad, bool on)
	{
		startX = startX < 0 ? 0 : startX >= MAPX ? MAPX - 1 : startX;
		startY = startY < 0 ? 0 : startY >= MAPY ? MAPY - 1 : startY;
		prevMap = (MapData[,])map.Clone();
		for (int i = -rad; i <= rad; i++)
		{
			for (int j = -rad; j <= rad; j++)
			{
				if (((i + 0.5) * (i + 0.5) + (j + 0.5) * (j + 0.5)) - (rad * rad) <= 1) // 높이 관리 추가하기
				{
					int x = startX + i < 0 ? 0 : startX + i >= MAPX ? MAPX - 1 : startX + i;
					int y = startY + j < 0 ? 0 : startY + j >= MAPY ? MAPY - 1 : startY + j;
					if (map[y, x].visiblity != on)
					{
						map[y, x].visiblity = on;
					}
				}
			}
		}
		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}
	public void UpdateMap(Vector2Int idxXY, int rad, bool on)
	{
		prevMap = (MapData[,])map.Clone();
		for (int i = -rad; i <= rad; i++)
		{
			for (int j = -rad; j <= rad; j++)
			{
				if (((i + 0.5) * (i + 0.5) + (j + 0.5) * (j + 0.5)) - (rad * rad) <= 1) //높이 관리 추가하기
				{
					int x = idxXY.x + i < 0 ? 0 : idxXY.x + i >= MAPX ? MAPX - 1 : idxXY.x + i;
					int y = idxXY.y + j < 0 ? 0 : idxXY.y + j >= MAPY ? MAPY - 1 : idxXY.y + j;
					if (map[y, x].visiblity != on)
					{
						map[y, x].visiblity = on;
					}
				}
			}
		}
		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	public static Vector2Int PosToIdxVector(Vector3 pos)
	{
		int x = (int)pos.x + MAPX / 2;
		int y = (int)pos.z + MAPY / 2;
		x = x < 0 ? 0 : x >= MAPX ? MAPX - 1 : x;
		y = y < 0 ? 0 : y >= MAPY ? MAPY - 1 : y;
		Vector2Int idx = new Vector2Int(y, x);
		return idx;
	}
	public static Vector3 IdxVectorToPos(Vector2Int idx) // Y는 필요할 경우 따로 할당.
	{
		int x = (int)idx.x - MAPX / 2;
		int z = (int)idx.y - MAPY / 2;
		x = x < 0 ? 0 : x >= MAPX ? MAPX - 1 : x;
		z = z < 0 ? 0 : z >= MAPY ? MAPY - 1 : z;
		Vector3 pos = new Vector3(x, 0, z);
		return pos;
	}
	
}
