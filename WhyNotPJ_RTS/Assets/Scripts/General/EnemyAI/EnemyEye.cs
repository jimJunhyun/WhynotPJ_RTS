using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyEye : MonoBehaviour
{
	public static EnemyEye instance;
    
	public Perceive perceived = new Perceive();

	private void Awake()
	{
		instance = this;

		perceived.InitMap(false);

		perceived.UpdateMap(100, 100, 5, true);
	}
}
