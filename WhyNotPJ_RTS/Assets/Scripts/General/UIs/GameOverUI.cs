using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    Canvas canvas;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		Off();
	}

	public void On()
	{
		canvas.enabled = true;
	}
	public void Off()
	{
		canvas.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			On();
		}
	}
}
