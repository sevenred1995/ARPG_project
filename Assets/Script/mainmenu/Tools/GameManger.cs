using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum BattleType {
    Person,
    Team,
    None
}
public class GameManger : MonoBehaviour {

    public static GameManger _instance;
    public GameObject pos;

    public BattleType battleType = BattleType.None;
    public int trancriptID = -1;
    public bool isMaster = false;

    public Dictionary<int, GameObject> playerDict = new Dictionary<int, GameObject>();
    public BattleController battleController;
    public void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        pos=GameObject.Find("spawnPos");
        string prefabname = "Player-gril";
        if(PhotonEngine.Instance.role!=null)
        {
            if (PhotonEngine.Instance.role.Isman)
                prefabname = "Player-boy";
        }
        GameObject playeGo = GameObject.Instantiate(Resources.Load<GameObject>("player/" + prefabname))as GameObject;
        playeGo.transform.position = pos.transform.position;
        battleController = this.GetComponent<BattleController>();
        battleController.OnAsyncPostionAndRotation += this.OnAsyncPostionAndRotation;
        battleController.OnAsyncPlayerMoveAnimation += this.OnAsyncPlayerMoveAnimation;
    }


    //计算经验值
    public static int GetRequireExpByLevel(int level)
    {
        return (int)((level - 1) * (100f + (100f + (100f + 10f * (level - 2f)))) / 2);
    }
    public void AddPlayer(int roleID,GameObject go) {
        playerDict.Add( roleID, go);
    }
    private void OnAsyncPostionAndRotation(int roleID, Vector3 pos, Vector3 eulerAnglers) {
        GameObject go = null;
        if(playerDict.TryGetValue(roleID, out go))
        {
            go.GetComponent<PlayerMove01>().SetPostionAndRotation(pos, eulerAnglers);
        }else
        {
            Debug.LogWarning("don't find a player gameobject");
        }
    }
    private void OnAsyncPlayerMoveAnimation(int roleID, PlayerMoveAnimationModel model) {
        GameObject go = null;
        if (playerDict.TryGetValue(roleID, out go))
        {
            go.GetComponent<PlayerMove01>().SetAnimation(model);
        }
        else
        {
            Debug.LogWarning("Don't find a player gameobject");
        }
    }
   
}
