using UnityEngine;
using System.Collections;

public class CameraFllow : MonoBehaviour {
    public GameObject player;
    public Vector3 offset;
    public float smoothing = 2;
    public bool isController=true;
    void Start()
    {
        player = GameManger._instance.player;
    }
    void FixedUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;
        transform.position=Vector3.Lerp(transform.position,targetPos,smoothing*Time.deltaTime);
    }
}
