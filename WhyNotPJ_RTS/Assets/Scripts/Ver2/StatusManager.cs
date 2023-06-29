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
			//�ݺ������� �̰Ÿ� ���Ҷ����� ��� �ٸ��� ���� ����� �ø�.
		}
	}

	public void OnDizzyActivate(UnitMover effector, MoverChecker inflicter)
	{
		
	}

	public void OnDizzyDisactivate(UnitMover effector, MoverChecker inflicter)
	{

	}
}
