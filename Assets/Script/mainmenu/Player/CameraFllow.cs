using UnityEngine;
using System.Collections;

public class CameraFllow : MonoBehaviour {
    public GameObject player;
    public Vector3 offset;
    public float smoothing = 2;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.Find("Bip01").gameObject;
        //offset = new Vector3(0, 20f, 20f);
    }
    void FixedUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;
        transform.position=Vector3.Lerp(transform.position,targetPos,smoothing*Time.deltaTime);
    }
}
