using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBreak : MonoBehaviour, IProducable
{
    protected List<Renderer> children = new List<Renderer>();
    protected float angleRad;
    protected Material mat;

    protected float length = 0;
    public const float UNITHEIGHT = 1.5f;

    protected int Hp = 100;
    // 임시. 나중에 체력있는 오브젝트 관련 인터페이스든 뭐든 상속해서 구현 필요.
    //없음말고

    int sights = 0;

    public bool isBroken = false;

    //아래 인터페이스 구현부는 수정 요망
    public string myName { get; }
    public float produceTime { get; }
    public Element element { get; }
    public Action onCompleted { get; }
    public GameObject prefab { get; }

    public bool isPlayer { get; set; }
    public float healthPoint { get; }

	//여기까지 인터페이스

	public virtual void Awake()
    {
        GetComponentsInChildren<Renderer>(children);

        mat = new Material(children[0].material);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].material = mat;
        }
    }

    public virtual void Update()
    {
        CheckDest();

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

        length = dir.magnitude;
        angleRad = Mathf.Atan2(dir.x, dir.z);

        StartCoroutine(DelayRay(id, affectHeight));
    }

    public void CheckDest()
    {
        if (Hp <= 0 && !isBroken)
        {
            isBroken = true;
            if (sights > 0)
            {
                CheckVis();
            }
        }
    }

    public bool CheckVis() //발견되기 전까진 남아있음.
    {
        if (isBroken)
        {
            int myId = 0;
            for (int y = 0; y < Perceive.MAPY; y++)
            {
                for (int x = 0; x < Perceive.MAPX; x++)
                {
                    if ((myId != 0 && myId == Perceive.fullMap[y, x, 1].Id) || (Perceive.fullMap[y, x, 1].Id != 0 && ConstructBuild.instance.strtIdPair[Perceive.fullMap[y, x, 1].Id] == this))
                    {
                        if (myId == 0)
                            myId = Perceive.fullMap[y, x, 1].Id;
                        Perceive.fullMap[y, x, 1].Id = 0;
                    }
                }
            }
            ConstructBuild.instance.strtIdPair.Remove(myId);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    IEnumerator DelayRay(int id, bool affectHeight)
    {
        yield return null;
        Vector3Int idx = Perceive.PosToIdxVector(transform.position);

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

                Physics.SphereCast(rayPos, 0.1f, Vector3.down, out hit, 200f, Perceive.CONSTRUCTMASK | Perceive.GROUNDMASK);
                //Debug.Log(isRemoving);
                if (hit.collider.transform.parent == transform)
                {
                    if (affectHeight)
                        Perceive.fullMap[v.y, v.x, 1].height = (int)hit.point.y;
                    else
                        Perceive.fullMap[v.y, v.x, 1].height = Perceive.fullMap[v.y, v.x, 0].height;
                    Perceive.fullMap[v.y, v.x, 1].Id = id;
                }
            }
        }
    }
}
