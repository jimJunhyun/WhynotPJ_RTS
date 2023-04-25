using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 이건 플레이어에게 보이는 것과 관련되어있음.
/// 
/// 유닛쪽 스크립트에서 자신 위치가 보인다면 렌더러 관련을 전부 비활성화시키면서
/// 자신을 안보이도록 할거임.
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
				earthTex.SetPixel(y, Perceive.MAPX - x, Color.clear);
			}
		}
	}

	public void UpdateTexture(MapData[,,] map, MapData[,,] prevMap)
	{
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				MapData examiner = map[y, x, 1].emptyVal ? map[y, x, 0] : map[y, x, 1];
				MapData prevExaminer = prevMap[y, x, 1].emptyVal ? prevMap[y, x, 0] : prevMap[y, x, 1];
				if ((examiner.visiblity <= 0 && prevExaminer.visiblity > 0) || (examiner.visiblity > 0 && prevExaminer.visiblity <= 0))
				{
					Color c;
					if (examiner.visiblity > 0)
					{
						c = Color.clear;
						
					}
					else
					{
						c = Color.black;
					}

					tex.SetPixel(y,Perceive.MAPX - x, c);
					
				}
				if (examiner.visiblity > 0)
				{
					earthTex.SetPixel( y,Perceive.MAPX - x, Color.black);
				}
			}
		}
		mat.SetTexture("_Masker", tex);
		mat.SetTexture("_EarthMask", earthTex);
		tex.Apply();
		earthTex.Apply();
	}
}
