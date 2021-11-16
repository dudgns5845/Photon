using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    //총알속력
    public float speed = 10;

    //ownerId
    public int ownerId;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //부딪힌놈에 PhotonView 를 가져온다
        PhotonView pv = other.GetComponent<PhotonView>();
        //만약에 가져왔으면
        if(pv != null)
        {
            print(pv.ViewID);
            //만약에 ownerId 와 가져온 PhotonView의 id가 같지않다면
            if(pv.ViewID != ownerId)
            {
                //만약에 방장이라면
                if(PhotonNetwork.IsMasterClient)
                {
                    //PlayerMove 컴포넌트 가져오자
                    PlayerMove pm = other.GetComponent<PlayerMove>();
                    //가져왔다면 OnDamaged 실행
                    if(pm != null)
                    {
                        pm.OnDamaged(5);
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
