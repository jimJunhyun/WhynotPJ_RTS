using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainCamp : MonoBehaviour, IProducable
{
    public string myName => "Main Camp";

    public float produceTime => 0f;

    public Element element => new Element(0, 0, 0);

    public Action onCompleted => null;

    public GameObject prefab => gameObject;

	public Sprite image => null;

    public bool playerSide;
    public bool isPlayer
    {
        get => playerSide;
        set => isPlayer = true;
    }
    public float healthPoint => 0f;

	public float currentHealth = 100;

    public Action onDamaged;
    public UnityEvent OnDestroyed;

    public void OnHit(UnitController attacker)
	{
        currentHealth -= attacker.attackPower * (1f - (0 - attacker.defensePenetration) / (0 - attacker.defensePenetration + 100f));
        onDamaged.Invoke();
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            OnDestroyed?.Invoke();
        }
    }

    public void AddOnDamaged(Action onDam)
	{
        onDamaged += onDam;
	}
}
