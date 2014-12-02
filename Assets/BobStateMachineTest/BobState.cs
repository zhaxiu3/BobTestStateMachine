using UnityEngine;
namespace Engine.Test
{
    public enum BobStateEnum
    {
        INVALID = -1
    }
	[System.Serializable]
	public class BobState
	{
	    /// <summary>
	    /// 当前是否激活
	    /// </summary>
	    public bool active = false;
	    /// <summary>
	    /// 状态名
	    /// </summary>
	    public string m_name;
        
        public virtual void OnEnter(object owner, string transname) {
            active = true;
        }
        public virtual void OnExit(object owner, string transname)
        {
            active = false;
        }
        public virtual void OnUpdate(object owner)
        { 
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
