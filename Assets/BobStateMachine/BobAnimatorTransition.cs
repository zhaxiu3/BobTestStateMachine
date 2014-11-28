using UnityEngine;
using System.Collections;

namespace Engine
{
    [System.Serializable]
	public class BobAnimatorTransition : BobTransition<Animator> {
        public float m_ExitTime;
	}
}

