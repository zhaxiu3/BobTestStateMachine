using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Engine
{

	public class BobStateMachineRoot : MonoBehaviour
    {
        BobAnimatorFSM m_FSM0 = new BobAnimatorFSM();
        BobAnimatorState state1 = new BobAnimatorState();
        BobAnimatorState state2 = new BobAnimatorState();
        BobAnimatorState state3 = new BobAnimatorState();
        public string m_fsmJson;
        //BobAnimatorFSM m_FSM1 = newBobAnimatorFSM();
        void Awake()
        {
            m_FSM0.Init(state1, this.GetComponent<Animator>()); int _____________fixit;

            AnimatorParameter _param1 = new AnimatorParameter("Trigger_12", AnimatorParameterType.TRIGGER, true);
            AnimatorParameter _param2 = new AnimatorParameter("Trigger_23", AnimatorParameterType.TRIGGER, true);
            AnimatorParameter _param3 = new AnimatorParameter("Trigger_31", AnimatorParameterType.TRIGGER, true);

            m_FSM0.AddTransition(state1, state2, "trans12", Animator.StringToHash("trans12"), new List<AnimatorParameter>() { _param1 }, 0.1f);
            m_FSM0.AddTransition(state2, state3, "trans23", Animator.StringToHash("trans23"), new List<AnimatorParameter>() { _param2 }, 0.3f);
            m_FSM0.AddTransition(state3, state1, "trans31", Animator.StringToHash("trans31"), new List<AnimatorParameter>() { _param3 });

            m_fsmJson = fastJSON.JSON.ToJSON(m_FSM0);
            Debug.Log(m_fsmJson);
            
           
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
                m_FSM0.SendEvent("trans12");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_23");
                m_FSM0.SendEvent("trans23");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //GetComponent<Animator>().SetTrigger("Trigger_31");
                m_FSM0.SendEvent("trans31");
            }
		}
	}
}
