using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
	public int x;
	public int y;
	public int height;
	public static float GetDist(Vector3 from, Vector3 to) //�ϴ��� ������
	{
		from.y = 0;
		to.y = 0;
		Vector3 dist = to - from;
		return dist.magnitude;
	}
	public static float GetDistSqr(Vector3 from, Vector3 to) //�ϴ��� ������
	{
		from.y = 0;
		to.y = 0;
		Vector3 dist = to - from;
		return dist.sqrMagnitude;
	}

	//�̰� �������.
	//public int visiblity; 
	// bool ���� int �� �ٲ����μ� ������ �ִ� ���� �þ� ���� �����ϱ� ������.
	//�����ӵ� ������. ������� �ӵ��� �����δٴ� ���� �Ͽ� ����Ȳ ���� 90 �̻��� �������� ������.
	//�������� �ӵ��� �����̸� �ָ������µ�, �ϴ� �׷��� ���������� �ʴ´�.

	public bool emptyVal;
}

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

//�÷��̾�� �ǽð� ���� ȹ�� �Ұ���.
//AI�� �ǽð� ���� ȹ�� ����.


/// <summary>
/// Ű�� ���� ������ ������, ���� ������ ��ĥ�� �� �� �̻��� �þ߰� ��ġ�� �������� �����Ÿ��� �߻���.
/// ���⸦ ���� �� �� Ű�⸦ ���������μ�, ���ڰŸ��� ����.
/// �׷��� ���ؼ� �� ���ΰ�ħ�� ����ü�� �����, �̰��� ����Ͽ� Ŵ�� ���� ����Ʈ�� ����.
/// ������Ʈ�� �ʿ��ϸ� �� Ŵ�� ���� �����Ű�� ������ ��. = UpdateMap()
/// 
/// 
/// �ϴ� ���� ������ ������ ��� �װ��� �켱������ �����.
/// �׸��� ���� ������ �������� ���� ������ �ٽ� �����.
/// </summary>

public class Perceive
{
	const int HEIGHTTHRESHOLD = 2;

	public const int MAPX = 200;
	public const int MAPY = 200;
	public static MapData[,,] fullMap = new MapData[MAPY, MAPX, 2]; //���⿡ ���� ���� ������ ��� ����.

	public bool isPlayer;

	public const int GROUNDMASK =1 <<  8;


	//int�� �����Ұ���. (vis�� ���� ����)
	public int[,,] map; //���⿡�� ���̴� ���� ���� ������ ����.
	int[,,] prevMap; //���⿡�� ������ ���̴� ���� ���� ������ ����.

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
				fullMap[y, x, 0].emptyVal = false;
				fullMap[y, x, 1].x = x;
				fullMap[y, x, 1].y = y;
				fullMap[y, x, 1].height = 0;
				fullMap[y, x, 1].emptyVal = true;


				Vector3 pos = IdxVectorToPos(new Vector3Int(x, y));
				pos.y = 100;

				Physics.Raycast(pos, Vector3.down, out hit, 200f, GROUNDMASK);
				fullMap[y, x, 0].height = (int)hit.point.y;
			}
		}
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

	
	public void UpdateMap() //�̰� Eye���� ���������� ������Ʈ��.
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

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, fullMap[startPos.y, startPos.x, startPos.z].height, true);
		}

		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	void UpdateMapRecurOff(Vector3Int startPos, int distance)
	{
		prevMap = (int[,,])map.Clone();

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, fullMap[startPos.y, startPos.x, startPos.z].height, false);
		}

		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	void UpdateMapRayRecur(Vector3Int pos, int distance, int angle, int height, bool isOn) //�ձ׷��� �þ� ������
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

				if (!fullMap[yIdx, xIdx, 1].emptyVal && fullMap[yIdx, xIdx, 1].height <= height + HEIGHTTHRESHOLD)
				{
					if (isOn)
					{
						map[yIdx, xIdx, 1] += 1;
					}
					else
					{
						map[yIdx, xIdx, 1] -= 1;
					}
				}
			}
			else
			{
				break;
			}
			
				
			yAccumulate += yInc;
			xAccumulate += xInc;
		}
	}
	#endregion

	public static Vector3Int PosToIdxVector(Vector3 pos) //���� ���� �Ҵ�
	{
		int x = (int)pos.x + MAPX / 2;
		int y = -(int)pos.z + MAPY / 2;
		x = x < 0 ? 0 : x >= MAPX ? MAPX - 1 : x;
		y = y < 0 ? 0 : y >= MAPY ? MAPY - 1 : y;
		
		Vector3Int idx = new Vector3Int(y, x);
		return idx;
	}
	public static Vector3 IdxVectorToPos(Vector3Int idx) // Y�� �ʿ��� ��� ���� �Ҵ�.
	{
		float x = idx.y - MAPX / 2;
		//float y = fullMap[idx.y, idx.x, idx.z].height;
		float z =  - idx.x + MAPY / 2;
		Vector3 pos = new Vector3(x, 0, z);
		return pos;
	}
	
	
}
