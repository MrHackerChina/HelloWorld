using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌控制类
/// </summary>
public class CardCtrl : MonoBehaviour {

    /// <summary>
    /// 数据
    /// </summary>
    public CardDto cardDto { get; set; }

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 是不是自身的
    /// </summary>
    private bool isMine;

    /// <summary>
    /// 是否被选中
    /// </summary>
    public  bool Selected { get; set; }




    /// <summary>
    /// 初始化卡牌数据
    /// </summary>
    /// <param name="card"></param>
    /// <param name="index">第几张牌</param>
    /// <param name="isMine"></param>
    public void Init(CardDto card,int index,bool isMine)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.cardDto = card;
        this.isMine = isMine;
        if(Selected)
        {
            Selected = false;
            transform.localPosition -= new Vector3(0, 0.3f, 0);
        }
        string path = string.Empty;
        if (isMine)
            path = "Poker/" + CardColor.GetString(card.Color) + CardColor.GetWeight(card.Weight); /*+ card.Name;*/
        else
            path = "Poker/Back";
        Sprite sp = Resources.Load<Sprite>(path);
        spriteRenderer.sprite = sp;
        spriteRenderer.sortingOrder = index;
    }

    private void OnMouseDown()
    {
        if (!isMine)
            return;
        this.Selected = !Selected;
        if(Selected)
            transform.localPosition += new Vector3(0, 0.3f, 0);
        else
            transform.localPosition -= new Vector3(0, 0.3f, 0);
    }

    /// <summary>
    /// 选择的状态
    /// </summary>
    public void SelectedState()
    {
        if(!Selected)
        {
            Selected = true;
            transform.localPosition += new Vector3(0, 0.3f, 0);
        }
    }
}
