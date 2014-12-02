using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

namespace Engine.Test
{
    /// <summary>
    /// 现在的TransitionName无法在添加之后更改
    /// 文档中应写上：在AnimatorController中添加上事件名
    /// 这样之后的工作就是：设置事件中parameter的值
    /// </summary>
    public class BobStateMachineEditorNew : EditorWindow
    {
        public static void OpenStateMachineEditor(BobStateMachineRoot target)
        {
            BobStateMachineEditorNew window = EditorWindow.GetWindow<BobStateMachineEditorNew>();
            //window.m_titleWidthList = new List<int>() { 200, 200, 300 };
            window.SetTarget(target);
        }

        public List<int> m_titleWidthList = new List<int>() { 300, 200, 300 };
        /// <summary>
        /// 编辑对象
        /// </summary>
        BobStateMachineRoot m_target;

        #region 公共方法
        public void SetTarget(BobStateMachineRoot target)
        {
            m_target = target;
            if (m_target == null)
            {
                return;
            }
            UpdateParameterNames();
            UpdateParameterList();
        }
        public void UpdateParameterNames()
        {
            m_ParameterNames.Clear();
            for (int i = 0; i < m_target.m_FSM0.m_parameters.Count; i++)
            {
                m_ParameterNames.Add(m_target.m_FSM0.m_parameters[i].Name);
            }
        }
        #endregion

        #region 响应消息
        void OnGUI()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null)
            {
                SetTarget(null);
            }
            else if (m_target != obj.GetComponent<BobStateMachineRoot>())
            {
                OnSelectionChange();
            }
            
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
                SetTarget(null);
            }
            else if (m_target != obj.GetComponent<BobStateMachineRoot>())
            {
                SetTarget(obj.GetComponent<BobStateMachineRoot>());
                if (m_target == null)
                {
                    return;
                }
                if (m_target.m_FSM0 == null)
                {
                    return;
                }
            }

        }

        #endregion

        #region static方法
        public static void InitFromAnimator(Animator animator, ref BobAnimatorFSM fsm)
        {
            AnimatorController _ac = AnimatorController.GetEffectiveAnimatorController(animator);
            AnimatorControllerLayer _aclyer =  _ac.GetLayer(fsm.m_AnimatorLayer);
            List<State> _states = GetAllSates(_aclyer.stateMachine);
            fsm.m_States.Clear();
            fsm.m_parameters.Clear();
            
            fsm.m_parameters = GetAllParameters(_ac);
            for (int i = 0; i < _states.Count; i++)
            {
                State _curState = _states[i];
                fsm.m_States.AddValue(_curState.uniqueNameHash, BobAnimatorState.CreateState(_curState.uniqueName));
                Transition[] _transitions = _aclyer.stateMachine.GetTransitionsFromState(_curState);
                for (int j = 0; j < _transitions.Length; j++)
                {
                    fsm.m_States[i].m_TransitionConditions.AddValue(_transitions[j].name, BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>()));
                    int _conditionCount = _transitions[j].conditionCount;
                    for (int k = 0; k < _conditionCount; k++)
                    {
                        AnimatorCondition _condition = _transitions[j].GetCondition(k);
                        if (_condition.mode == TransitionConditionMode.ExitTime)
                        {
                            //不管ExitTime
                            continue;
                        }
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
        #endregion
        #region 绘制函数
        Vector2 m_statelistviewScrollPos;
        int m_CurSelStateIndex = 0;
        string m_newstatename = "";
        BobAnimatorState m_currentState;
        /// <summary>
        /// 是否选中了AnyState
        /// </summary>
        bool m_bIsSelAnyState = false;
        void drawStateList()
        {
            GUI.Box(new Rect(0, 0, m_titleWidthList[0], Screen.height), "");
            m_statelistviewScrollPos = EditorGUILayout.BeginScrollView(m_statelistviewScrollPos, GUILayout.Width(m_titleWidthList[0]));
            {
                if (GUILayout.Button(m_target.m_FSM0.m_AnyState.m_name))
                {
                    m_bIsSelAnyState = true;
                    m_CurSelEventIndex = 0;
                    m_CurSelParamIndex = 0;
                    m_currentState = m_target.m_FSM0.m_AnyState;
                    UpdateParameterList();
                }
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
                        EditorGUILayout.LabelField("IsDefault",GUILayout.Width(70));
                        _currentState.active = EditorGUILayout.Toggle(_currentState.active,GUILayout.Width(20));
                        if (_currentState.active == true)
                        {
                            for (int j = 0; j < m_target.m_FSM0.m_States.Count; j++)
                            {
                                if (_currentState != m_target.m_FSM0.m_States[j])
                                {
                                    m_target.m_FSM0.m_States[j].active = false;
                                }
                            }
                        }
                        if (_currentState.active == true)
                        {
                            m_target.m_FSM0.m_CurrentState = _currentState;
                        }
                        if (GUILayout.Button(_currentState.m_name, GUILayout.Width(140)))
                        {
                            m_bIsSelAnyState = false;
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
        string m_currentEventName = "";
        void drawEventList()
        {
            GUI.backgroundColor = Color.gray;
            GUI.Box(new Rect(m_titleWidthList[0], 0, m_titleWidthList[1], Screen.height), "");
            if (m_currentState == null)
            {
                return;
            }
            if (m_CurSelEventIndex >= m_currentState.m_TransitionConditions.Count)
            {
                m_CurSelEventIndex = 0;
                return;
            }
            if (m_CurSelEventIndex >= 0 && m_CurSelEventIndex < m_currentState.m_TransitionConditions.Count )
            {
                m_currentState.m_TransitionConditions.KeyList[m_CurSelEventIndex] = EditorGUILayout.TextField(m_currentState.m_TransitionConditions.KeyList[m_CurSelEventIndex]);
            }
            m_eventlistviewScrollPos = EditorGUILayout.BeginScrollView(m_eventlistviewScrollPos, GUILayout.Width(m_titleWidthList[1]));
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
                            UpdateParameterList();
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
            GUI.Box(new Rect(m_titleWidthList[0] + m_titleWidthList[1], 0, m_titleWidthList[2], Screen.height), "");
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

            m_paramlistviewScrollPos = EditorGUILayout.BeginScrollView(m_paramlistviewScrollPos, GUILayout.Width(m_titleWidthList[2]));
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
