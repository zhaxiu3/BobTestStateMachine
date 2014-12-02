using UnityEngine;
using System.Collections;

namespace Engine.Test
{
    public class BobTransition
    {
        public int m_uniqueHash;
        public string m_uniqueName;
        public BobState m_fromState;
        public BobState m_toState;
        public bool isGlobal
        {
            get
            {
                return m_fromState == null;
            }
        }
    }
}

