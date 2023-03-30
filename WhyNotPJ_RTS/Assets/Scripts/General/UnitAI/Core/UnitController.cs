using Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour, IProducable
{
    [SerializeField]
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

    private Dictionary<State, IUnit> stateDictionary = null;
    private IUnit currentState;

    [HideInInspector]
    public UnitMove unitMove;

    private void Awake()
    {
        stateDictionary = new Dictionary<State, IUnit>();
        Transform stateTrm = transform.Find("States");

        foreach (State state in Enum.GetValues(typeof(State)))
        {
            IUnit stateScript = stateTrm.GetComponent($"Unit{state}State") as IUnit;

            if (stateScript == null)
            {
                Debug.LogError($"There is no script: {state}");

                return;
            }

            stateScript.SetUp(transform);
            stateDictionary.Add(state, stateScript);
        }

        unitMove = GetComponent<UnitMove>();
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

    public bool IsCurrentState(State type)
    {
        return stateDictionary[type] == currentState;
    }

    public IUnit GetStateScript()
    {
        return currentState;
    }
}
