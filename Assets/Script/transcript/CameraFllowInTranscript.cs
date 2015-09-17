using UnityEngine;
using System.Collections;

public class CameraFllowInTranscript : MonoBehaviour {

    public GameObject player;
    public Vector3 offset;
    public float smoothing = 2; 
    void Start() {
        player =  TranscriptManager._instance.player.transform.Find("Bip01").gameObject;
    }
    void FixedUpdate() {
        Vector3 targetPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
