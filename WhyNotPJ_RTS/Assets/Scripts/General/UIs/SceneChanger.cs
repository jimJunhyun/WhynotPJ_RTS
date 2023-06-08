using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	Canvas loadingScreen;
	Slider progressBar;

	public static SceneChanger instance;

	private void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);

		progressBar = GetComponentInChildren<Slider>();
		loadingScreen = GetComponentInChildren<Canvas>();

		loadingScreen.gameObject.SetActive(false);
	}
	public async void Change(int idx)
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(idx);
		//loading.allowSceneActivation = false;
		loadingScreen.gameObject.SetActive(true);
		while (!loading.isDone)
		{
			await System.Threading.Tasks.Task.Delay(100);
			progressBar.value = loading.progress;
		}
		await System.Threading.Tasks.Task.Delay(1000);
		loadingScreen.gameObject.SetActive(false);
		//loading.allowSceneActivation = true;
	}
	public async void Change(string name)
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(name);
		//loading.allowSceneActivation = false;
		loadingScreen.gameObject.SetActive(true);
		while (!loading.isDone)
		{
			await System.Threading.Tasks.Task.Delay(100);
			progressBar.value = loading.progress;
		}
		await System.Threading.Tasks.Task.Delay(1000);
		loadingScreen.gameObject.SetActive(false);
		//loading.allowSceneActivation = true;
	}
}
