using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARDS);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARDS:
                setTableCards(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    private Image[] imgCode = null;
    // Use this for initialization
    void Start()
    {
        imgCode = new Image[3];
        for (int i = 0; i < imgCode.Length; i++)
            imgCode[i] = TransformHelp.Find<Image>(transform, "imgCard_"+i.ToString());
    }

    /// <summary>
    /// 设置底牌
    /// </summary>
    /// <param name="cards"></param>
    public void setTableCards(List<CardDto> cards)
    {
        string path = string.Empty;
        for (int i = 0; i < cards.Count; i++)
        {
            path = "Poker/" + CardColor.GetString(cards[i].Color) + CardColor.GetWeight(cards[i].Weight);
            imgCode[i].sprite= Resources.Load<Sprite>(path);
        }
    }
}
