using UnityEngine;
namespace Engine
{
    public enum BobStateEnum
    {
        INVALID = -1
    }
	[System.Serializable]
	public class BobState<T>
	{
	    /// <summary>
	    /// 当前是否激活
	    /// </summary>
	    public bool active = false;
	    /// <summary>
	    /// 状态名
	    /// </summary>
	    public BobStateEnum m_name;
        
        public virtual void OnEnter(T owner) {
            active = true;
        }
        public virtual void OnExit(T owner) {
            active = false;
        }
        public virtual void OnUpdate(T owner) { 
        }

	}

    /// <summary>
    /// 状态机事件，发送事件以改变状态
    /// </summary>
    public class BobStateEvent
    {
        public string m_name = "";
    }
}
