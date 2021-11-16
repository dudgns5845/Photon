using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    static public GameManager instance;

    //���� ����View
    public PhotonView myPhotonView;

    //�÷��̾� ���� ��ġ��
    public Transform[] playerPos;
    //���� ��ġ�ؾ��� Index
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
            //���� ���� ����
            PhotonNetwork.GameVersion = "1";
            //���� �õ� (name���� -> master����)
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

        //���� �÷��̾ ����
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
        PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);

        print("���� " + PhotonNetwork.LocalPlayer.NickName + "�Դϴ�");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //�г��� ����
        PhotonNetwork.NickName = "������";
        //�κ� ���� �õ�
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        //PhotonNetwork.JoinRoom("������");

        RoomOptions roomOptions = new RoomOptions();
        //�ο��� ����
        roomOptions.MaxPlayers = 10;
        //�����̸����� ���� �����
        PhotonNetwork.CreateRoom("������", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        CreatePlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "���� �����Ͽ����ϴ�");
    }
}
