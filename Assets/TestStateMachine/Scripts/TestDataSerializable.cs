using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Engine
{
	
	public class TestDataSerializable : MonoBehaviour {
        Animator m_animator;
        public List<string> stateNames;
		// Use this for initialization
        void Start()
        {
            m_animator = GetComponent<Animator>();
            for (int i = 0; i < m_animator.layerCount; i++)
            {
                AnimatorStateInfo _currentState =  m_animator.GetCurrentAnimatorStateInfo(i);
                Debug.Log(_currentState.nameHash);
            }

            for (int i = 0; i < stateNames.Count; i++)
            {
                Debug.Log(stateNames[i] + ":" + Animator.StringToHash(stateNames[i]));
            }
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
