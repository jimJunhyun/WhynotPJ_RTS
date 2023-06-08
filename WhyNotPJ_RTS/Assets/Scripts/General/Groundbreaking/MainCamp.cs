using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamp : MonoBehaviour, IProducable
{
    public string myName => "Main Camp";

    public float produceTime => 0f;

    public Element element => new Element(0, 0, 0);

    public Action onCompleted => null;

    public GameObject prefab => gameObject;

    public bool isPlayer
    {
        get => true;
        set => isPlayer = true;
    }
    public float healthPoint => 0f;
    public float currentHealth = 100;

    public void OnHit(UnitController attacker)
	{
        currentHealth -= attacker.attackPower * (1f - (0 - attacker.defensePenetration) / (0 - attacker.defensePenetration + 100f));

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
        }
    }
}
