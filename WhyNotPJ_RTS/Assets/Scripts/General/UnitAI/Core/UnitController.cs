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
}
