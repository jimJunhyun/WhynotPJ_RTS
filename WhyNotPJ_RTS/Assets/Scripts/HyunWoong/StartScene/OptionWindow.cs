using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [SerializeField]
    private Button[] _btns;
    [SerializeField] 
    Button _quitBtn;
    [SerializeField]
    private Image[] _panels;

    private StartSceneUIManager _ssuiManager;

    private void Awake()
    {
        _btns = transform.Find("Buttons").GetComponentsInChildren<Button>();

        _ssuiManager = GameObject.Find("MainSceneUI").GetComponent<StartSceneUIManager>();

        #region Buttons AddListner
        _btns[0].onClick.AddListener(() => WindowActive(0));
        _btns[1].onClick.AddListener(() => WindowActive(1));
        _btns[2].onClick.AddListener(() => WindowActive(2));

        _quitBtn.onClick.AddListener(() => WindowDeactive());
        #endregion
    }

    private void OnEnable()
    {
        WindowClear();
        _panels[0].gameObject.SetActive(true);
    }

    private void WindowActive(int btnIndex)
    {
        print($"BtnIndex: {btnIndex}");
        WindowClear();
        _panels[btnIndex].gameObject.SetActive(true);
    }

    private void WindowDeactive()
    {
        _ssuiManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void WindowClear()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].gameObject.SetActive(false);
        }
    }
}
