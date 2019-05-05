using Protocol.Code;
using Protocol.Dto.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerBase : CharacterBase
{

    /// <summary>
    /// 卡牌的父物体
    /// </summary>
    protected Transform cardParent;

 

    private List<GameObject> cardObjList;
    // Use this for initialization
    protected void Start()
    {
        cardParent = TransformHelp.Find(transform, "CardPoint");
        cardObjList = new List<GameObject>();
        //StartCoroutine(initCardList());
    }

    protected IEnumerator initCardList()
    {
        GameObject carPrefab = Resources.Load<GameObject>("Card/OtherCard");

        for (int i = 0; i < 17; i++)
        {
            createGo(i, carPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// /创建卡牌 object
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index"></param>
    private void createGo(int index, GameObject carPrefab)
    {
        GameObject cardGo = GameObject.Instantiate(carPrefab, cardParent);
        cardGo.transform.localPosition = new Vector2(0.2f * index, 0);
        cardGo.GetComponent<SpriteRenderer>().sortingOrder = index;
        cardObjList.Add(cardGo);
    }
    /// <summary>
    /// 添加底牌
    /// </summary>
    public void addTableCard()
    {
        GameObject carPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 3; i++)
        {
            createGo(i, carPrefab);
        }
    }

    /// <summary>
    /// 移除卡牌
    /// </summary>
    /// <param name="remainCardList"></param>
    public void removeCard(int cardCount)
    {
        for (int i = cardCount; i < cardObjList.Count; i++)
        {
            cardObjList[i].gameObject.SetActive(false);
        }
    }

}
