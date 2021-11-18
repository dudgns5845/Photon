using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomInfoButton : MonoBehaviour
{
    public Text btn_Text;

    public void SetInfo(string roomName, int currentPlayer, int maxPlayer)
    {
        btn_Text.text = roomName + "(" + currentPlayer + "/" + maxPlayer + ")";

        this.roomName = roomName;
    }

    string roomName;
    public void Onclick()
    {
        GameObject input = GameObject.Find("Canvas/InputRoomName");
        if(input != null)
        {
            InputField field = input.GetComponent<InputField>();
            field.text = roomName;
        }
    }
}
