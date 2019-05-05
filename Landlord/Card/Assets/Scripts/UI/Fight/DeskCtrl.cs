using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCtrl : CharacterBase
{

    private void Awake()
    {
        Bind(CharacterEvent.UPFATE_SHOW_DESK);
    }


    public override void Exectue(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.UPFATE_SHOW_DESK:
                updateShowDesk(message as List<CardDto>);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 卡牌的列表
    /// </summary>
    private List<CardCtrl> cardCtrlList;
    /// <summary>
    /// 卡牌的父物体
    /// </summary>
    private Transform cardParent;
    // Use this for initialization
    void Start () {
        cardParent = TransformHelp.Find(transform, "CardPoint");
        cardCtrlList = new List<CardCtrl>();
    }

    private void updateShowDesk(List<CardDto> cardList)
    {
        if(cardCtrlList.Count>cardList.Count)
        {
            //原来比现在的多
            int index = 0;
            foreach (var cardCtrl in cardCtrlList)
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(cardList[index], index, true);
                index++;
                if (index >= cardList.Count)
                    break;
            }
            for (int i = index; i < cardCtrlList.Count; i++)
            {
                cardCtrlList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            //原来比现在的少
            int index = 0;
            foreach (var cardCtrl in cardCtrlList)
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(cardList[index], index, true);
                index++;
            }
            GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
            for (int i = index; i < cardList.Count; i++)
            {
                createGo(cardList[i], i, cardPrefab);
            }
        }
    }
    /// <summary>
    /// /创建卡牌 object
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void createGo(CardDto card, int index, GameObject carPrefab)
    {
        GameObject cardGo = GameObject.Instantiate(carPrefab, cardParent);
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.5f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);
        this.cardCtrlList.Add(cardCtrl);
    }


}
