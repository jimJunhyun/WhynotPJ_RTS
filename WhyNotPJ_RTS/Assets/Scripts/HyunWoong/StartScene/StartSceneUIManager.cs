using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUIManager : MonoBehaviour
{
    private Button[] _btns;
    [SerializeField] private GameObject _optionWindow;
    private void Awake()
    {
        _btns = transform.Find("Buttons").GetComponentsInChildren<Button>();

        _btns[0].onClick.AddListener(() => StartGame());
        _btns[1].onClick.AddListener(() => OptionWindowActive());
        _btns[2].onClick.AddListener(() => QuitGame());
    }

    private void StartGame()
    {
        print("게임 시작");
    }

    private void OptionWindowActive()
    {
        _optionWindow.SetActive(true);
        gameObject.SetActive(false);
    }

    private void QuitGame()
    {
        print("게임 종료");
    }
}
