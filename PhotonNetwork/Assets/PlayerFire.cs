using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{
    //총알공장
    public GameObject bulletFactory;
    //파편효과공장
    public GameObject bulletEft;

    void Start()
    {
        
    }


    void Update()
    {
        //내것이 아니라면 리턴
        if (photonView.IsMine == false) return;
       

        //만약에 1번키를 누르면
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //서버한테 RpcFire 함수 호출 해줘, 위치, 앞방향
            photonView.RPC("RpcFire", RpcTarget.All, 
                Camera.main.transform.position, 
                Camera.main.transform.forward);
        }

        //만약에 2번키를 누르면
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Ray 로 총알을 쏜다
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //만약에 맞았다면
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                //서버한테 RpcShowBulletEft 함수 호출 해줘, 위치, normal
                photonView.RPC("RpcShowBulletEft", RpcTarget.All, hit.point, hit.normal);

                //PlayerMove 가져오자
                PlayerMove pm = hit.transform.GetComponent<PlayerMove>();
                if(pm != null)
                {
                    pm.OnDamaged(10);
                }

                //Enemy 가져오자
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.OnHit();
                }
            }
        }
    }

    [PunRPC]
    void RpcFire(Vector3 position, Vector3 forward)
    {
        //총알공장에서 총알을 만든다
        GameObject bullet = Instantiate(bulletFactory);
        //만든총알을 카메라 위치에 놓는다
        bullet.transform.position = position;
        //만든총알의 앞방향을 카메라의 앞방향으로 설정한다
        bullet.transform.forward = forward;
        //Bullet 컴포넌트 가져와서 ownerId를 설정한다
        Bullet b = bullet.GetComponent<Bullet>();
        b.ownerId = photonView.ViewID;
    }

    [PunRPC]
    void RpcShowBulletEft(Vector3 position, Vector3 normal)
    {
        //파편효과만들고
        GameObject eft = Instantiate(bulletEft);
        //만든효과를 맞은위치에 놓고
        eft.transform.position = position;
        //만든효과의 앞방향을 맞은위치의 normal방향으로
        eft.transform.forward = normal;
        //3초뒤에 파괴하자
        Destroy(eft, 3);
    }
}
