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
		Texture2D t = new Texture2D((int)bridge.length, 15);


		for (int y = fromPos.y-rad; y <fromPos.y + rad; y++)
		{
			for (int x = fromPos.x- rad; x < fromPos.x + rad; x++)
			{
				if(bridge.pos.x + (int)bridge.length > x && bridge.pos.y + (int)bridge.length > y && bridge.pos.x - (int)bridge.length < x && bridge.pos.y - (int)bridge.length < y)
				{
					//�ձ׷��Կ��� �� �Ÿ��� ���ϱ�.
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
