using UnityEngine;
using System.Collections;
using TaiDouCommon.Model;

public class PlayerSpawn : MonoBehaviour {
    public Transform[] playerspawnPostion;

    void Awake() {
        LoadPlayer();
    }
    public void LoadPlayer() {
        if(GameManger._instance.battleType==BattleType.Person)
        {
            //个人战斗
            GameObject playerPrefab = null;
            if(PhotonEngine.Instance.role.Isman)
            {
                playerPrefab = Resources.Load<GameObject>("player-transcript/Player-boy");
            }
            else
            {
                playerPrefab = Resources.Load<GameObject>("player-transcript/Player-gril");
            }
            GameObject go=GameObject.Instantiate(playerPrefab, playerspawnPostion[0].position, Quaternion.identity) as GameObject;
            TranscriptManager._instance.player = go;
            go.AddComponent<TouchControl>();//为新创建的角色添加eashtouch控制移动类
        }
        else
        {
           GameObject[] playerPrefab = {null,null,null};
           int i = 0;
           foreach(Role role in PhotonEngine.Instance.rolelist)
           {
               
               if (PhotonEngine.Instance.role.Isman)
               {
                   playerPrefab[i] = Resources.Load<GameObject>("player-transcript/Player-boy");
               }
               else
               {
                   playerPrefab[i] = Resources.Load<GameObject>("player-transcript/Player-gril");
               }
               GameObject go=GameObject.Instantiate(playerPrefab[i], playerspawnPostion[i].position, Quaternion.identity) as GameObject;
               go.AddComponent<TouchControl>();//为新创建的角色添加eashtouch控制移动类
               go.GetComponent<Player>().roleID = role.ID;
               GameManger._instance.AddPlayer(role.ID,go);
               if(role.ID==PhotonEngine.Instance.role.ID)
               {
                   //当前创建的角色是客户端控制的角色
                   TranscriptManager._instance.player = go;
               }
               else
               {
                   go.GetComponent<PlayerMove01>().isCanController = false;
               }
               i++;
           }
           //GameObject.Instantiate(playerPrefab, playerspawnPostion[0].position, Quaternion.identity);
        }
    }
}
