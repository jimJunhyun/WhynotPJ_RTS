using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Producer))]
public class ProducerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("������ �����ϴ�.") && Application.isPlaying)
		{
			(target as Producer).AddProduct((target as Producer).testUnit);
		}
	}
}
