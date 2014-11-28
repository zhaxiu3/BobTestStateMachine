using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace Engine
{
    public class BobStateMachineEditorNew : EditorWindow
    {

        [MenuItem("ByBob/OpenStateMachineEditor")]
        static void OpenStateMachineEditor()
        {
            BobStateMachineEditorNew window = EditorWindow.GetWindow<BobStateMachineEditorNew>();
            window.width = new List<int>() { 200, 200, 300 };
        }

        public List<int> width = new List<int>() { 200, 200, 300 };
        Vector2 WindowSize;
        BobStateMachineRoot m_target;

        #region 响应消息
        void OnGUI()
        {
            WindowSize.x = Screen.width;
            WindowSize.y = Screen.height;
            if (m_target == null)
            {
                return;
            }
            if (m_target.m_FSM0 == null)
            {
                return;
            }
            if (m_target.m_FSM0.m_States.Count <= 0)
            {
                m_currentState = null;
                return;
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    drawStateList();
                } EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                {
                    drawEventList();
                } EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                {
                    drawParameterList();
                } EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();

            } EditorGUILayout.EndHorizontal();

        }



        List<string> m_ParameterNames = new List<string>();
        /// <summary>
        /// 更新当前选中的物体
        /// </summary>
        void OnSelectionChange()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                m_target = null;
            }
            else
            {
                m_target = obj.GetComponent<BobStateMachineRoot>();
                m_ParameterNames.Clear();
                if (m_target == null)
                {
                    return;
                }
                if (m_target.m_FSM0 == null)
                {
                    return;
                }
                for (int i = 0; i < m_target.m_FSM0.m_parameters.Count; i++)
                {
                    m_ParameterNames.Add(m_target.m_FSM0.m_parameters[i].Name);
                }
            }
        }
        #endregion

        #region 绘制函数
        Vector2 m_statelistviewScrollPos;
        int m_CurSelStateIndex = 0;
        string newstatename;
        BobAnimatorState m_currentState;
        void drawStateList()
        {
            GUI.Box(new Rect(0, 0, width[0], Screen.height), "");
            m_statelistviewScrollPos = EditorGUILayout.BeginScrollView(m_statelistviewScrollPos, GUILayout.Width(width[0]));
            {
                for (int i = 0; i < m_target.m_FSM0.m_States.Count; i++)
                {
                    if (m_CurSelStateIndex == i)
                    {
                        GUI.backgroundColor = Color.gray;
                    }
                    else
                    {
                        GUI.backgroundColor = Color.white;
                    }
                    BobAnimatorState _currentState = m_target.m_FSM0.m_States[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(_currentState.m_name))
                        {
                            m_CurSelStateIndex = i;
                            m_CurSelEventIndex = 0;
                            m_CurSelParamIndex = 0;
                            m_currentState = _currentState;
                            UpdateParameterList();
                        }
                        if (GUILayout.Button("-", GUILayout.Width(30)))
                        {
                            m_target.m_FSM0.m_States.RemoveAt(i);
                            m_currentState = null;
                            m_CurSelStateIndex = 0;
                            break;
                        }
                    } EditorGUILayout.EndHorizontal();
                }
            } EditorGUILayout.EndScrollView();
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            {
                newstatename = GUILayout.TextField(newstatename, GUILayout.Width(160));
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    m_target.m_FSM0.m_States.AddValue(Animator.StringToHash(newstatename), BobAnimatorState.CreateState(newstatename));
                }
            } EditorGUILayout.EndHorizontal();
        }


        Vector2 m_eventlistviewScrollPos;
        int m_CurSelEventIndex = 0;
        void drawEventList()
        {
            GUI.backgroundColor = Color.gray;
            GUI.Box(new Rect(width[0], 0, width[1], Screen.height), "");
            if (m_currentState == null)
            {
                return;
            }
            if (m_currentState.m_TransitionConditions.Count <= 0)
            {
                m_CurSelEventIndex = -1;                
            }
            else
            {
	            m_eventlistviewScrollPos = EditorGUILayout.BeginScrollView(m_eventlistviewScrollPos, GUILayout.Width(width[1]));
	            {
	                for (int i = 0; i < m_currentState.m_TransitionConditions.Count; i++)
	                {
	                    if (m_CurSelEventIndex == i)
	                    {
	                        GUI.backgroundColor = Color.gray;
	                    }
	                    else
	                    {
	                        GUI.backgroundColor = Color.white;
	                    }
	                    string _currentEvent = m_currentState.m_TransitionConditions.KeyList[i];
	                    EditorGUILayout.BeginHorizontal();
	                    {
	                        if (GUILayout.Button(_currentEvent))
	                        {
	                            m_CurSelEventIndex = i;
	                            m_CurSelParamIndex = 0;
	                            UpdateParameterList();
	                        }
	                        if (GUILayout.Button("-", GUILayout.Width(30)))
	                        {
	                            m_currentState.m_TransitionConditions.RemoveValue(_currentEvent);
	                            m_CurSelEventIndex = 0;
	                            break;
	                        }
	
	                    } EditorGUILayout.EndHorizontal();
	                }
	            } EditorGUILayout.EndScrollView();
            }

            int ________________________________________addfunctionToFinish;
            //if (GUILayout.Button("+"))
            //{
            //    m_currentState.m_TransitionConditions.AddValue("haha", BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>()));
            //    UpdateParameterList();
            //}
        }

        private void UpdateParameterList()
        {
            m_currentParameterList.Clear();
            if(m_currentState == null){
                return;
            }
            if(m_currentState.m_TransitionConditions.Count <= 0){
                return;
            }
            
            for (int i = 0; i < m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.Count; i++)
            {
                m_currentParameterList.Add(0);
            }

            for (int i = 0; i < m_currentParameterList.Count; i++)
            {
                m_currentParameterList[i] = m_ParameterNames.IndexOf(m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters[i].Name);
            }
        }
        List<int> m_currentParameterList = new List<int>();
        Vector2 m_paramlistviewScrollPos;
        int m_CurSelParamIndex = 0;
        void drawParameterList()
        {
            GUI.Box(new Rect(width[0] + width[1], 0, width[2], Screen.height), "");
            if (m_currentState == null)
            {
                return;
            }
            if (m_currentState.m_TransitionConditions.Count <= 0)
            {
                m_CurSelEventIndex = -1;
                return;
            }
            if (m_CurSelEventIndex == -1)
            {
                return;
            }
            if (m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.Count <= 0)
            {
                return;
            }
            if (m_currentParameterList.Count <= 0)
            {
                return;
            }
            else
            {
	            m_paramlistviewScrollPos = EditorGUILayout.BeginScrollView(m_paramlistviewScrollPos, GUILayout.Width(width[2]));
	            {
	                for (int i = 0; i < m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.Count; i++)
	                {
	                    if (m_CurSelParamIndex == i)
	                    {
	                        GUI.backgroundColor = Color.gray;
	                    }
	                    else
	                    {
	                        GUI.backgroundColor = Color.white;
	                    }
	                    AnimatorParameter currentParam = new AnimatorParameter(m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters[i]); //拷贝一个当前值
	                    EditorGUILayout.BeginHorizontal();
	                    {
	                        m_currentParameterList[i] = EditorGUILayout.Popup(m_currentParameterList[i], m_ParameterNames.ToArray());
	                        int _paramIndex = m_currentParameterList[i];
	                        AnimatorParameter _newParam = new AnimatorParameter(m_target.m_FSM0.m_parameters[_paramIndex]);
	
	                        if (currentParam.Name == _newParam.Name)
	                        {
	                            switch (currentParam.ParamType)
	                            {
	                                case AnimatorParameterType.BOOL:
	                                    {
	                                        currentParam.m_boolValue = EditorGUILayout.Toggle(currentParam.m_boolValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.FLOAT:
	                                    {
	                                        currentParam.m_floatValue = EditorGUILayout.FloatField(currentParam.m_floatValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.INT:
	                                    {
	                                        currentParam.m_intValue = EditorGUILayout.IntField(currentParam.m_intValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.TRIGGER:
	                                default:
	                                    {
	                                        break;
	                                    }
	                            }
	                            m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters[i] = currentParam;
	                        }
	                        else
	                        {
	                            switch (_newParam.ParamType)
	                            {
	                                case AnimatorParameterType.BOOL:
	                                    {
	                                        _newParam.m_boolValue = EditorGUILayout.Toggle(_newParam.m_boolValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.FLOAT:
	                                    {
	                                        _newParam.m_floatValue = EditorGUILayout.FloatField(_newParam.m_floatValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.INT:
	                                    {
	                                        _newParam.m_intValue = EditorGUILayout.IntField(_newParam.m_intValue);
	                                        break;
	                                    }
	                                case AnimatorParameterType.TRIGGER:
	                                default:
	                                    {
	                                        break;
	                                    }
	                            }
	                            m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters[i] = _newParam;
	                        }
	                        if (GUILayout.Button("-", GUILayout.Width(30)))
	                        {
	                            m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.RemoveAt(i);
	                            UpdateParameterList();
	                            break;
	                        }
	                    } EditorGUILayout.EndHorizontal();
	
	                }
	
	            } EditorGUILayout.EndScrollView();
            }
            if (GUILayout.Button("+"))
            {
                m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.Add(new AnimatorParameter(m_target.m_FSM0.m_parameters[0]));
                UpdateParameterList();
            }
        }
        #endregion
    }
}
