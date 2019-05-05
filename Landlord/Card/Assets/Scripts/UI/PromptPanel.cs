using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : UIBase {

    private Text tex;
    private CanvasGroup cg;
    [SerializeField]
    [Range(0,3)]
    private float showTime = 1;
    private float timer = 0;

    private void Awake()
    {
        Bind(UIEvent.PROMPT_MSG);
    }
    public override void Execute(int eventCode, object message)
    {
       switch(eventCode)
        {
            case UIEvent.PROMPT_MSG:
                PromptMsg msg = message as PromptMsg;
                promptMessage(msg.Text,msg.Color);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        tex = TransformHelp.Find<Text>(transform, "Text");
        cg = TransformHelp.Find<CanvasGroup>(transform, "Text");
        cg.alpha = 0;
        tex.text = null;
    }
	
    private void promptMessage(string text,Color color)
    {
        tex.text = text;
        tex.color = color;
        cg.alpha = 0;
        timer = 0;
        StartCoroutine(promptAnim());
    }

    IEnumerator promptAnim()
    {
        while(cg.alpha<1)
        {
            cg.alpha += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }

        while(timer<=1)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
