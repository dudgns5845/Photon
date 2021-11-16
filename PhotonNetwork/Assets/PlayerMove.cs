using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    //�ӷ�
    public float speed = 5;

    //ī�޶�
    public GameObject cam;

    //ObjeRotate
    public ObjRotate objRot;

    //NickName
    public Text nickName;

    //���� ü��
    float currHp = 100;
    //�ִ� ü��
    public float maxHp = 100;
    //HP UI
    public Image hpUi;

    Vector3 receivePos;
    Quaternion receiveRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���࿡ �� �� �ִ� ���¶��
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //���࿡ ���� �� �ִ� ���¶��
        if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Start()
    {
        //���࿡ �����̶��
        if(PhotonNetwork.IsMasterClient)
        {
            //EnemyManager ���ӿ�����Ʈ ã��
            GameObject enemyMgr = GameObject.Find("EnemyManger");
            //EnemyManager ������Ʈ�� Ȱ��ȭ ��Ű��
            enemyMgr.GetComponent<EnemyManger>().enabled = true;

            //�÷��̾��� ��ġ�� �Ҵ�����
            GameManager.instance.SetPlayerPos(photonView);
        }

        //�г���
        nickName.text = photonView.Owner.NickName;

        //����ü���� �ִ�ü������ ����
        currHp = maxHp;

        //���� ������ Player�� �ƴ϶��
        if(photonView.IsMine == false)
        {
            //ī�޶� ��Ȱ��ȭ
            cam.SetActive(false);
            //Player - ObjRotate ��Ȱ��ȭ
            objRot.enabled = false;
        }
        else
        {
            GameManager.instance.myPhotonView = photonView;
        }
    }

    void Update()
    {
        //���� ������ Player���
        if (photonView.IsMine)
        {
            //W,A,S,D Ű�� �Է��� ����
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //������ ���ϰ�       
            Vector3 dir = transform.right * h + transform.forward * v;
            dir.Normalize();

            //��������
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
        print(photonView.Owner.NickName + "�� HP : " + currHp);
    }

    [PunRPC]
    void RpcSetPlayerPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
