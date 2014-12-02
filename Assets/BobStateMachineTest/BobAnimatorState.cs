using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Test
{
    [System.Serializable]
	public class BobAnimatorState : BobState
    {

        public BobTransitionConditionMap m_TransitionConditions = new BobTransitionConditionMap();

        private float m_Timer = 0;
        public override void OnEnter(object owner,string transname)
        {
            base.OnEnter(owner, transname);
            m_Timer = 0;
        }

        public override void OnExit(object owner, string transname)
        {
            base.OnExit(owner, transname);
            m_Timer = 0;
        }

        public override void OnUpdate(object owner)
        {
            m_Timer += Time.deltaTime;
        }

        public static BobAnimatorState CreateState(string name)
        {
            BobAnimatorState _astate = new BobAnimatorState();
            _astate.m_name = name;
            return _astate;
        }
	}
}
