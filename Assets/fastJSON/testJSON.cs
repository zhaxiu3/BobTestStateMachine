using UnityEngine;
using System.Collections;
using fastJSON;
using System.Collections.Generic;
[System.Serializable]
public class TestA
{
    public int a = 0;
    public int b = 1;
    public string haha = "asdf";
    public List<string> d = new List<string>() { "good","bad" };
    public Dictionary<string, int> e = new Dictionary<string, int>() { { "en", 12 }, {"en2",12}};

}
public class testJSON : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TestA data = new TestA();
        string json = fastJSON.JSON.ToJSON(data);
        Debug.Log(json);
        TestA b = fastJSON.JSON.ToObject<TestA>(json);
        Debug.Log(b.haha);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
