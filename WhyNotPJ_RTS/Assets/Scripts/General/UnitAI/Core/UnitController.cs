using Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class UnitController : MonoBehaviour, ISelectable, IProducable
{
    #region Unit Attributes
    [Header("Unit Attributes"), SerializeField]
    private string myName;
    [SerializeField]
    private float produceTime;
    [SerializeField]
    private int vio, def, rec;
    private Action onCompleted;
    
    public string _myName => myName;
    public float _produceTime => produceTime;
    public Element _element => new Element(vio, def, rec);
    public Action _onCompleted => onCompleted;

    public bool isPlayer;
    #endregion

    #region Unit Status
    public static float detectRange = 5f;
    [Header("Unit Status")]
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public float defensePenetration;
    [Space(20)]
    public float healthPoint;
    public float defensePower;
    [Space(20)]
    public float moveSpeed;
    #endregion

    private Dictionary<State, IUnitState> stateDictionary = null;
    private IUnitState currentState;
    public IUnitState CurrentState => currentState;
	public GameObject _prefab => gameObject;

    //test
    public GameObject marker;
    public bool isSelect = false;

    // ISelectable
	public bool CanDragSelect => true;
	public Vector3 WorldPos => transform.position;

	private void Awake()
    {
        //test
        marker = transform.Find("Marker").gameObject;

        stateDictionary = new Dictionary<State, IUnitState>();
        Transform stateTrm = transform.Find("States");

        foreach (State state in Enum.GetValues(typeof(State)))
        {
            IUnitState stateScript = stateTrm.GetComponent($"Unit{state}State") as IUnitState;

            if (stateScript == null)
            {
                Debug.LogError($"There is no script: {state}");

                return;
            }

            stateScript.SetUp(transform);
            stateDictionary.Add(state, stateScript);
        }
    }

    private void Start()
    {
        ChangeState(State.Wait);
    }

    private void Update()
    {
        currentState.UpdateState();

        Debug.Log(isSeen());
		if (isSeen() && !isPlayer)
		{
            //오브젝트 보이게
            Debug.Log($"발견됨 : {transform.name}");
		}
    }

    public void ChangeState(State type)
    {
        currentState?.OnExitState();

        currentState = stateDictionary[type];

        currentState?.OnEnterState();
    }

    // ISelectable
	public void OnSelectUnit()
	{
        //test
        marker.SetActive(true);
        isSelect = true;
	}

	public void OnDeselectUnit()
	{
        //test
        marker.SetActive(false);
        isSelect = false;
	}

    public IUnitState GetStateDict(State st)
	{
        return stateDictionary[st];
	}

    public bool isSeen() //이후 수정 필요. 
	{
        Vector3Int posIdx = Perceive.PosToIdxVector(transform.position);
        int floor = 0;
		if (Mathf.Abs(Perceive.fullMap[posIdx.y, posIdx.x, 0].height - transform.position.y) > 1)
		{
            floor = 1;
		}
        //Debug.Log(Perceive.fullMap[posIdx.y, posIdx.x, 0].height + " : " + transform.position.y);
		if (isPlayer)
		{
            return EnemyEye.instance.perceived.map[posIdx.y, posIdx.x, floor] >= 1;
		}
		else
		{
            return PlayerEye.instance.perceived.map[posIdx.y, posIdx.x, floor] >= 1;
        }
	}
}
