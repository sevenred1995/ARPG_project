using UnityEngine;
using System.Collections;

public class PlayerShow : MonoBehaviour {
    public void OnPress(bool isPress)
    {
        if(!isPress)
        {
            StartmenuController.instance.OnPlayerSelect(transform.parent.gameObject);
        }
    }

}
