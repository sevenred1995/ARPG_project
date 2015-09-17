using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyPostionModel{
    public List<EnemyPostionProperty> list = new List<EnemyPostionProperty>();
}
public class EnemyPostionProperty {
    public string guid;
    public Vector3Obj postion;
    public Vector3Obj eulerAnglers;
}