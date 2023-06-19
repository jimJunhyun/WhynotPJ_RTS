using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage", menuName = "Set Stage")]
public class SetStage : ScriptableObject
{
    public string stageName;
    public string sceneName;
    public Sprite image;
    public string additionalInfo;
}
