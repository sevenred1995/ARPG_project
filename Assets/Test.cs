using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    private bool isFirst = true;
    void Start() {

    }
    void Update() {
        Debug.Log("Test1");
    }
    void FixedUpdate() {
        transform.Rotate(Vector3.up);
    }
    void LateUpdate() {
        Debug.Log("Test2");
    }
    public void SetTime() {
        if(isFirst)
        {
            Time.timeScale = 0;
            isFirst = false;
        }
        else
        {
            Time.timeScale = 1;
            isFirst = true;
        }
    }

}
