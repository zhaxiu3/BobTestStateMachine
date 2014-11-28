using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Engine
{
  
	[System.Serializable]
	public class BobAnimatorFSM : BobFSM<Animator>
    {
        /// <summary>
        /// 对应的Animator的layer
        /// </summary>
        public int m_AnimatorLayer = 0;
        public BobHashStateMap m_States = new BobHashStateMap();
        public List<AnimatorParameter> m_parameters = new List<AnimatorParameter>();
        public BobAnimatorState m_AnyState = new BobAnimatorState() { m_name = "AnyState"};
        public void AddTransition(BobAnimatorState from, BobAnimatorState to, string uniquename, int uniquehash, BobTransitionCondition parameters)
        {
            base.AddTransition(from, to, uniquename, uniquehash);
            from.m_TransitionConditions.AddValue(uniquename, parameters);
        }
        public void AddGlobalTransition(BobAnimatorState to, string uniquename, int uniquehash, BobTransitionCondition parameters)
        {
            base.AddGlobalTransition(to, uniquename, uniquehash);
            m_AnyState.m_TransitionConditions.AddValue(uniquename, parameters);
        }

        public override bool SendEvent(string transname)
        {
            if (!IsEventAvailable(transname))
            {
                return false;
            }
            BobTransitionCondition _params = ((BobAnimatorState)this.m_CurrentState).m_TransitionConditions.getValue(transname);
            if (_params == null)
            {
                return false;
            }
            float _ExitTime = _params.m_ExitTime;
            for (int i = 0; i < _params.m_parameters.Count; i++)
            {
                SetParameter(_params.m_parameters[i]);
            }
            return true;
        }
        public override bool SendGlobalEvent(string transname)
        {
            BobTransitionCondition _params = ((BobAnimatorState)this.m_AnyState).m_TransitionConditions.getValue(transname);
            if (_params == null)
            {
                return false;
            }
            float _ExitTime = _params.m_ExitTime;
            for (int i = 0; i < _params.m_parameters.Count; i++)
            {
                SetParameter(_params.m_parameters[i]);
            }
            return true;
        }

        private void SetParameter(AnimatorParameter ap)
        {
            switch (ap.ParamType)
            {
                case AnimatorParameterType.BOOL:
                    {
                        m_Owner.SetBool(ap.Name, (bool)ap.Value);
                        break;
                    }
                case AnimatorParameterType.FLOAT:
                    {
                        m_Owner.SetFloat(ap.Name, (float)ap.Value);
                        break;
                    }
                case AnimatorParameterType.INT:
                    {
                        m_Owner.SetInteger(ap.Name, (int)ap.Value);
                        break;
                    }
                case AnimatorParameterType.TRIGGER:
                    {
                        m_Owner.SetTrigger(ap.Name);
                        break;
                    }
            }
        }

        int m_currentStateHash;
        public override void OnUpdate()
        {
            int newStateHash = m_Owner.GetNextAnimatorStateInfo(m_AnimatorLayer).nameHash;
            if (newStateHash == 0)
            {
                return;
            }
            if (m_currentStateHash != newStateHash)
            {
                m_currentStateHash = newStateHash;
                ChangeState(m_States.getValue(newStateHash), string.Empty);
            }
            base.OnUpdate();
        }
    }
    #region ParameterTypeDefine
    public enum AnimatorParameterType
    {
        BOOL = 0,
        FLOAT,
        INT,
        TRIGGER
    }
    [System.Serializable]
    public class AnimatorParameter
    {
        public string Name;
        public AnimatorParameterType ParamType;
        public bool m_boolValue;
        public float m_floatValue;
        public int m_intValue;
        public object Value
        {
            get
            {
                switch (ParamType)
                {
                    case AnimatorParameterType.BOOL:
                        {
                            return m_boolValue;
                        }
                    case AnimatorParameterType.FLOAT:
                        {
                            return m_floatValue;
                        }
                    case AnimatorParameterType.INT:
                        {
                            return m_intValue;
                        }
                    case AnimatorParameterType.TRIGGER:
                        {
                            return "Trigger";
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            set
            {
                switch (ParamType)
                {
                    case AnimatorParameterType.BOOL:
                        {
                            m_boolValue = System.Convert.ToBoolean(value);
                            break;
                        }
                    case AnimatorParameterType.FLOAT:
                        {
                            m_floatValue = System.Convert.ToSingle(value);
                            break;
                        }
                    case AnimatorParameterType.INT:
                        {
                            m_intValue = System.Convert.ToInt32(value);
                            break;
                        }
                    case AnimatorParameterType.TRIGGER:
                    default:
                        {
                            break;
                        }
                }
            }
        }
        private AnimatorParameter(string _name, AnimatorParameterType _type, object _value)
        {
            Name = _name;
            ParamType = _type;
            Value = _value;
        }
        public AnimatorParameter(AnimatorParameter src)
        {
            Name = src.Name;
            ParamType = src.ParamType;
            Value = src.Value;
        }
        #region 工厂方法
        public static AnimatorParameter CreateNewTrigger(string name) {
            return new AnimatorParameter(name, AnimatorParameterType.TRIGGER, null);
        }
        public static AnimatorParameter CreateNewBool(string name, bool bvalue) {
            return new AnimatorParameter(name, AnimatorParameterType.BOOL, bvalue);
        }
        public static AnimatorParameter CreateNewInt(string name, int ivalue) {
            return new AnimatorParameter(name, AnimatorParameterType.INT, ivalue);
        }
        public static AnimatorParameter CreateNewFloat(string name, float fvalue)
        {
            return new AnimatorParameter(name, AnimatorParameterType.FLOAT, fvalue);
        }
        #endregion
    }

    #endregion

    /// <summary>
    /// 用于设置发送事件时需要改变的参数
    /// </summary>
    [System.Serializable]
    public class BobTransitionCondition
    {
        public List<AnimatorParameter> m_parameters = new List<AnimatorParameter>();
        public float m_ExitTime = 0;
        public static BobTransitionCondition CreateTransitionCondition(List<AnimatorParameter> parameters, float exittime = 0)
        {
            BobTransitionCondition conditions = new BobTransitionCondition();
            conditions.m_parameters.AddRange(parameters.ToArray());
            conditions.m_ExitTime = exittime;
            return conditions;
        }
    }
    [System.Serializable]
    public class BobTransitionConditionMap : BobDictionary<string, BobTransitionCondition> { }

    /// <summary>
    /// 将transition的hashId和event的名称对应起来
    /// </summary>
    [System.Serializable]
    public class BobHashStateMap : BobDictionary<int, BobAnimatorState> { }
    
}

