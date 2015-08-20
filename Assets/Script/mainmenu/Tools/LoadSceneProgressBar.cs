using UnityEngine;
using System.Collections;

public class LoadSceneProgressBar : MonoBehaviour {

    public static LoadSceneProgressBar _instance;
    
    private GameObject bg;
    private UISlider progresBar;
    private bool isAsyn = false;
    private AsyncOperation ao = null;
    void Awake()
    {
        _instance = this;
        gameObject.SetActive(false);

        bg = transform.Find("Bg").gameObject;
        progresBar = transform.Find("Bg/Progressbg").GetComponent<UISlider>();
    }
    void Update()
    {
        if(isAsyn)
        {
            progresBar.value = ao.progress;
        }
    }
    public void Show(AsyncOperation ao)
    {
        gameObject.SetActive(true);
        bg.SetActive(true);
        isAsyn = true;
        this.ao = ao;
    }

}
