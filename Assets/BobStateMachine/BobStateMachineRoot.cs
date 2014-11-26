using UnityEngine;
using System.Collections;
namespace Engine
{
    [System.Serializable]
    public class BobAnimatorFSM : BobFSM<Animator> { }
	public class BobStateMachineRoot : MonoBehaviour
    {
        BobAnimatorFSM m_FSM0 = new BobAnimatorFSM();
        BobAnimatorState state1 = new BobAnimatorState();
        BobAnimatorState state2 = new BobAnimatorState();
        BobAnimatorState state3 = new BobAnimatorState();
        //BobAnimatorFSM m_FSM1 = newBobAnimatorFSM();
        void Awake()
        {

            m_FSM0.Init(state1, this.GetComponent<Animator>()); int _____________fixit;
            m_FSM0.AddTransition(state1, state2, "trans12", Animator.StringToHash("trans12"));
            m_FSM0.AddTransition(state2, state3, "trans23", Animator.StringToHash("trans23"));
            m_FSM0.AddTransition(state3, state1, "trans31", Animator.StringToHash("trans31"));
           
        }
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
            m_FSM0.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                m_FSM0.SendEvent("trans12");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_FSM0.SendEvent("trans23");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_FSM0.SendEvent("trans31");
            }
		}
	}
}
