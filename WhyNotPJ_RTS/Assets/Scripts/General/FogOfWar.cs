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
		Vector3 dist = pos - new Vector3(x,  0, y);
		return dist.magnitude;
	}
	public bool visiblity;
}

public class FogOfWar : MonoBehaviour
{
	public int mapX = 200;
	public int mapY = 200;
	public Material mat;


	MapData[,] map;

    Texture2D tex;
    

	private void Awake()
	{
		map = new MapData[mapX, mapY];

		tex = new Texture2D(mapX, mapY);
		
		InitMap(map);
		UpdateTexture(map);
		UpdateMapTemp(100, 100, 5);
		
	}

	public void UpdateTexture(MapData[,] map)
	{
		for (int y = 0; y < mapY; ++y)
		{
			for (int x = 0; x < mapX; ++x)
			{
				Color c;
				if (map[y, x].visiblity)
				{
					c = Color.clear;
				}
				else
				{
					c = Color.black;
				}
				
				tex.SetPixel(x, y, c);
			}
		}
		mat.SetTexture("_Masker", tex);
		//mat.
	}

	public void UpdateMapTemp(int startX, int startY, int rad)
	{
		for (int i = -rad; i <= rad; i++)
		{
			for (int j = -rad; j <= rad; j++)
			{
				if(((i+ 0.5) * (i + 0.5) + (j + 0.5) * (j + 0.5)) - (rad * rad) <= 1)
				{
					map[startX + i, startY + j].visiblity = true;
					Color c = Color.black;
					if (map[startX + i, startY + j].visiblity)
					{
						c = Color.clear;
					}

					tex.SetPixel(startX + i, startY + j, c);
				}
			}
		}
		mat.SetTexture("_Masker", tex);
	}

	public void InitMap(MapData[,] map)
	{
		for (int y = 0; y < mapY; y++)
		{
			for (int x = 0; x < mapX; x++)
			{
				map[y, x].x = x;
				map[y, x].y = y;
				map[y, x].height = 0; //ray ½÷
				map[y, x].visiblity = false;
			}
		}
	}
}
