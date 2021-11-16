using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPun
{
    //�Ѿ˰���
    public GameObject bulletFactory;
    //����ȿ������
    public GameObject bulletEft;

    void Start()
    {
        
    }


    void Update()
    {
        //������ �ƴ϶�� ����
        if (photonView.IsMine == false) return;
       

        //���࿡ 1��Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //�������� RpcFire �Լ� ȣ�� ����, ��ġ, �չ���
            photonView.RPC("RpcFire", RpcTarget.All, 
                Camera.main.transform.position, 
                Camera.main.transform.forward);
        }

        //���࿡ 2��Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Ray �� �Ѿ��� ���
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //���࿡ �¾Ҵٸ�
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                //�������� RpcShowBulletEft �Լ� ȣ�� ����, ��ġ, normal
                photonView.RPC("RpcShowBulletEft", RpcTarget.All, hit.point, hit.normal);

                //PlayerMove ��������
                PlayerMove pm = hit.transform.GetComponent<PlayerMove>();
                if(pm != null)
                {
                    pm.OnDamaged(10);
                }

                //Enemy ��������
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
        //�Ѿ˰��忡�� �Ѿ��� �����
        GameObject bullet = Instantiate(bulletFactory);
        //�����Ѿ��� ī�޶� ��ġ�� ���´�
        bullet.transform.position = position;
        //�����Ѿ��� �չ����� ī�޶��� �չ������� �����Ѵ�
        bullet.transform.forward = forward;
        //Bullet ������Ʈ �����ͼ� ownerId�� �����Ѵ�
        Bullet b = bullet.GetComponent<Bullet>();
        b.ownerId = photonView.ViewID;
    }

    [PunRPC]
    void RpcShowBulletEft(Vector3 position, Vector3 normal)
    {
        //����ȿ�������
        GameObject eft = Instantiate(bulletEft);
        //����ȿ���� ������ġ�� ����
        eft.transform.position = position;
        //����ȿ���� �չ����� ������ġ�� normal��������
        eft.transform.forward = normal;
        //3�ʵڿ� �ı�����
        Destroy(eft, 3);
    }
}
