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
        public BobDictionary<string, List<AnimatorParameter>> m_TransitionParameters = new BobDictionary<string, List<AnimatorParameter>>();
        /// <summary>
        /// 状态的ExitTime
        /// </summary>
        public BobDictionary<string, float> m_TransitionExitTime = new BobDictionary<string, float>();
	    public override bool SendEvent(string transname)
        {
            List<AnimatorParameter> _params = m_TransitionParameters.getValue(transname);
            float _ExitTime = m_TransitionExitTime.getValue(transname);

            if (!IsEventAvailable(transname))
            {
                return false;
            }
            for (int i = 0; i < _params.Count; i++)
            {
                SetParameter(_params[i]);
            }
            if (_ExitTime == 0)
            {                
                DoSendEvent(transname);
            }
            else
            {
                m_bIsWaitingForExitTime = true;
                m_currentStateInfo = m_Owner.GetCurrentAnimatorStateInfo(m_AnimatorLayer);
                m_waitedEvent = transname;
            }
            return true;
	    }
        public void AddTransition(BobAnimatorState from, BobAnimatorState to, string uniquename, int uniquehash, List<AnimatorParameter> parameters,float time = 0)
        {
            AddTransition(from, to, uniquename, uniquehash);
            m_TransitionParameters.AddValue(uniquename, parameters);
            m_TransitionExitTime.AddValue(uniquename, time);
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

        bool m_bIsWaitingForExitTime = false;
        AnimatorStateInfo m_currentStateInfo;
        private string m_waitedEvent;
        public override void OnUpdate()
        {
            if (m_bIsWaitingForExitTime)
            {
                if (m_currentStateInfo.nameHash != m_Owner.GetCurrentAnimatorStateInfo(m_AnimatorLayer).nameHash)
                {
                    DoSendEvent(m_waitedEvent);
                    m_bIsWaitingForExitTime = false;
                }
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
        public AnimatorParameter(string _name, AnimatorParameterType _type, object _value)
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
    }

    #endregion
}

