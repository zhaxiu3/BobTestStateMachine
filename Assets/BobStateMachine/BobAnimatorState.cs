using UnityEngine;
using System.Collections;

namespace Engine
{
	public class BobAnimatorState : BobState<MonoBehaviour>
    {
        private float m_Timer = 0;
        public override void OnEnter(MonoBehaviour owner)
        {
            base.OnEnter(owner);
            m_Timer = 0;
        }

        public override void OnExit(MonoBehaviour owner)
        {
            base.OnExit(owner);
        }

        public override void OnUpdate(MonoBehaviour owner)
        {
            m_Timer += Time.deltaTime;
        }
	}
}
