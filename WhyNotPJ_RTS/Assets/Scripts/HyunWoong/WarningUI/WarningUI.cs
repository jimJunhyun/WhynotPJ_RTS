using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    public static WarningUI Instance;

    public List<GameObject> _eventList = new List<GameObject>(); 
    public GameObject _eventCard;        
    private Transform _contentTrm;      

    private float _curHeight = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _contentTrm = GameObject.Find("Content").transform;

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(_eventCard);
            _eventList.Add(obj);
            obj.transform.parent = _contentTrm;
            obj.SetActive(false);
        }
    }

    [ContextMenu("NewCard")]
    public void NewCard()
    {
        ContentAdder();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ContentAdder();
        }
    }

    private void ContentAdder()
    {
        GameObject obj = null;
        foreach (GameObject go in _eventList)
        {
            if (!go.activeSelf)
            {
                obj = go;
                obj.SetActive(true);
                break;
            }
        }
        if (!obj)
        {
            obj = Instantiate(_eventCard);
            _eventList.Add(obj);
            obj.transform.parent = _contentTrm;
        }
    }
}
