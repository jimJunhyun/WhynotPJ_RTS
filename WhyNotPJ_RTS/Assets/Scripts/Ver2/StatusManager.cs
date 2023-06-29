using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public static StatusManager instance;


    public Anomalies allAnomalies;


	private void Awake()
	{
		instance = this;
		for (int i = 0; i < allAnomalies.allAnomalies.Count; i++)
		{
			allAnomalies.allAnomalies[i].onActivated += OnDizzyActivate;
			allAnomalies.allAnomalies[i].onDisactivated += OnDizzyDisactivate;
			//반복적으로 이거를 더할때마다 어떻게 다르게 만들 방법을 궁리.
		}
	}

	public void OnDizzyActivate(UnitMover effector, MoverChecker inflicter)
	{
		
	}

	public void OnDizzyDisactivate(UnitMover effector, MoverChecker inflicter)
	{

	}
}
