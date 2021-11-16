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
        //만약에 방장이라면 == isMine
        if(PhotonNetwork.IsMasterClient)
        {
            navi = GetComponent<NavMeshAgent>();
            navi.enabled = true;
            navi.SetDestination(Vector3.zero);
        }
    }

    void Update()
    {
        //만약에 방장이 아니라면
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
