using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Dto;
using Protocol.Constant;

public class ChatHandler : HanderBase
{

    private ChatMsg msg;
    public ChatHandler()
    {
        msg = new ChatMsg();
    }
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case ChatCode.SRES:
                ChatDto dto = value as ChatDto;
                int userId = dto.UserId;
                int chatType = dto.ChatType;
                string text = Constant.GetChatText(chatType);
                msg.Change(userId, chatType, text);
                //文字
                Dispatch(AreaCode.UI, UIEvent.PLAYER_CHAT, msg);
                //声音
                Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Chat/Chat_" + chatType);
                break;
            default:
                break;
        }
    }
}
