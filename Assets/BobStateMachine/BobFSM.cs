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
        private BobState<T> m_CurrentState;
        private BobState<T> m_GlobalState;
        private List<BobTransition<T>> m_Transitions = new List<BobTransition<T>>();

        public void Init(BobState<T> InitialState, T owner)
        {
            m_Owner = owner;
            ChangeState(InitialState);
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
            ChangeState(_trans.m_toState);
        }

        protected bool IsEventAvailable(string transname)
        {
            int index = IndexOf(transname);
            if (index == -1)
            {
                return false;
            }
            BobTransition<T> _trans = m_Transitions[index];
            if (_trans.m_fromState != m_CurrentState)
            {
                return false;
            }
            return true;
        }
        public void ChangeState(BobState<T> newState){
            if(null != m_CurrentState)            
            {            
                m_CurrentState.OnExit(m_Owner);
            }
            newState.OnEnter(m_Owner);
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
