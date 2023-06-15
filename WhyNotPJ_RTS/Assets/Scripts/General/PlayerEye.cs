using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEye : MonoBehaviour
{
    public static PlayerEye instance;

	public Perceive perceived = new Perceive();


	private void Awake()
	{
		instance = this;

		perceived.ResetMap(true);
		//perceived.AllEnableTmp();
	}

	private void LateUpdate()
	{
		perceived.UpdateMap();
	}
}
