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
	public void UpdateBridgeTexture(BridgeRender bridge, Vector2Int fromPos, int rad)
	{
		Texture2D t = new Texture2D((int)bridge.length, 15);


		for (int y = fromPos.y-rad; y <fromPos.y + rad; y++)
		{
			for (int x = fromPos.x- rad; x < fromPos.x + rad; x++)
			{
				if(bridge.pos.x + (int)bridge.length > x && bridge.pos.y + (int)bridge.length > y && bridge.pos.x - (int)bridge.length < x && bridge.pos.y - (int)bridge.length < y)
				{
					//둥그렇게에서 그 거리를 구하기.
					Vector2Int v = new Vector2Int(y, x);
					v -= fromPos;
					if (v.sqrMagnitude <= 1.2f && v.sqrMagnitude >= 0.8f)
					{
						t.SetPixel(y, x, Color.clear);
					}
					else
					{
						t.SetPixel(y, x, Color.black);
					}
				}
				
			}
		}
		

		t.Apply();
		Graphics.CopyTexture(t, bridge.mat.GetTexture("_MaskTex"));
	}

	public void UpdateTexture(int[,,] map, int[,,] prevMap)
	{
		for (int y = 0; y < Perceive.MAPY; ++y)
		{
			for (int x = 0; x < Perceive.MAPX; ++x)
			{
				if ((map[y, x, 0] <= 0 && prevMap[y, x, 0] > 0) || (map[y, x, 0] > 0 && prevMap[y, x, 0] <= 0))
				{
					Color c;
					if (map[y, x, 0] > 0)
					{
						c = Color.clear;
						
					}
					else
					{
						c = Color.black;
					}

					tex.SetPixel(y,Perceive.MAPX - x, c);
					
				}
				if (map[y, x, 0] > 0)
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
