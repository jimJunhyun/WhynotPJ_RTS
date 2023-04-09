using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPosGen : MonoBehaviour
{
	public static EnemyPosGen instance;

	public EnemySettings set;
	public Transform basePos;
	public float calcApt;
	public float calcRad;

	int curAccumulated = 0;

	public List<IUnit> myControls = new List<IUnit>();
	public List<IUnit> accumulations = new List<IUnit>();
	public List<IBuilding> buildings = new List<IBuilding>();

	Vector2Int pos;

	private void Awake()
	{
		instance = this;
	}

	public void SamplePos(IUnit con)
	{
		float selected = Random.Range(0, set.passiveBias + set.warBias);
		
		if (con.element.rec >= 5f)
		{
			//�����迭 ����
			//���� ����� �̹߰� �������� �̵��ϵ��� �ϱ�..
			//�̹߰� ������ �� ���� �������� �̵��ϵ���?
			//�켱����
			con.Move(Vector3.zero);
			Debug.Log("�������� ����");
		}
		else if(set.warBias >= selected)
		{
				con.state = UnitState.Alert;
				curAccumulated += 1;
				accumulations.Add(con);
				myControls.Remove(con);
				Debug.Log("���� �ϳ� ����.");
			
			Debug.Log(con.myName + " ���� �������� ���.");
			//�߰��� �� ������ �����鼭, ����� ���� ������ �����Ͽ��ٸ� ������ ���� ���ο��� ���� ���
			//�߰��� �� ������ ���ٸ� ��� ���·� ������.
			//��Ҵٰ� ����
		}
		else
		{
			Vector3 v = Perceive.IdxVectorToPos(FindHighestHeightIdx());
			con.Move(v); 

			con.state = UnitState.Alert;//�Ϸ�� ���� ���� ����. (�Ϸ�� �ൿ�� �����ϱ�, �Ǵ� ������ �ڷ�ƾ���� �����Ͽ� ��ȯ���� ��ٸ��� �ϱ�??)
			Debug.Log(con.myName + " ���� ������� ���");

			//������ ����� ������ ���� ��.
			//�Ÿ����� �ݺ��, ���̿��� ���
			
		}
	}

	public Vector2Int FindHighestHeightIdx() //�����鼭 ������� ��ȣ��.
	{
		float largestH = float.MinValue;
		/*Vector2Int v*/pos = Perceive.PosToIdxVector(EnemyBrain.instance.transform.position);
		for (int y = 0; y < Perceive.MAPY; y++)
		{
			for (int x = 0; x < Perceive.MAPX; x++)
			{
				if(EnemyEye.instance.perceived.map[y, x].visiblity) 
				{
					float num = EnemyEye.instance.perceived.map[y, x].height * (set.heightBias / (set.distBias + 1)) - MapData.GetDist(Perceive.IdxVectorToPos(new Vector2Int(y, x)), EnemyBrain.instance.transform.position) * (set.distBias / (set.heightBias + 1));
					//���̰� ���꿡 �ȵ��µ�?
					Debug.Log($"{x} ,  {y} : {num}");
					if (num > largestH) 
					{
						pos.x = y;
						pos.y = x;
						largestH = num;
					}
					
				}
			}
		}
		return pos;
	}

	public void WholeAttack()
	{
		for (int i = 0; i < accumulations.Count; i++)
		{
			accumulations[i].Move(EnemyBrain.instance.playerBase);
		}
		Debug.Log("�Ѱ���");
	}

	public void FindPlaying()
	{
		if (EnemyBrain.instance.playerBase != null && curAccumulated >= accumulations.Count)
		{
			WholeAttack();
		}
		else if (myControls.Count > 0)
		{
			IUnit curC = myControls.Find((x) => { return x.state == UnitState.Wait; });
			if (curC != null)
			{
				SamplePos(curC);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(Perceive.IdxVectorToPos(pos), 1);
	}
}
