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
