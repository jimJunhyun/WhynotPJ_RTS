using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterTmp : MonoBehaviour
{
    int prevIntPosX;
    int prevIntPosY;
	public FogOfWar fow;
	// Update is called once per frame
	private void Awake()
	{
		prevIntPosX = (int)transform.position.x + 100;
		prevIntPosY = (int)transform.position.y + 100;
	}
	void Update()
    {
        if(Mathf.Abs(prevIntPosX - (int)(transform.position.x + 100) )> 1 || Mathf.Abs(prevIntPosY - (int)(transform.position.y + 100)) > 1)
		{
			fow.UpdateMapTemp((int)transform.position.x + 100, (int)transform.position.y + 100, 5);
			prevIntPosX = (int)transform.position.x + 100;
			prevIntPosY = (int)transform.position.y + 100;
		}
    }
}
