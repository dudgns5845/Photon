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
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        Canvas[0].SetActive(false);
        Canvas[1].SetActive(true);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결 끊김");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        print("로비접속완료");
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });

    public override void OnCreatedRoom()
    {
        print("방생성 완료");
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public override void OnJoinedRoom() => print("방 입장 성공");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 입장 실패");

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text,new RoomOptions { MaxPlayers = 2},null);
    public override void OnJoinRandomFailed(short returnCode, string message) => print("방 입장2 실패");

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom() => print("방 나감");     

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            string playerstr = "방에 있는 플레이어 목록 : ";
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerstr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(playerstr);
        }
        else //현재 로비의 정보
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }


}
