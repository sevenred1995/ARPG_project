using UnityEngine;
using System.Collections;

public class CameraFllow : MonoBehaviour {
    public GameObject player;
    public Vector3 offset;
    public float smoothing = 2;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;
        transform.position=Vector3.Lerp(transform.position,targetPos,smoothing*Time.deltaTime);
    }
}
