using UnityEngine;
using System.Collections;

namespace Engine
{
    [System.Serializable]
	public class BobAnimatorState : BobState<Animator>
    {
        private float m_Timer = 0;
        public override void OnEnter(Animator owner)
        {
            base.OnEnter(owner);
            m_Timer = 0;
        }

        public override void OnExit(Animator owner)
        {
            base.OnExit(owner);
            m_Timer = 0;
        }

        public override void OnUpdate(Animator owner)
        {
            m_Timer += Time.deltaTime;
        }
	}
}
