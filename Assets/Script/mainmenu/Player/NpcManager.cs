using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcManager : MonoBehaviour {

    public static NpcManager _instance;
    public GameObject[] npcArray;
    private Dictionary<int, GameObject> npcDic = new Dictionary<int, GameObject>();

    void Awake()
    {
        _instance = this;
        InitNpcDic();
    }
    void InitNpcDic()
    {
        foreach(GameObject go in npcArray)
        {
            int id=int.Parse(go.name.Substring(0,4));
            npcDic.Add(id, go);
        }
    }
    public GameObject GetNpcById(int id)
    {
        GameObject go;
        npcDic.TryGetValue(id, out go);
        return go;
    }

}
