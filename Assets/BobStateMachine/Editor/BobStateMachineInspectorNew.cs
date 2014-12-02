using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Engine
{
	[CustomEditor(typeof(BobStateMachineRoot))]
	public class BobStateMachineInspectorNew : Editor {

        BobStateMachineRoot m_target;
        void OnEnable()
        {
            m_target = target as BobStateMachineRoot;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Editor"))
                {
                    BobStateMachineEditorNew.OpenStateMachineEditor(m_target);
                }
                if (GUILayout.Button("Reset"))
                {
                    BobStateMachineEditorNew.InitFromAnimator(m_target.GetComponent<Animator>(), ref m_target.m_FSM0);
                }
            } EditorGUILayout.EndHorizontal();
        }
	}
}

