using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Engine
{
	public class BobFSM<T>
	{
	    /// <summary>
	    /// 状态机的所有者
	    /// </summary>
	    protected T m_Owner;
        public BobState<T> m_CurrentState;
        private List<BobTransition<T>> m_Transitions = new List<BobTransition<T>>();

        #region 处理Global事件
        public BobState<T> m_GlobalState;
        private List<BobTransition<T>> m_GlobalTransitions = new List<BobTransition<T>>();
        #endregion

        public void Init(BobState<T> InitialState, T owner)
        {
            setOwner(owner);
            Init(InitialState);
        }
        public void Init(BobState<T> InitialState)
        {
            ChangeState(InitialState, string.Empty);
        }

        public void setOwner(T owner)
        {
            m_Owner = owner;
        }
        /// <summary>
        /// 添加新的Transition
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="uniquename"></param>
        /// <param name="uniquehash"></param>
        public virtual void AddTransition(BobState<T> from, BobState<T> to, string uniquename, int uniquehash)
        {
            BobTransition<T> _newTrans = new BobTransition<T>();
            _newTrans.m_fromState = from;
            _newTrans.m_toState = to;
            _newTrans.m_uniqueName = uniquename;
            _newTrans.m_uniqueHash = uniquehash;
            m_Transitions.Add(_newTrans);
        }
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="transname">事件名称</param>
        /// <returns></returns>
        public virtual bool SendEvent(string transname){
            if (!IsEventAvailable(transname))
            {
                return false;
            }
            DoSendEvent(transname);
            return true;
        }
        protected void DoSendEvent(string transname)
        {
            int index = IndexOf(transname);
            BobTransition<T> _trans = m_Transitions[index];
            ChangeState(_trans.m_toState,_trans.m_uniqueName);
        }

        public virtual void AddGlobalTransition(BobState<T> to, string uniquename, int uniquehash)
        {
            BobTransition<T> _newTrans = new BobTransition<T>();
            _newTrans.m_fromState = null;
            _newTrans.m_toState = to;
            _newTrans.m_uniqueName = uniquename;
            _newTrans.m_uniqueHash = uniquehash;
            m_GlobalTransitions.Add(_newTrans);
        }
        public virtual bool SendGlobalEvent(string transname)
        {
            DoSendEvent(transname);
            return true;
        }
        protected void DoSendGlobalEvent(string transname)
        {
            int index = IndexOf(transname);
            BobTransition<T> _trans = m_GlobalTransitions[index];
            ChangeState(_trans.m_toState, _trans.m_uniqueName);
        }

        protected bool IsEventAvailable(string transname)
        {
            int index = IndexOf(transname);
            if (index == -1)
            {
                return false;
            }
            BobTransition<T> _trans = m_Transitions[index];

            if (_trans.isGlobal)
            {//若是全局事件，则不判断当前状态
                return true;
            }
            if (_trans.m_fromState != m_CurrentState)
            {
                return false;
            }
            return true;
        }
        public void ChangeState(BobState<T> newState, string transname){
            if(null != m_CurrentState)            
            {            
                m_CurrentState.OnExit(m_Owner, transname);
            }
            if (newState == null)
            {
                Debug.LogError("what i am null!!!!!");
            }
            newState.OnEnter(m_Owner, transname);
            m_CurrentState = newState;
        }
        public virtual void OnUpdate() 
        {
            if (m_CurrentState != null && m_CurrentState.active)
            {
                m_CurrentState.OnUpdate(m_Owner);
            }
            else
            {
                Debug.LogError("Error: FSM has no currentState or currentState is not active");
            }
        }

        #region private
        private int IndexOf(string transname)
        {
            int index = -1;
            for (int i = 0; i < m_Transitions.Count; i++)
            {
                if (m_Transitions[i].m_uniqueName == transname)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private int IndexOf(int transhash)
        {
            int index = -1;
            for (int i = 0; i < m_Transitions.Count; i++)
            {
                if (m_Transitions[i].m_uniqueHash == transhash)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion

    }
}
