using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //������
    public InputField roomName;
    //�ο���
    public InputField maxPlayer;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnCreate()
    {
        RoomOptions roomOptions = new RoomOptions();
        //�ο��� ����
        roomOptions.MaxPlayers = byte.Parse(maxPlayer.text);
        //�����̸����� ���� �����
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("����� �Ϸ�");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("����� ���� : " + returnCode + ", " + message);
    }

    public void OnJoin()
    {
        //������
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("������ �Ϸ�");
        //Play������ �̵�
        PhotonNetwork.LoadLevel("PlayScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("������ ���� : " + returnCode + ", " + message);


    }

    //�븮��Ʈ ����
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //������ ��ư ����
    public GameObject roomInfoFactory;

    List<GameObject> roomInfoBtn;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //�븮��Ʈ  UI ����
        DeleteRoomList();
        //�븮��Ʈ ���� ����
        UpdateRoomCache(roomList);
        //�븮��Ʈ ui����
        CreateRoomList();

    }
     
    void DeleteRoomList()
    {
        for(int i = 0; i < roomInfoBtn.Count; i++)
        {
            Destroy(roomInfoBtn[i]);
        }
        roomInfoBtn.Clear();
    }
    void CreateRoomList()
    {
        foreach(RoomInfo info in roomCache.Values)
        {
            //������ ��ư�� �����
            GameObject roominfo = Instantiate(roomInfoFactory);
            //���� ��ư�� ��ġ�� ���Ѵ�
            //���� ��ư�� ������ �־��ش�
            RoomInfoButton btn = roominfo.GetComponent<RoomInfoButton>();
            btn.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            roominfoBtn.Add(roominfo);
        }

    }

    void UpdateRoomCache(List<RoomInfo> roomList)
    {

        for (int i = 0; i < roomList.Count; i++)
        {
            //���࿡ ���̸��� cache�� �ִٸ�
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //�׹� ��������?
                if (roomList[i].RemovedFromList)
                {
                    //cache���� ����
                    roomCache.Remove(roomList[i].Name);
                    continue;

                }
            }
            //���� ���Ӱ� �߰� �Ǿ��ٴ°�!!
            else
            {
                //cache �ش� ���� �߰�
                roomCache[roomList[i].Name] = roomList[i];
            }
        }

    }
}
