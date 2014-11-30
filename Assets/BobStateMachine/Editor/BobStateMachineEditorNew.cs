using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

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
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                m_target = null;
            }
            else if (m_target != obj.GetComponent<BobStateMachineRoot>())
            {
                OnSelectionChange();
            }

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
            Repaint();
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                m_target = null;
            }
            else if (m_target != obj.GetComponent<BobStateMachineRoot>())
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
                InitFromAnimator(obj.GetComponent<Animator>(), ref m_target.m_FSM0);
                for (int i = 0; i < m_target.m_FSM0.m_parameters.Count; i++)
                {
                    m_ParameterNames.Add(m_target.m_FSM0.m_parameters[i].Name);
                }
            }

        }
        #endregion
        public void InitFromAnimator(Animator animator, ref BobAnimatorFSM fsm)
        {
            AnimatorController _ac = AnimatorController.GetEffectiveAnimatorController(animator);
            AnimatorControllerLayer _aclyer =  _ac.GetLayer(fsm.m_AnimatorLayer);
            List<State> _states = GetAllSates(_aclyer.stateMachine);
            fsm.m_States.Clear();
            fsm.m_parameters.Clear();
            
            fsm.m_parameters = GetAllParameters(_ac);
            for (int i = 0; i < _states.Count; i++)
            {
                fsm.m_States.AddValue(_states[i].uniqueNameHash, BobAnimatorState.CreateState(_states[i].uniqueName));
                Transition[] _transitions = _aclyer.stateMachine.GetTransitionsFromState(_states[i]);
                for (int j = 0; j < _transitions.Length; j++)
                {
                    fsm.m_States[i].m_TransitionConditions.AddValue(_transitions[j].uniqueName, BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>()));
                    int _conditionCount = _transitions[j].conditionCount;
                    for (int k = 0; k < _conditionCount; k++)
                    {
                        AnimatorCondition _condition = _transitions[j].GetCondition(k);
                        fsm.m_States[i].m_TransitionConditions[j].m_parameters.Add(fsm.m_parameters.getValue(_condition.parameter));
                    }
                }
            }

        }

        public static List<State> GetAllSates(StateMachine fsm)
        {
            List<State> ret = new List<State>();
            GetStateFromStateMachine(ref ret, fsm);
            return ret;
        }
        private static void GetStateFromStateMachine(ref List<State> results, StateMachine fsm)
        {
            for (int i = 0; i < fsm.stateCount; i++)
            {
                results.Add(fsm.GetState(i));
            }
            for (int i = 0; i < fsm.stateMachineCount; i++)
            {
                GetStateFromStateMachine(ref results, fsm.GetStateMachine(i));
            }
        }

        private static BobNameParameterMap GetAllParameters(AnimatorController animator)
        {
            BobNameParameterMap _parameters = new BobNameParameterMap();
            for (int i = 0; i < animator.parameterCount; i++)
            {
                AnimatorControllerParameter _acp = animator.GetParameter(i);
                switch (_acp.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        {
                            _parameters.AddValue(_acp.name,AnimatorParameter.CreateNewBool(_acp.name, true));
                            break;
                        }
                    case AnimatorControllerParameterType.Float:
                        {
                            _parameters.AddValue(_acp.name,AnimatorParameter.CreateNewFloat(_acp.name, 0f));
                            break;
                        }
                    case AnimatorControllerParameterType.Int:
                        {
                            _parameters.AddValue(_acp.name,AnimatorParameter.CreateNewInt(_acp.name, 0));
                            break;
                        }
                    case AnimatorControllerParameterType.Trigger:
                    default:
                        {
                            _parameters.AddValue(_acp.name,AnimatorParameter.CreateNewTrigger(_acp.name));
                            break;
                        }
                }
            }
            return _parameters;
        }

        #region 绘制函数
        Vector2 m_statelistviewScrollPos;
        int m_CurSelStateIndex = 0;
        string m_newstatename = "";
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
                        if (GUILayout.Button(_currentState.m_name, GUILayout.Width(140)))
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
                m_newstatename = GUILayout.TextField(m_newstatename, GUILayout.Width(160));
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    m_target.m_FSM0.m_States.AddValue(Animator.StringToHash(m_newstatename), BobAnimatorState.CreateState(m_newstatename));
                }
            } EditorGUILayout.EndHorizontal();
        }


        Vector2 m_eventlistviewScrollPos;
        int m_CurSelEventIndex = 0;
        string m_newEventName = "";
        void drawEventList()
        {
            GUI.backgroundColor = Color.gray;
            GUI.Box(new Rect(width[0], 0, width[1], Screen.height), "");
            if (m_currentState == null)
            {
                return;
            }

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
                        if (GUILayout.Button(_currentEvent,GUILayout.Width(150)))
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

            int ________________________________________addfunctionToFinish;
            EditorGUILayout.BeginHorizontal();
            {
                m_newEventName = EditorGUILayout.TextField(m_newEventName, GUILayout.Width(160));
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    m_currentState.m_TransitionConditions.AddValue(m_newEventName, BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>()));
                    //UpdateParameterList();
                }
            } EditorGUILayout.EndHorizontal();
        }

        private void UpdateParameterList()
        {
            m_currentParameterList.Clear();
            if (m_currentState == null)
            {
                return;
            }
            if (m_currentState.m_TransitionConditions.Count <= 0)
            {
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
            if (GUILayout.Button("+"))
            {
                if (m_target.m_FSM0.m_parameters.Count <= 0)
                {
                    ShowNotification(new GUIContent("Please add parameters first"));
                    return;
                }
                m_currentState.m_TransitionConditions[m_CurSelEventIndex].m_parameters.Add(new AnimatorParameter(m_target.m_FSM0.m_parameters[0]));
                UpdateParameterList();
            }

            
        }
        #endregion
    }
}
