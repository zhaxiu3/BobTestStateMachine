using UnityEngine;
using System.Collections;
namespace Engine
{
	public class BobStateMachineRoot : MonoBehaviour
	{
        BobFSM<MonoBehaviour> m_FSM = new BobFSM<MonoBehaviour>();
        void Awake()
        {
            m_FSM.Init(new BobState<MonoBehaviour>(), this); int _____________fixit;
        }
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
