using Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitController : PoolableMono, IProducable, ISelectable
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
    [SerializeField]
    private Sprite m_image;

    public string myName => m_myName;
    public float produceTime => m_produceTime;
    public Element element => new Element(vio, def, rec);
    public Action onCompleted => m_onCompleted;
	public GameObject prefab => gameObject;
	public Sprite image => m_image;

    public bool isPlayer
	{
        get => m_isPlayer;
        set
        {
            m_isPlayer = value;
            helmetRenderer.material = isPlayer ? teamMat[0] : teamMat[1];
        }
	}
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
    public UnitMove UnitMove => unitMove;

    //target
    [HideInInspector]
    public MainCamp mainCamp = null;
    [HideInInspector]
    public UnitController enemy = null;
    [HideInInspector]
    public GroundBreak construction = null;

    [SerializeField]
    private List<Material> teamMat;
    [SerializeField]
    private MeshRenderer helmetRenderer;

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
        //if (Input.GetMouseButtonDown(0))
        //{
        //          if(!isPlayer)
        //              return;
        //	if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //	{
        //		unitMove.SetTargetPosition(hit.point);
        //	}
        //}
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


    public bool isSeen() //���� ���� �ʿ�. 
	{
        Vector3Int posIdx = Perceive.PosToIdxVector(transform.position);
        int floor = 0;
		if (Perceive.fullMap[posIdx.x, posIdx.y, 1].Id != 0&& Mathf.Abs(Perceive.fullMap[posIdx.x, posIdx.y, 0].height - transform.position.y) > Perceive.HEIGHTTHRESHOLD)
		{
            floor = 1;
            Debug.Log("2cmd");
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

    public void InitTarget()
    {
        mainCamp = null;
        enemy = null;
        construction = null;
    }

    public void InitTarget(MainCamp mainCamp)
    {
        this.mainCamp = mainCamp;
        enemy = null;
        construction = null;
    }

    public void InitTarget(UnitController enemy)
    {
        mainCamp = null;
        this.enemy = enemy;
        construction = null;
    }

    public void InitTarget(GroundBreak construction)
    {
        mainCamp = null;
        enemy = null;
        this.construction = construction;
    }
}
