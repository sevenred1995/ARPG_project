using UnityEngine;
using System.Collections;

public class PowerShow : MonoBehaviour {

   // public static PowerShow _instance;

    private bool isStart=true;
    private bool isUp = true;//是否为值的上升
    private int startValue=0;
    private int endValue=0;
    private float speed = 10;

    
    public UILabel numLabel;
    public TweenAlpha numTween;

    void Awake()
    {
        //_instance = this;
        numLabel = transform.Find("Label").GetComponent<UILabel>();
        numTween = transform.GetComponent<TweenAlpha>();
        EventDelegate ed = new EventDelegate(this, "OnTween");
        numTween.onFinished.Add(ed);
        gameObject.SetActive(false);
    }
    void Update()
    {
        if(isStart)
        {
            if(isUp)
            {
                startValue += (int)(speed * Time.deltaTime);
                if (startValue > endValue)
                {
                    startValue = endValue;
                    isStart = false;
                    numTween.PlayReverse();
                }
            }else
            {
                startValue -= (int)(speed * Time.deltaTime);
                if (startValue < endValue)
                {
                    startValue = endValue;
                    isStart = false;
                    numTween.PlayReverse();
                }
            }
            numLabel.text = startValue.ToString();
        }
    }
    public void ShowNumChange(int startValue,int endValue)
    {
        gameObject.SetActive(true);
        numTween.PlayForward();
        this.startValue = startValue;
        this.endValue = endValue;
        if(startValue<endValue)
        {
            isUp = true;
        }else
        {
            isUp = false;
        }
        speed = Mathf.Abs(startValue - endValue) * 0.5f;
        isStart = true;
    }
    void OnTween()
    {
        if(isStart==false)
        {
            gameObject.SetActive(false);
        }
    }
}
