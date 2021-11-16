using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField inputNickName;
    void Start()
    {     
    }

    void Update()
    {        
    }

    public void OnConnect()
    {
        //게임 버전 설정
        PhotonNetwork.GameVersion = "1";
        //접속 시도 (name서버 -> master서버)
        PhotonNetwork.ConnectUsingSettings();        
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod());
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod());

        //만약에 InputField에 값이 있다면
        if(inputNickName.text.Length > 0)
        {
            //닉네임 설정
            PhotonNetwork.NickName = inputNickName.text;
            //로비 진입 시도
            PhotonNetwork.JoinLobby();
            //PhotonNetwork.JoinLobby(new TypedLobby("로비이름", LobbyType.Default));
        }
        else
        {
            print("닉네임을 입력해주세요~");
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비접속 성공");
        //로비씬으로 이동
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
