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
	public void UpdateBridgeTexture(BridgeRender bridge, Vector3Int fromPos, int rad)
	{ 
		Texture2D t = new Texture2D(100, 100);
		//Vector3 bPos = Perceive.IdxVectorToPos(bridge.pos);
		
		for (int y = 0; y < 100; y++)
		{
			for (int x = 0; x < 100; x++)
			{
				//�߽����� 50, 50
				//(x-50) * cosR - (y-50)sinR + 50 : X��
				//(x-50) * sinR + (y-50)cosR + 50 : Y��

				//1. ��ġ �� �Ÿ� ����� ����Ȯ
				//�и� �̷л� �Ϻ��ѵ�...

				//2. ���� �þ߰� �ѹ��� ������ ���� �� �� �ϳ��� ��.
				//�þ߿����� ���� �þ߰� �ѹ��� ������ ���� �����ϱ� ���ؼ�
				//int�� �����ְ��ְ� �ߴµ�....?..............
				//���⼭�� �ٸ����� ���׸����� ���� �ٸ��� ������ ������ ����� ���̴�.

				//�׷��� �ٸ����� ���� ���׸����� �����ϸ� �� ����...
				//�׷����� �ؽ��� �ϳ����ٰ� �ٸ��� �����ɶ����� �ؽ��ĸ� ����Ʈ��ó�� ���̵���?
				//�ϸ� ���� ������
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
