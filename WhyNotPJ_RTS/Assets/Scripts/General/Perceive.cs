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
	public int visiblity; 
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
	public static MapData[,,] fullMap; //���⿡ ���� ���� ������ ��� ����.

	public bool isPlayer;

	public int ground = 8;


	//int�� �����Ұ���.
	public MapData[,,] map; //���⿡�� ���̴� ���� ���� ������ ����.
	MapData[,,] prevMap; //���⿡�� ������ ���̴� ���� ���� ������ ����.

	public delegate void UpdMaps(Vector3Int startPos, int dist);

	List<MapUpdateBhv> ons = new List<MapUpdateBhv>();
	List<MapUpdateBhv> offs = new List<MapUpdateBhv>();


	public void InitMap(bool isPlayer)
	{
		RaycastHit hit;
		map = new MapData[MAPY, MAPX, 2];
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x, 0].x = x;
				map[y, x, 0].y = y;
				map[y, x, 0].height = 0;
				map[y, x, 0].visiblity = 0;
				map[y, x, 0].emptyVal = false;
				map[y, x, 1].x = x;
				map[y, x, 1].y = y;
				map[y, x, 1].height = 0;
				map[y, x, 1].visiblity = 1;
				map[y, x, 1].emptyVal = true;


				Vector3 pos = IdxVectorToPos(new Vector3Int(x, y));
				pos.y = 100;

				Physics.Raycast(pos, Vector3.down, out hit, 200f, 1 << ground);
				map[y, x, 0].height = (int)hit.point.y;
			}
		}
		prevMap = (MapData[,,])map.Clone();
		this.isPlayer = isPlayer;
	}

	public void AllEnableTmp()
	{
		for (int y = 0; y < MAPY; y++)
		{
			for (int x = 0; x < MAPX; x++)
			{
				map[y, x, 0].visiblity = int.MaxValue / 2;
				prevMap[y, x, 0].visiblity = int.MaxValue / 2;
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
		prevMap = (MapData[,,])map.Clone();

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, map[startPos.y, startPos.x, startPos.z].height, true);
		}

		if (isPlayer)
		{
			FogOfWar.instance.UpdateTexture(map, prevMap);
		}
	}

	void UpdateMapRecurOff(Vector3Int startPos, int distance)
	{
		prevMap = (MapData[,,])map.Clone();

		for (int i = 0; i < 360; ++i)
		{
			UpdateMapRayRecur(startPos, distance, i, map[startPos.y, startPos.x, startPos.z].height, false);
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
			MapData examiner = map[yIdx, xIdx, 1].emptyVal ? map[yIdx, xIdx, 0] : map[yIdx, xIdx, 1];
			if (examiner.height > height + HEIGHTTHRESHOLD)
				break;
			if(isOn)
				examiner.visiblity += 1;
			else
				examiner.visiblity -= 1;

			if(map[yIdx, xIdx, 1].emptyVal)
			{
				map[yIdx, xIdx, 0] = examiner;
			}
			else
			{
				map[yIdx, xIdx, 1] = examiner;
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
		float z =  - idx.x + MAPY / 2;
		Vector3 pos = new Vector3(x, 0, z);
		return pos;
	}
	
	
}
