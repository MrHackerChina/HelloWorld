using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMessage  {

    public int mainkey { get; set; }
    public int subkey { get; set; }
    public object message { get; set; }

    public IMessage()
    {

    }

    public void Change(int main,int sub,object mess)
    {
        this.mainkey = main;
        this.subkey = sub;
        this.message = mess;
    }
}
