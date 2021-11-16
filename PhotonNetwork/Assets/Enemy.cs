using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun, IPunObservable
{
    //Nav Mesh Agent
    NavMeshAgent navi;

    Vector3 receivePos;
    Quaternion receiveRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        if(stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Start()
    {
        //���࿡ �����̶�� == isMine
        if(PhotonNetwork.IsMasterClient)
        {
            navi = GetComponent<NavMeshAgent>();
            navi.enabled = true;
            navi.SetDestination(Vector3.zero);
        }
    }

    void Update()
    {
        //���࿡ ������ �ƴ϶��
        if(PhotonNetwork.IsMasterClient == false)
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 0.2f);
        }
    }

    public void OnHit()
    {
        photonView.RPC("RpcOnHit", RpcTarget.All);
    }

    [PunRPC]
    void RpcOnHit()
    {
        Destroy(gameObject);
    }
}
