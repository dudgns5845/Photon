using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //방제목
    public InputField roomName;
    //인원수
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
        //인원수 제한
        roomOptions.MaxPlayers = byte.Parse(maxPlayer.text);
        //방의이름으로 방을 만든다
        PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방생성 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방생성 실패 : " + returnCode + ", " + message);
    }

    public void OnJoin()
    {
        //방입장
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방입장 완료");
        //Play씬으로 이동
        PhotonNetwork.LoadLevel("PlayScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방입장 실패 : " + returnCode + ", " + message);


    }

    //룸리스트 정보
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //방정보 버튼 공장
    public GameObject roomInfoFactory;

    List<GameObject> roomInfoBtn;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //룸리스트  UI 삭제
        DeleteRoomList();
        //룸리스트 정보 갱신
        UpdateRoomCache(roomList);
        //룸리스트 ui갱신
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
            //방정보 버튼을 만든다
            GameObject roominfo = Instantiate(roomInfoFactory);
            //만든 버튼의 위치를 정한다
            //만든 버튼의 정보를 넣어준다
            RoomInfoButton btn = roominfo.GetComponent<RoomInfoButton>();
            btn.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            roominfoBtn.Add(roominfo);
        }

    }

    void UpdateRoomCache(List<RoomInfo> roomList)
    {

        for (int i = 0; i < roomList.Count; i++)
        {
            //만약에 방이름이 cache에 있다면
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //그방 지워졌니?
                if (roomList[i].RemovedFromList)
                {
                    //cache에서 삭제
                    roomCache.Remove(roomList[i].Name);
                    continue;

                }
            }
            //방이 새롭게 추가 되었다는것!!
            else
            {
                //cache 해당 방을 추가
                roomCache[roomList[i].Name] = roomList[i];
            }
        }

    }
}
