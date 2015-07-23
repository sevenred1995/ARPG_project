using UnityEngine;
using System.Collections;

public class CameraFllow : MonoBehaviour {
    private GameObject player;
    private Vector3 offset;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0, 20f, 20f);
    }
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
