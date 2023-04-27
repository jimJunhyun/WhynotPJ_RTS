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
				earthTex.SetPixel(y, Perceive.MAPX - x, Color.clear);
			}
		}
	}
	public void UpdateBridgeTexture(BridgeRender bridge, Vector2Int fromPos, int rad)
	{ 
		Texture2D t = new Texture2D(100, 100);
		//Vector3 bPos = Perceive.IdxVectorToPos(bridge.pos);
		for (int y = fromPos.y-rad; y < fromPos.y + rad; y++)
		{
			for (int x = fromPos.x- rad; x < fromPos.x + rad; x++)
			{
				if(x * x + y * y <= rad * rad)
				{
					Vector3 p = Perceive.IdxVectorToPos(new Vector3Int(y, x, 0));
					int cx = Mathf.Clamp(bridge.pos.x - (int)p.x, 0, 100);
					int cy = Mathf.Clamp(bridge.pos.y - (int)p.y, 0, 100);
					t.SetPixel(cy, cx, Color.blue);
					if (x > bridge.pos.x - rad && x < bridge.pos.x + rad && y > bridge.pos.y - rad && y < bridge.pos.y + rad)
					{
					}

				}
				//Vector3 v = Perceive.IdxVectorToPos(new Vector3Int(y, x));
				
					
					//�ձ׷��Կ��� �� �Ÿ��� ���ϱ�.
					//Vector2Int v = new Vector2Int(y, x);
					//v -= fromPos;
					//if (v.sqrMagnitude <= rad * rad * 1.2)
					//{
					//	t.SetPixel(y, x, Color.clear);
					//}
					//else
					//{
					//	t.SetPixel(y, x, Color.black);
					//}
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
