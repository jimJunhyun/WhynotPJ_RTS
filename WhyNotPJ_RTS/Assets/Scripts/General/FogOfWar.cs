using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �̰� �÷��̾�� ���̴� �Ͱ� ���õǾ�����.
/// 
/// ������ ��ũ��Ʈ���� �ڽ� ��ġ�� ���δٸ� ������ ������ ���� ��Ȱ��ȭ��Ű�鼭
/// �ڽ��� �Ⱥ��̵��� �Ұ���.
/// </summary>
public class FogOfWar : MonoBehaviour
{
	public static FogOfWar instance;

	public Material mat;

    Texture2D tex;
    Texture2D earthTex;
    

	private void Awake()
	{
		instance =this;

		tex = new Texture2D(Perceive.MAPX, Perceive.MAPY);
		earthTex = new Texture2D(Perceive.MAPX, Perceive.MAPY);
		InitEarth();
		mat.SetTexture("_Masker", tex);
		mat.SetTexture("_EarthMask", earthTex);
		tex.Apply();
		earthTex.Apply();
	}

	public void InitEarth()
	{
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				earthTex.SetPixel(y, x, Color.clear);
			}
		}
	}

	public void UpdateTexture(MapData[,] map, MapData[,] prevMap)
	{
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				if(map[y, x].visiblity != prevMap[y, x].visiblity)
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

					tex.SetPixel(y, x, c);
					
				}
				if (map[y, x].visiblity)
				{
					earthTex.SetPixel(y, x, Color.black);
				}
			}
		}
		mat.SetTexture("_Masker", tex);
		mat.SetTexture("_EarthMask", earthTex);
		tex.Apply();
		earthTex.Apply();
	}
}
