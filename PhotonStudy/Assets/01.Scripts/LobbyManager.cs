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
        //람다식으로 표현하기
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
        //방옵션 ==> 방의 설정을 결정하는것
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text); // 방 인원 수 제한 0은 제한 없음
        roomOptions.IsVisible = true; // 방을 보이거나 안보이게 할 수 있다.
        roomOptions.IsOpen = true; // 방은 보이나 들어올 수 있는 걸 설정할 수 있다.

        //방 생성 => 방 이름과 방 옵션을 넣어준다
        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);

    }

    //방생성 완료시 호출
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        print("방 개수" + PhotonNetwork.CountOfRooms);
    }

    //방 생성 실패시 호출
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //방에 접속하기
    public void OnClickJoinRoom()
    {
        /*
         *  방에 접속하기
            PhotonNetwork.JoinRandomRoom(); //로비 안의 방 아무거나 랜덤 접속
            PhotonNetwork.JoinRoom("방이름"); // 특정 방에 접속
            PhotonNetwork.JoinOrCreateRoom("방이름"); // 특정 방에 접속하거나 없으면 방을 생성하고 접속
        */

        PhotonNetwork.JoinRoom(inputRoomName.text);
        //PhotonNetwork.JoinOrCreateRoom(inputRoomName.text);
    }

    //룸 접속 성공 시 호출
    public override void OnJoinedRoom()
    {
        //  base.OnJoinedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //룸 접속 실패시 호출
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // base.OnJoinRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    Dictionary<string, RoomInfo> cacheRoom = new Dictionary<string, RoomInfo>();

    //해당 로비에 방 목록의 변경 사항이 있으면 호출(추가,삭제,참가)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        print("업데이트 호출");
        //룸리스트 UI 삭제
        DeleteRoomListUI();
        //룸Cache를 갱신
        UpdateCacheRoom(roomList);

        CreateRoomListUI();
    }

    public GameObject BTN_RoomInfo;
    public Transform roomlistContent;

    void DeleteRoomListUI()
    {
        //roomlistContent 자식을 다 지우자
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


    //룸 리스트가 갱신될때마다 ui업데이트
    void UpdateCacheRoom(List<RoomInfo> roomList)
    {
        //1.roomList를 순차적으로 돌면서
        for (int i = 0; i < roomList.Count; i++)
        {
            //2.해당이름이 cacheRoom에 key 값으로 설정 되었다면
            if (cacheRoom.ContainsKey(roomList[i].Name))
            {
                //3. 해당 roomInfo 갱신(변경,삭제)
                //만약 삭제가 되었다면
                if (roomList[i].RemovedFromList)
                {
                    cacheRoom.Remove(roomList[i].Name);
                    continue;
                }
            }
            //4. 그렇지 않으면 roomInfo를 cacheRoom에 추가 또는 변경 한다
            cacheRoom.Add(roomList[i].Name, roomList[i]);
        }
    }
}
