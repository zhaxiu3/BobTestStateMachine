using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Engine.Test
{
	[System.Serializable]
	public class BobAnimatorFSMData
	{
        public BobAnimatorStateDictionary m_states;// = new BobAnimatorStateDictionary();
        public BobAnimatorTransitionDictionary m_transitions;// = new BobAnimatorTransitionDictionary();
	}
	
	[System.Serializable]
	public class BobAnimatorStateData {
	    public string m_name;
	}
	
	[System.Serializable]
	public class BobAnimatorTransitionData {
	    public string m_name;
        public BobAnimatorStateData m_fromState;
        public BobAnimatorStateData m_toState;
	    public float m_ExitTime;
	    public List<BobAnimatorParameterData> m_parameters = new List<BobAnimatorParameterData>();
	}
	
	[System.Serializable]
	public class BobAnimatorParameterData {
	    public string m_name;
        public AnimatorParameterType m_type;
        public float m_floatValue;
        public int m_intValue;
        public bool m_boolValue;
	}

    [System.Serializable]
    public class BobAnimatorStateDictionary : BobDictionary<string, BobAnimatorStateData> { }

    [System.Serializable]
    public class BobAnimatorTransitionDictionary : BobDictionary<string, BobAnimatorTransitionData> { }
}