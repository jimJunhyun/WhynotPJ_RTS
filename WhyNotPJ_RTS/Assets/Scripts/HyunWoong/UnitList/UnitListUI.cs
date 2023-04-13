using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListUI : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();   //���õ� ���ֵ��� �� ����Ʈ
    public List<GameObject> selectedList = new List<GameObject>();   //���õ� ���ֵ��� �� ����Ʈ
    public GameObject unitCard;         //UI���� ������ ������ ��Ÿ���� ī��
    private Transform _contentTrm;      //ī�尡 �� ��ġ

    private float _curHeight = 0;

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
        foreach(GameObject go in unitList)
        {
            if (!go.activeSelf)
            {
                obj = go;
                obj.SetActive(true);
                selectedList.Add(obj);
                break;
            }
        }
        if (!obj)
        {
            obj = Instantiate(unitCard);
            unitList.Add(obj);
            selectedList.Add(obj);
            obj.transform.parent = _contentTrm;
        }
    }

    [ContextMenu("ClearList")]
    private void ContentClear()
    {
        foreach(GameObject go in selectedList)
        {
            go.SetActive(false);
        }
        selectedList.Clear();
    }
}
