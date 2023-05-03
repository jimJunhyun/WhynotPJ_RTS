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
	public void UpdateBridgeTexture(BridgeRender bridge, Vector3Int fromPos, int rad)
	{ 
		Texture2D t = new Texture2D(100, 100);
		//Vector3 bPos = Perceive.IdxVectorToPos(bridge.pos);
		
		for (int y = 0; y < 100; y++)
		{
			for (int x = 0; x < 100; x++)
			{
				//중심점은 50, 50
				//(x-50) * cosR - (y-50)sinR + 50 : X점
				//(x-50) * sinR + (y-50)cosR + 50 : Y점

				//1. 위치 및 거리 계산이 부정확
				//분명 이론상 완벽한데...

				//2. 여러 시야가 한번에 지나갈 때에 그 중 하나만 됨.
				//시야에서는 여러 시야가 한번에 지나갈 때를 수정하기 위해서
				//int만 더해주고빼주고 했는데....?..............
				//여기서는 다리마다 마테리얼이 전부 다르기 때문에 문제가 생기는 것이다.

				//그러면 다리마다 같은 마테리얼을 적용하면 또 모르지...
				//그러려면 텍스쳐 하나에다가 다리가 생성될때마다 텍스쳐를 포스트잇처럼 붙이듯이?
				//하면 되지 않을까
				if ((bridge.pos + new Vector3Int((int)((x - 50) * Mathf.Cos(bridge.angleRad) - (y - 50) * Mathf.Cos(bridge.angleRad) + 50), (int)((x - 50) * Mathf.Sin(bridge.angleRad) + (y - 50) * Mathf.Cos(bridge.angleRad) + 50)) - fromPos).sqrMagnitude <= rad * rad)
				{
					t.SetPixel(100 - x, y, Color.clear);
				}
			}
		}
		

		t.Apply();
		bridge.mat.SetTexture("_Masker", t);
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
