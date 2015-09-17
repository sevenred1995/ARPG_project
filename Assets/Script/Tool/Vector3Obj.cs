using UnityEngine;
using System.Collections;

public class Vector3Obj{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public Vector3Obj(){

    }
    public Vector3Obj(Vector3 temp) {
        x = temp.x;
        y = temp.y;
        z = temp.z;
    }
    public Vector3 ToVector3() {
        Vector3 temp;
        temp.x = (float)this.x;
        temp.y = (float)this.y;
        temp.z = (float)this.z;
        return temp;
    }
}
