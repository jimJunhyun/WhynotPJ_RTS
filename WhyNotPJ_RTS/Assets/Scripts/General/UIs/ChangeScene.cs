using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void Changer(int idx)
    {
        SceneChanger.instance.Change(idx);
    }
    public void Changer(string name)
    {
        SceneChanger.instance.Change(name);
    }
    public void NonAsyncChanger(int idx)
    {
        SceneChanger.instance.NonAsyncChange(idx);
    }
    public void NonAsyncChanger(string name)
    {
        SceneChanger.instance.NonAsyncChange(name);
    }
}
