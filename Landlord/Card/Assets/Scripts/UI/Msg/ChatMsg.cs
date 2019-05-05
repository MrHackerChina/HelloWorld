using System;
using System.Collections.Generic;

public class ChatMsg
{
    public int UserId;
    public int ChatType;
    public string Text;

    public ChatMsg()
    {

    }
    public void Change(int userId,int chatType,string text)
    {
        this.UserId = userId;
        this.ChatType = chatType;
        this.Text = text;
    }

}

