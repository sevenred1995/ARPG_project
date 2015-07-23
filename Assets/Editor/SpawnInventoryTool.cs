using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpawnInventoryTool:Editor {
    static Vector3 firstPos = new Vector3(-116,80,0);
    [MenuItem("Tools/CREATEINVENTORY")]
    static void CreateInventoryItem()
    {

        GameObject go = GameObject.Find("InventoryScroll");
        Object[] selection = (Object[])Selection.objects;
        if (selection.Length == 0) return;
        for(int i=0;i<32;i++)
        {
            int num = go.GetComponentsInChildren<UISprite>().Length;
            Debug.Log(num / 2);
            GameObject itemGo = Instantiate(selection[0]) as GameObject;
            itemGo.transform.parent = go.transform;
            itemGo.transform.localPosition = GetNextPos(num / 2);
            itemGo.transform.localScale = new Vector3(1, 1, 1);
            itemGo.transform.name = selection[0].name;
        }
        
    }
    static Vector3 GetNextPos(int num)
    {
        int row=num/4;
        int clum=num%4;
        Vector3 Nextpos=new Vector3(firstPos.x+clum*75f,firstPos.y-row*75f,0);
        return Nextpos;
    }
}
