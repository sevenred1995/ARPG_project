using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour {

	public PosType posType;
    public float coldTime=4;
    private float coldTimer;
    private UISprite maskSprite;
    private UIButton btn;
	private PlayerAnimationControll playerAnimationControll;
    private PlayerAttack playerAttack;
	void Start()
	{
		playerAnimationControll=TranscriptManager._instance.player.GetComponent<PlayerAnimationControll>();
        playerAttack = TranscriptManager._instance.player.GetComponent<PlayerAttack>();
        if(transform.Find("Mask"))
           maskSprite = transform.Find("Mask").GetComponent<UISprite>();  
        btn = this.GetComponent<UIButton>();
    }
    void Update()
    {
        if(playerAttack.hp<=0)
        {
            this.GetComponent<UIButton>().isEnabled = false;
            return;
        }
        if (maskSprite == null) return;
        if (coldTimer > 0)
        {
            coldTimer -= Time.deltaTime;
            maskSprite.fillAmount = (coldTimer / coldTime);
            if(coldTimer<=0)
            {
                Enable();
            }
        }else
        {
            maskSprite.fillAmount = 0;
        }
    }

	public void OnPress(bool isPress)
	{
		playerAnimationControll.OnAttackButtonClick(isPress,posType);
        if(isPress)
        {
            coldTimer = coldTime;
            Disable();
        }
	}
    public void Disable()
    {
        this.collider.enabled = false;
        btn.SetState(UIButtonColor.State.Normal, true);
    }
    public void Enable()
    {
        this.collider.enabled = true;
    }
}
