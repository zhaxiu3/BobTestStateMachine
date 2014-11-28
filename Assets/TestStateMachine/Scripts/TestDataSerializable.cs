using UnityEngine;
using System.Collections;
namespace Engine
{
	
	public class TestDataSerializable : MonoBehaviour {
        public BobAnimatorFSMData data = new BobAnimatorFSMData();
        public BobAnimatorFSMData data2 = new BobAnimatorFSMData();
		// Use this for initialization
        void Start()
        {
            BobAnimatorStateData _state1 = new BobAnimatorStateData();
            _state1.m_name = "state1";
            BobAnimatorStateData _state2 = new BobAnimatorStateData();
            _state2.m_name = "state2";

            BobAnimatorParameterData param1 = new BobAnimatorParameterData();
            param1.m_name = "Boolparam";
            param1.m_type = AnimatorParameterType.BOOL;
            param1.m_boolValue = true;

            BobAnimatorTransitionData _transition = new BobAnimatorTransitionData();
            _transition.m_name = "Transition1";
            _transition.m_fromState = _state1;
            _transition.m_toState = _state2;
            _transition.m_parameters.Add(param1);
            _transition.m_ExitTime = 0.2f;

            data.m_states.AddValue(_state1.m_name,_state1);
            data.m_states.AddValue(_state2.m_name,_state2);
            data.m_transitions.AddValue(_transition.m_name, _transition);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
