using UnityEngine;
using System.Collections;

public class GameManger : MonoBehaviour {
    //计算经验值
    public static int GetRequireExpByLevel(int level)
    {
        return (int)((level - 1) * (100f + (100f + (100f + 10f * (level - 2f)))) / 2);
    }
}
