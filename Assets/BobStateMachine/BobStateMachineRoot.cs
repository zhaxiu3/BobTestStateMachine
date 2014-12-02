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
            //BobAnimatorState state1 = BobAnimatorState.CreateState("Base Layer.State1");
            //BobAnimatorState state2 = BobAnimatorState.CreateState("Base Layer.State2");
            //BobAnimatorState state3 = BobAnimatorState.CreateState("Base Layer.State3");

            //m_FSM0.m_States.AddValue(Animator.StringToHash(state1.m_name), state1);
            //m_FSM0.m_States.AddValue(Animator.StringToHash(state2.m_name), state2);
            //m_FSM0.m_States.AddValue(Animator.StringToHash(state3.m_name), state3);

            m_FSM0.InitAnimatorFSM(GetComponent<Animator>());
            

            //AnimatorParameter _param1 = AnimatorParameter.CreateNewTrigger("Trigger_12");
            //AnimatorParameter _param2 = AnimatorParameter.CreateNewTrigger("Trigger_23");
            //AnimatorParameter _param3 = AnimatorParameter.CreateNewTrigger("Trigger_31");
            //AnimatorParameter _param4 = AnimatorParameter.CreateNewInt("intParam", 0);

            //m_FSM0.m_parameters.AddValue(_param1.Name, _param1);
            //m_FSM0.m_parameters.AddValue(_param2.Name, _param2);
            //m_FSM0.m_parameters.AddValue(_param3.Name, _param3);
            //m_FSM0.m_parameters.AddValue(_param4.Name, _param4);

            //BobTransitionCondition _condition1 = BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>() { _param1, _param2, _param4 }, 0.1f);
            //BobTransitionCondition _condition2 = BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>() { _param2 }, 0.3f);
            //BobTransitionCondition _condition3 = BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>() { _param3 });
            //BobTransitionCondition _condition4 = BobTransitionCondition.CreateTransitionCondition(new List<AnimatorParameter>() { _param3 });

            //m_FSM0.AddTransition(state1, state2, "trans12", Animator.StringToHash("trans12"), _condition1);
            //m_FSM0.AddTransition(state1, state3, "trans13", Animator.StringToHash("trans13"), _condition2);
            //m_FSM0.AddTransition(state2, state3, "trans23", Animator.StringToHash("trans23"), _condition3);
            //m_FSM0.AddTransition(state3, state1, "trans31", Animator.StringToHash("trans31"), _condition4);

            //m_FSM0.AddGlobalTransition(state3, "trans3", Animator.StringToHash("trans3"), _condition2);
                       
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
