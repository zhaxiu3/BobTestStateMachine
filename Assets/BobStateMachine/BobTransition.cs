using UnityEngine;
using System.Collections;

namespace Engine
{
    public class BobTransition<T>
    {
        public int m_uniqueHash;
        public string m_uniqueName;
        public BobState<T> m_fromState;
        public BobState<T> m_toState;
        public bool isGlobal
        {
            get
            {
                return m_fromState == null;
            }
        }
    }
}

