using Protocol.Code;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase {

    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD,
            CharacterEvent.ADD_MY_CARS,
            CharacterEvent.DEAL_CARD,
            CharacterEvent.REMOVE_MY_CARD
            );
    }


    public  override void Exectue(int eventCode, object messagge)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                StartCoroutine(initCardList(messagge as List<CardDto>));
                break;
            case CharacterEvent.ADD_MY_CARS:
                addTableCard(messagge as GrabDto);
                break;
            case CharacterEvent.DEAL_CARD:
                dealSelectCard();
                break;
            case CharacterEvent.REMOVE_MY_CARD:
                removeCard(messagge as List<CardDto>);
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

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    // Use this for initialization
    void Start () {
        cardParent = TransformHelp.Find(transform, "CardPoint");
        cardCtrlList = new List<CardCtrl>();
        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }
    /// <summary>
    /// 出牌
    /// </summary>
    private void dealSelectCard()
    {
        List<CardDto> selectCardList = getSelectCard();
        DealDto dto = new DealDto(selectCardList, Models.GameModel.UserDto.Id);
        if (!dto.IsRegular)
        {
            promptMsg.Change("请选择合理的手牌！！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        }
        else
        {
            socketMsg.Change(OpCode.FIGHT, FightCode.DEAL_CREQ, dto);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
            
    }
    /// <summary>
    /// 移除卡牌
    /// </summary>
    /// <param name="remainCardList"></param>
    private void removeCard(List<CardDto> remainCardList)
    {
        int index = 0;
        foreach (var cc in cardCtrlList)
        {
            if (remainCardList.Count == 0)
                break;
            else
            {
                cc.gameObject.SetActive(true);
                cc.Init(remainCardList[index], index, true);
                index++;
                if (index == remainCardList.Count)
                    break;
            }
        }
        for (int i = index; i < cardCtrlList.Count; i++)
        {
            cardCtrlList[i].Selected = false;
            ///销毁
            cardCtrlList[i].gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 获取选中的牌
    /// </summary>
    private List<CardDto> getSelectCard()
    {
        List<CardDto> selectCardList = new List<CardDto>();
        foreach (var cardCtrl in cardCtrlList)
        {
            if (cardCtrl.Selected)
                selectCardList.Add(cardCtrl.cardDto);
        }
        return selectCardList;
    }

    /// <summary>
    /// 添加底牌
    /// </summary>
    /// <param name="cardList"></param>
    void addTableCard(GrabDto dto)
    {
        List<CardDto> tableCards = dto.TableCardList;
        List<CardDto> playerCards = dto.PlayerCardList;
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            cardCtrl.gameObject.SetActive(true);
            cardCtrl.Init(playerCards[index], index, true);

            index++;
        }
        GameObject carPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = index; i < playerCards.Count; i++)
        {
            createGo(playerCards[i], i, carPrefab);
        }
    }
	
    private IEnumerator initCardList(List<CardDto> cardList)
    {
        GameObject carPrefab = Resources.Load<GameObject>("Card/MyCard");

        for (int i = 0; i < cardList.Count; i++)
        {
            createGo(cardList[i], i, carPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// /创建卡牌 object
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void createGo(CardDto card,int index, GameObject carPrefab)
    {
        GameObject cardGo = GameObject.Instantiate(carPrefab, cardParent);
        
        cardGo.name = card.Name;
        cardGo.transform.localPosition = new Vector2((0.5f * index), 0);
        CardCtrl cardCtrl = cardGo.GetComponent<CardCtrl>();
        cardCtrl.Init(card, index, true);
        this.cardCtrlList.Add(cardCtrl);
    }

}
