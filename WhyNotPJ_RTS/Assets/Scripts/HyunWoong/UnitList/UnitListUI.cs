using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListUI : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();   //선택된 유닛들이 들어갈 리스트
    public List<UnitDefault> selectedList = new List<UnitDefault>();   //선택된 유닛들이 들어갈 리스트
    public GameObject unitCard;         //UI에서 유닛의 스택을 나타내는 카드
    private Transform _contentTrm;      //카드가 들어갈 위치

    private float _curHeight = 0;

    [SerializeField] private float _enterSpeed = 2f;

    [SerializeField] private bool _isEnter = false;
    [SerializeField] private bool _isActivate = false;

    [SerializeField] private float _xOuterPos, _xEnterPos;
    [SerializeField] private RectTransform _mainRect;

    private void Start()
    {
        _contentTrm = GameObject.Find("Content").transform;

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(unitCard);
            unitList.Add(obj);
            obj.transform.parent = _contentTrm;
            obj.SetActive(false);
        }
    }

    [ContextMenu("NewCard")]
    public void NewCard()
    {
        ContentAdder();
    }

    public void NoCard()
    {
        ContentClear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ContentAdder();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ContentClear();
        }
    }

    private void ContentAdder()
    {
        GameObject obj = null;

        foreach (UnitDefault item in UnitManager.Instance.SelectedUnitList)
        {
            selectedList.Add(item);
        }

        foreach (UnitDefault go in selectedList)
        {
            if (!go.gameObject.activeSelf)
            {
                obj = go.gameObject;
                obj.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void EnterButton()
    {
        if (!_isActivate)
        {
            _isActivate = true;
            _isEnter = !_isEnter;
            StartCoroutine(EnterUI());
        }
    }

    float xPos;

    private IEnumerator EnterUI()
    {
        while (true)
        {
            //print(Vector2.Distance(new Vector2(_xOuterPos, 0), _mainRect.anchoredPosition));
            if (_isEnter == false && Vector2.Distance(new Vector2(_xOuterPos, 0), _mainRect.anchoredPosition) <= .5f)
            {
                xPos = _xOuterPos;
                break;
            }

            if (_isEnter && Vector2.Distance(new Vector2(_xEnterPos, 0), _mainRect.anchoredPosition) <= .5f)
            {
                xPos = _xEnterPos;
                break;
            }
            
            if (_isEnter)
            {
                xPos = Mathf.Lerp(xPos, _xEnterPos, Time.deltaTime * _enterSpeed);
            }
            else
            {
                xPos = Mathf.Lerp(xPos, _xOuterPos, Time.deltaTime * _enterSpeed);
            }
            _mainRect.anchoredPosition = new Vector2(xPos, 0);
            yield return null;
        }
        _isActivate = false;
    }

    [ContextMenu("ClearList")]
    private void ContentClear()
    {
        foreach (UnitDefault go in selectedList)
        {
            go.gameObject.SetActive(false);
        }
        selectedList.Clear();
    }
}
