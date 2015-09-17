using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateEnemyModel {
    public List<CreateEnemyProperty> list = new List<CreateEnemyProperty>();
}
public class CreateEnemyProperty {
    public string GUID;
    public string PrefabName;
    public Vector3Obj Postion;
}
