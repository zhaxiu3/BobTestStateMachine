using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Engine.Test
{

	public class BobStateMachineRoot : MonoBehaviour
    {
        public BobAnimatorFSM m_FSM0 = new BobAnimatorFSM();
        void Awake()
        {
            m_FSM0.InitAnimatorFSM(GetComponent<Animator>());
                       
        }
		
		// Update is called once per frame
		void Update () {
            m_FSM0.OnUpdate();            
		}
        public bool SendEvent(string transname)
        {
            return m_FSM0.SendEvent(transname);
        }
        public bool SendGlobalEvent(string transname)
        {
            return m_FSM0.SendGlobalEvent(transname);
        }

	}
}
