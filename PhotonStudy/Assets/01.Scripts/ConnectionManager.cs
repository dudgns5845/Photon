using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //아이디 입력
    public InputField inputID;
    // 게임버전
    public string gameVersion = "1";

    // 버튼이 클릭 되었을때
    public void OnClickConnect()
    {
        //예외 처리
        if (inputID.text.Length == 0) {
            print("ID가 비었습니다.");    
            return;
        } 

        //ID에 값이 있으면
        //포톤 기본 셋팅을 하고 (게임 버전 셋팅)
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = false; //마스터가 씬 전환시 자동 다른 유저들도 전환 가능
        //접속시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //네임서버에 접속
    public override void OnConnected()
    {
        base.OnConnected();
        //시스템상의 현재 실행중인 함수의 이름 호출하기
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //마스터에 접속 ==> 로비에 들어갈 수 있는 상태
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //닉네임 설정
        PhotonNetwork.NickName = inputID.text;
        //로비로 진입
        //아무 설정없으면 기본 로비로 접속
        PhotonNetwork.JoinLobby(); 
        //내가 들어가고 싶은 특정 로비 ==> PhotonNetwork.JoinLobby(new TypedLobby("로비이름",LobbyType.Default))
        //PhotonNetwork.JoinLobby(new TypedLobby("Rio_Room",LobbyType.Default));
        //PhotonNetwork.JoinLobby(new TypedLobby("RioWorld",LobbyType.Default));

    }

    //로비에 접속이 성공하면 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        print("로비 인원" + PhotonNetwork.CountOfPlayers);
        //로비 씬으로 이동  ==> 씬 전환할때 동기화 때문에 SceneManager.LoadScene()으로 하면 안된다.
        PhotonNetwork.LoadLevel("SceneLobby");
    }
}
