using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBreak : MonoBehaviour
{
	protected List<Renderer> children = new List<Renderer>(); 
	protected Vector3 pos;
	protected float angleRad;
	protected Material mat;

	protected float length = 0;
	public const float UNITHEIGHT = 1.5f;

	int sights = 0;

	public virtual void Awake()
	{
		GetComponentsInChildren<Renderer>(children);
	}

	public void See(bool seeing)
	{
		int val = 0;
		if (seeing)
			val = 1;
		else
			val = -1;
		if ((sights <= 0 && sights + val > 0) || (sights > 0 && sights + val <= 0))
		{
			sights += val;
			mat.SetInt("_Conceal", sights <= 0 ? 1 : 0);
		}

	}

	public virtual void Gen(Vector3 startPos, Vector3 endPos, bool affectHeight, int id)
	{
		Vector3 dir = endPos - startPos;
		pos = (startPos + endPos) / 2;
		
		length = dir.magnitude;
		angleRad = Mathf.Atan2(dir.x, dir.z);

		StartCoroutine(DelayRay(id, affectHeight));
	}

	public IEnumerator DelayRay(int id, bool affectHeight)
	{
		yield return null;
		Vector3Int idx = Perceive.PosToIdxVector(pos);

		for (int y = -(int)(length / 2); y < (int)(length / 2); y++)
		{
			for (int x = -5; x < 5; x++)
			{
				Vector3Int v = new Vector3Int(x, y, 0);
				v = new Vector3Int(
				(int)(v.x * Mathf.Sin(angleRad) + v.y * Mathf.Cos(angleRad) + idx.x),
				(int)(v.x * Mathf.Cos(angleRad) - v.y * Mathf.Sin(angleRad) + idx.y),
				0);

				Vector3 rayPos = Perceive.IdxVectorToPos(v);

				rayPos.y = 100f;
				RaycastHit hit;

				Physics.Raycast(rayPos, Vector3.down, out hit, 200f, Perceive.BRIDGEMASK | Perceive.GROUNDMASK);

				if (hit.collider.transform.parent == transform)
				{
					Debug.DrawLine(rayPos, hit.point, Color.cyan, 1000f);
					if(affectHeight)
						Perceive.fullMap[v.y, v.x, 1].height = (int)hit.point.y;
					Perceive.fullMap[v.y, v.x, 1].Id = id;
				}

			}
		}
	}
}
