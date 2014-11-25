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
        public void AddTransition(BobState<T> from, BobState<T> to)
        {
            BobTransition<T> _newTrans = new BobTransition<T>();
            _newTrans.m_fromState = from;
            _newTrans.m_toState = to;
        }
        public void ChangeState(BobState<T> newState){
            if(null != m_CurrentState)            
            {            
                m_CurrentState.OnExit(m_Owner);
            }
            newState.OnEnter(m_Owner);
            m_CurrentState = newState;
        }
        public void OnUpdate() 
        {
            if (m_CurrentState != null && m_CurrentState.active)
            {
                m_CurrentState.OnUpdate(m_Owner);
            }
        }
        
	}
}
