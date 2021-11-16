using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    //속력
    public float speed = 5;

    //카메라
    public GameObject cam;

    //ObjeRotate
    public ObjRotate objRot;

    //NickName
    public Text nickName;

    //현재 체력
    float currHp = 100;
    //최대 체력
    public float maxHp = 100;
    //HP UI
    public Image hpUi;

    Vector3 receivePos;
    Quaternion receiveRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //만약에 읽을 수 있는 상태라면
        if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Start()
    {
        //만약에 방장이라면
        if(PhotonNetwork.IsMasterClient)
        {
            //EnemyManager 게임오브젝트 찾자
            GameObject enemyMgr = GameObject.Find("EnemyManger");
            //EnemyManager 컴포넌트를 활성화 시키자
            enemyMgr.GetComponent<EnemyManger>().enabled = true;

            //플레이어의 위치를 할당하자
            GameManager.instance.SetPlayerPos(photonView);
        }

        //닉네임
        nickName.text = photonView.Owner.NickName;

        //현재체력을 최대체력으로 설정
        currHp = maxHp;

        //내가 생성한 Player가 아니라면
        if(photonView.IsMine == false)
        {
            //카메라 비활성화
            cam.SetActive(false);
            //Player - ObjRotate 비활성화
            objRot.enabled = false;
        }
        else
        {
            GameManager.instance.myPhotonView = photonView;
        }
    }

    void Update()
    {
        //내가 생성한 Player라면
        if (photonView.IsMine)
        {
            //W,A,S,D 키로 입력을 받자
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //방향을 정하고       
            Vector3 dir = transform.right * h + transform.forward * v;
            dir.Normalize();

            //움직이자
            transform.position += dir * speed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 0.2f);
        }
    }

    public void OnDamaged(float damage)
    {
        photonView.RPC("RpcOnDamaged", RpcTarget.All, damage);
    }

    [PunRPC]
    void RpcOnDamaged(float damage)
    {
        currHp -= damage;
        hpUi.fillAmount = currHp / maxHp;
        print(photonView.Owner.NickName + "의 HP : " + currHp);
    }

    [PunRPC]
    void RpcSetPlayerPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
