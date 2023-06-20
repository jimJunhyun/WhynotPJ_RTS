using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pauser : MonoBehaviour
{

    public Sprite reuleauxTr;
    public Sprite twoCapsule;

	Image img;
	GameObject pausePanel;
    bool paused = false;

	private void Awake()
	{
		img = GameObject.Find("Pause").GetComponent<Image>();
		pausePanel = GameObject.Find("PausePanel");
		paused = false;
		img.sprite = twoCapsule;
		Time.timeScale = 1;
		pausePanel.SetActive(false);
	}

	public void Pressed()
	{
		if (paused)
		{
			paused = false;
			img.sprite = twoCapsule;
			Time.timeScale = 1;
			pausePanel.SetActive(false);
		}
		else
		{
			paused = true;
			img.sprite = reuleauxTr;
			Time.timeScale = 0;
			pausePanel.SetActive(true);
		}
	}
}
