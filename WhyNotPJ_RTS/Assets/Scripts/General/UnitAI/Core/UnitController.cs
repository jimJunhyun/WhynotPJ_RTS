using Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour, IProducable, ISelectable
{
    #region Unit Attributes
    [Header("Unit Attributes"), SerializeField]
    private string m_myName;
    [SerializeField]
    private float m_produceTime;
    [SerializeField]
    private int vio, def, rec;
    private Action m_onCompleted;

    [SerializeField]
    private bool m_isPlayer;

    public string myName => m_myName;
    public float produceTime => m_produceTime;
    public Element element => new Element(vio, def, rec);
    public Action onCompleted => m_onCompleted;
	public GameObject prefab => gameObject;

    public bool isPlayer => m_isPlayer;
    #endregion

    #region Unit Status
    [Header("Unit Status")]
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public float defensePenetration;
    [Space(20), SerializeField]
    private float m_healthPoint;
    public float healthPoint
    {
        get
        {
            return m_healthPoint;
        }

        set
        {
            m_healthPoint = value;
        }
    }
    public float defensePower;
    [Space(20)]
    public float moveSpeed;
    public float detectRange = 5f;
    #endregion

    private Dictionary<State, IUnitState> stateDictionary = null;
    private IUnitState currentStateScript;
    public IUnitState CurrentStateScript => currentStateScript;
    public State currentState;

    public LayerMask whatIsMainCamp, whatIsUnit, whatIsConstruction;

    private UnitMove unitMove;

    //target
    [HideInInspector]
    public MainCamp mainCamp = null;
    [HideInInspector]
    public UnitController enemy = null;
    [HideInInspector]
    public GroundBreak construction = null;

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

        unitMove = GetComponent<UnitMove>();

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
        if (currentState == State.Dead)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                unitMove.SetTargetPosition(hit.point);
            }
        }
#endif

        currentStateScript.UpdateState();
    }

    public void ChangeState(State type)
    {
        currentStateScript?.OnExitState();

        currentStateScript = stateDictionary[type];
        currentState = type;

        currentStateScript?.OnEnterState();
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
}
