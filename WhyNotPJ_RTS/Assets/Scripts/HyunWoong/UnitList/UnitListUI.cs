using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ������ ����ϴ� UI�� �����ϴ� Ŭ����
/// </summary>
public class UnitInfoUI : MonoBehaviour
{
    public UnitInfoUI(GameObject prefab, Image image = null, TextMeshProUGUI text = null, Slider health = null)
	{
        this.prefab = prefab;
        this.image = image;
        this.text = text;
        this.health = health;
	}

    public GameObject prefab;
    public Image image;                 // ������ �������� ����
    public TextMeshProUGUI text;        // ������ �̸��� ����
    public Slider health;               // ������ ü���� ��Ÿ����
}

public class UnitListUI : MonoBehaviour
{
    public List<UnitInfoUI> unitCardList = new List<UnitInfoUI>();   //���õ� ���ֵ��� �� ����Ʈ
    public List<UnitController> selectedList = new List<UnitController>();   //���õ� ���ֵ��� �� ����Ʈ
    public GameObject unitCard;         //UI���� ������ ������ ��Ÿ���� ī��
    private Transform _contentTrm;      //ī�尡 �� ��ġ

    private float _curHeight = 0;

    [SerializeField] private float _enterSpeed = 2f;

    [SerializeField] private bool _isEnter = false;
    [SerializeField] private bool _isActivate = false;

    [SerializeField] private float _xOuterPos, _xEnterPos;
    [SerializeField] private RectTransform _mainRect;

    [SerializeField] Sprite image;

    private GameObject uiParent;

    private void Awake()
    {
        _contentTrm = GameObject.Find("Content").transform;
        uiParent = GameObject.Find("UnitProducer");

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(unitCard);
            unitCardList.Add(new UnitInfoUI(obj, obj.transform.Find("Image").GetComponent<Image>(), obj.GetComponentInChildren<TextMeshProUGUI>(), null));
            obj.transform.SetParent(_contentTrm);
            obj.SetActive(false);
        }
    }

    [ContextMenu("NewCard")]
    public void NewCard()
    {
        print("NewCard");
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

    public void ShowUnitInfo()
	{
        int count = UnitControllManager.Instance.SelectedUnitList.Count;
        uiParent.gameObject.SetActive(true);

        foreach (UnitInfoUI ui in unitCardList)
        {
            if (ui.prefab.gameObject.activeInHierarchy)
            {
                ui.prefab.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < count; i++)
		{
            unitCardList[i].prefab.gameObject.SetActive(true);
            unitCardList[i].text.text = UnitControllManager.Instance.SelectedUnitList[i]?.gameObject.name;
            unitCardList[i].image.sprite = image;
        }
	}

    public void HideUnitInfo()
	{
        foreach (UnitInfoUI tmp in unitCardList)
		{
            if (tmp.prefab.gameObject.activeInHierarchy)
			{
                tmp.prefab.gameObject.SetActive(false);
			}
		}

        uiParent.gameObject.SetActive(false);
    }

    private void ContentAdder()
    {
        GameObject obj = null;

        foreach (UnitController item in UnitControllManager.Instance.SelectedUnitList)
        {
            selectedList.Add(item);
        }

        foreach (UnitController go in selectedList)
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
        foreach (UnitController go in selectedList)
        {
            go.gameObject.SetActive(false);
        }
        selectedList.Clear();
    }
}
