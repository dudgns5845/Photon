using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager instance;

    //나의 포톤View
    public PhotonView myPhotonView;

    //플레이어 생성 위치들
    public Transform[] playerPos;
    //현재 위치해야할 Index
    int playerPosIndex;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            CreatePlayer();
        }
        else
        {
            //게임 버전 설정
            PhotonNetwork.GameVersion = "1";
            //접속 시도 (name서버 -> master서버)
            PhotonNetwork.ConnectUsingSettings();
        }        
    }

    public void SetPlayerPos(PhotonView pv)
    {
        pv.RPC("RpcSetPlayerPos", RpcTarget.AllBuffered, playerPos[playerPosIndex].position);
        playerPosIndex++;
    }

    void CreatePlayer()
    {
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 50;

        //나의 플레이어를 생성
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);

        print("나는 " + PhotonNetwork.LocalPlayer.NickName + "입니다");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //닉네임 설정
        PhotonNetwork.NickName = "김현진";
        //로비 진입 시도
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //PhotonNetwork.JoinRoom("노준학");

        RoomOptions roomOptions = new RoomOptions();
        //인원수 제한
        roomOptions.MaxPlayers = 10;
        //방의이름으로 방을 만든다
        PhotonNetwork.CreateRoom("김현진", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        CreatePlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "님이 참여하였습니다");
    }
}
