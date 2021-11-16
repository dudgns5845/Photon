using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    //���̵� �Է�
    public InputField inputID;
    // ���ӹ���
    public string gameVersion = "1";

    // ��ư�� Ŭ�� �Ǿ�����
    public void OnClickConnect()
    {
        //���� ó��
        if (inputID.text.Length == 0) {
            print("ID�� ������ϴ�.");    
            return;
        } 

        //ID�� ���� ������
        //���� �⺻ ������ �ϰ� (���� ���� ����)
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = false; //�����Ͱ� �� ��ȯ�� �ڵ� �ٸ� �����鵵 ��ȯ ����
        //���ӽõ�
        PhotonNetwork.ConnectUsingSettings();
    }

    //���Ӽ����� ����
    public override void OnConnected()
    {
        base.OnConnected();
        //�ý��ۻ��� ���� �������� �Լ��� �̸� ȣ���ϱ�
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //�����Ϳ� ���� ==> �κ� �� �� �ִ� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        //�г��� ����
        PhotonNetwork.NickName = inputID.text;
        //�κ�� ����
        //�ƹ� ���������� �⺻ �κ�� ����
        PhotonNetwork.JoinLobby(); 
        //���� ���� ���� Ư�� �κ� ==> PhotonNetwork.JoinLobby(new TypedLobby("�κ��̸�",LobbyType.Default))
        //PhotonNetwork.JoinLobby(new TypedLobby("Rio_Room",LobbyType.Default));
        //PhotonNetwork.JoinLobby(new TypedLobby("RioWorld",LobbyType.Default));

    }

    //�κ� ������ �����ϸ� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        print("�κ� �ο�" + PhotonNetwork.CountOfPlayers);
        //�κ� ������ �̵�  ==> �� ��ȯ�Ҷ� ����ȭ ������ SceneManager.LoadScene()���� �ϸ� �ȵȴ�.
        PhotonNetwork.LoadLevel("SceneLobby");
    }
}
