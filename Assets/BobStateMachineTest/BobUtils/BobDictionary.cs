using System.Collections.Generic;

namespace Engine.Test
{
	/// <summary>
	/// 自己实现的可以在Inspector上显示的字典
    /// 
	/// </summary>
	[System.Serializable]
	public class BobDictionary<TKey,TValue> 
	{
        /// <summary>
        /// 元素列表
        /// </summary>
	    [UnityEngine.SerializeField]
	    public List<TValue> values = new List<TValue>();
        /// <summary>
        /// 元素名字列表
        /// </summary>
	    [UnityEngine.SerializeField]
	    public List<TKey> keys = new List<TKey>();
        /// <summary>
        /// 添加新的元素
        /// </summary>
        /// <param name="value">元素值</param>
        /// <param name="name">元素名字</param>
	    public void AddValue(TKey key, TValue value)
	    {	
	        if (keys.Contains(key))
	        {
                throw new System.Exception(string.Format("The key {0} has already existed!",key.ToString()));
	        }
	        values.Add(value);
	        keys.Add(key);
	    }
        /// <summary>
        /// 删除名为name的元素
        /// </summary>
        /// <param name="name">索引元素用到的名字</param>
	    public void RemoveValue(TKey key)
	    {
	        int index = keys.IndexOf(key);
	        if (index >= 0)
	        {
	            values.RemoveAt(index);
	            keys.RemoveAt(index);
	        }
	    }
        /// <summary>
        /// 删除第index个元素
        /// </summary>
        /// <param name="index">元素下标</param>
	    public void RemoveAt(int index)
	    {
	        values.RemoveAt(index);
	        keys.RemoveAt(index);
	    }
        /// <summary>
        /// 获取名为name的元素的值
        /// </summary>
        /// <param name="name">索引元素用到的名字</param>
        /// <returns></returns>
	    public TValue getValue(TKey key) 
	    {
	        int index = keys.IndexOf(key);
	        if (index >= 0)
	        {
	            return values[index];
	        }
	        return default(TValue);
	    }
        /// <summary>
        /// 清空列表
        /// </summary>
	    public void Clear()
	    {
	        keys.Clear();
	        values.Clear();
	    }
        /// <summary>
        /// 列表的元素个数
        /// </summary>
	    public int Count
	    {
	        get
	        {
	            return values.Count;
	        }
	    }
	
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">元素下标</param>
        /// <returns>返回第index个元素</returns>
	    public TValue this[int index]
	    {
	        get
	        {
	            return values[index];
	        }
	    }
        /// <summary>
        /// 返回列表的名字列表
        /// </summary>
	    public List<TKey> KeyList
	    {
	        get
	        {
	            return keys;
	        }
	    }
        /// <summary>
        /// 列表中是否包含了名字为_name的元素
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
	    public bool ContainsKey(TKey key)
	    {
	        return KeyList.Contains(key);
	    }
	}
}
