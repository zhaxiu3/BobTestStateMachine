using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Engine
{

	public class BobStateMachineRoot : MonoBehaviour
    {
        public BobAnimatorFSM m_FSM0 = new BobAnimatorFSM();
        void Awake()
        {
            m_FSM0.InitAnimatorFSM(GetComponent<Animator>());
            m_FSM0.OnEnterEventHandler += m_FSM0_OnEnterEventHandler;
            m_FSM0.OnExitEventHandler += m_FSM0_OnExitEventHandler;
                       
        }

        void m_FSM0_OnExitEventHandler(object sender, StateChangeEventArgs e)
        {
            if (e.m_state.m_name == "Base Layer.State3")
            {
                m_FSM0.SetParameter(AnimatorParameter.CreateNewInt("intParam", 0));
            }

        }

        void m_FSM0_OnEnterEventHandler(object sender, StateChangeEventArgs e)
        {
        }
		// Use this for initialization
		void Start () {            
		
		}
		
		// Update is called once per frame
		void Update () {
            m_FSM0.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_12");
                m_FSM0.SendEvent("gotostate2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_23");
                m_FSM0.SendEvent("gotostate3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_31");
                m_FSM0.SendEvent("gotostate1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_31");
                m_FSM0.SendGlobalEvent("gotostate3");
            }
            
		}

	}
}
