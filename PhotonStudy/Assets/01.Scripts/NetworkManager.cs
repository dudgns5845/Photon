using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text statusText;
    public InputField roomInput, nickNameInput;

    public GameObject[] Canvas;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        for(int i = 0; i < Canvas.Length; i++)
        {
            Canvas[i].SetActive(false);
        }
        Canvas[0].SetActive(true);
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        print("���� ���� �Ϸ�");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        Canvas[0].SetActive(false);
        Canvas[1].SetActive(true);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("���� ����");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        print("�κ����ӿϷ�");
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });

    public override void OnCreatedRoom()
    {
        print("����� �Ϸ�");
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public override void OnJoinedRoom() => print("�� ���� ����");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("�� ���� ����");

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text,new RoomOptions { MaxPlayers = 2},null);
    public override void OnJoinRandomFailed(short returnCode, string message) => print("�� ����2 ����");

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom() => print("�� ����");     

    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο� �� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο� �� : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            string playerstr = "�濡 �ִ� �÷��̾� ��� : ";
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerstr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(playerstr);
        }
        else //���� �κ��� ����
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }


}
