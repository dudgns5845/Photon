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

    private void Start()
    {
        UISetting();
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
        base.OnRoomListUpdate(roomList);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        print("������Ʈ ȣ��");
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
        foreach (RoomInfo info in cacheRoom.Values)
        {
            GameObject room = Instantiate(BTN_RoomInfo);
            room.transform.SetParent(roomlistContent);
        }
    }


    //�� ����Ʈ�� ���ŵɶ����� ui������Ʈ
    void UpdateCacheRoom(List<RoomInfo> roomList)
    {
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
                    continue;
                }
            }
            //4. �׷��� ������ roomInfo�� cacheRoom�� �߰� �Ǵ� ���� �Ѵ�
            cacheRoom.Add(roomList[i].Name, roomList[i]);
        }
    }
}
