using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField inputRoomName;
    public InputField inputMaxPlayer;

    public Button btnCreateRoom;
    public Button btnJoinRoom;

    private void Awake()
    {
        Screen.SetResolution(800, 800, false);
    }

    private void Start()
    {
        UISetting();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinLobby();
    }

    void UISetting()
    {
        inputRoomName.onValueChanged.AddListener(OnChangedRoomName);
        //���ٽ����� ǥ���ϱ�
        //inputRoomName.onValueChanged.AddListener((string text) =>
        //{
        //    btnJoinRoom.interactable = text.Length > 0;
        //    OnChangedMaxPlayer(inputMaxPlayer.text);
        //});

        inputMaxPlayer.onValueChanged.AddListener(OnChangedMaxPlayer);
    }

    void OnChangedRoomName(string text)
    {
        btnJoinRoom.interactable = text.Length > 0;
        OnChangedMaxPlayer(inputMaxPlayer.text);
    }

    void OnChangedMaxPlayer(string text)
    {
        btnCreateRoom.interactable = btnJoinRoom.interactable && text.Length > 0;
    }


    public void OnClickCreateRoom()
    {
        //��ɼ� ==> ���� ������ �����ϴ°�
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text); // �� �ο� �� ���� 0�� ���� ����
        roomOptions.IsVisible = true; // ���� ���̰ų� �Ⱥ��̰� �� �� �ִ�.
        roomOptions.IsOpen = true; // ���� ���̳� ���� �� �ִ� �� ������ �� �ִ�.

        //�� ���� => �� �̸��� �� �ɼ��� �־��ش�
        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);

    }

    //����� �Ϸ�� ȣ��
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        print("�� ����" + PhotonNetwork.CountOfRooms);
    }

    //�� ���� ���н� ȣ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //�濡 �����ϱ�
    public void OnClickJoinRoom()
    {
        /*
         *  �濡 �����ϱ�
            PhotonNetwork.JoinRandomRoom(); //�κ� ���� �� �ƹ��ų� ���� ����
            PhotonNetwork.JoinRoom("���̸�"); // Ư�� �濡 ����
            PhotonNetwork.JoinOrCreateRoom("���̸�"); // Ư�� �濡 �����ϰų� ������ ���� �����ϰ� ����
        */

        PhotonNetwork.JoinRoom(inputRoomName.text);
        //PhotonNetwork.JoinOrCreateRoom(inputRoomName.text);
    }

    //�� ���� ���� �� ȣ��
    public override void OnJoinedRoom()
    {
        //  base.OnJoinedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.LoadLevel("SceneGameRoom");
    }

    //�� ���� ���н� ȣ��
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // base.OnJoinRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    Dictionary<string, RoomInfo> cacheRoom = new Dictionary<string, RoomInfo>();

    //�ش� �κ� �� ����� ���� ������ ������ ȣ��(�߰�,����,����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("OnRoomListUpdate ȣ��");
        base.OnRoomListUpdate(roomList);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //�븮��Ʈ UI ����
        DeleteRoomListUI();
        //��Cache�� ����
        UpdateCacheRoom(roomList);

        CreateRoomListUI();
    }

    public GameObject BTN_RoomInfo;
    public Transform roomlistContent;

    void DeleteRoomListUI()
    {
        //roomlistContent �ڽ��� �� ������
        foreach (Transform tr in roomlistContent)
        {
            Destroy(tr.gameObject);
        }
    }

    void CreateRoomListUI()
    {
        //foreach (var info in cacheRoom)
        //{
        //    GameObject room = Instantiate(BTN_RoomInfo);
        //    room.GetComponentInChildren<Text>().text = info.Key;
        //    room.transform.SetParent(roomlistContent);
        //}
        var e = cacheRoom.GetEnumerator();
        while (e.MoveNext())
        {
            GameObject room = Instantiate(BTN_RoomInfo);
            room.GetComponentInChildren<Text>().text = e.Current.Key;
            room.transform.SetParent(roomlistContent);
            room.GetComponent<RoomInfoButton>().SetInfo(e.Current.Value.Name, e.Current.Value.PlayerCount, e.Current.Value.MaxPlayers);
        }

    }


    //�� ����Ʈ�� ���ŵɶ����� ui������Ʈ
    void UpdateCacheRoom(List<RoomInfo> roomList)
    {
        if (cacheRoom == null)
        {
            cacheRoom = new Dictionary<string, RoomInfo>();
        }
        //1.roomList�� ���������� ���鼭
        for (int i = 0; i < roomList.Count; i++)
        {
            //2.�ش��̸��� cacheRoom�� key ������ ���� �Ǿ��ٸ�
            if (cacheRoom.ContainsKey(roomList[i].Name))
            {
                //3. �ش� roomInfo ����(����,����)
                //���� ������ �Ǿ��ٸ�
                if (roomList[i].RemovedFromList)
                {
                    cacheRoom.Remove(roomList[i].Name);
                }
                else
                {
                    cacheRoom[roomList[i].Name] = roomList[i];
                }
            }
            else
            {
                //4. �׷��� ������ roomInfo�� cacheRoom�� �߰� �Ǵ� ���� �Ѵ�
                cacheRoom.Add(roomList[i].Name, roomList[i]);
            }
        }
    }
}
