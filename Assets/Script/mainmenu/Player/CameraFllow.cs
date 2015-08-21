using UnityEngine;
using System.Collections;

public class CameraFllow : MonoBehaviour {
    public GameObject player;
    public Vector3 offset;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.Find("Bip01").gameObject;
        //offset = new Vector3(0, 20f, 20f);
    }
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
