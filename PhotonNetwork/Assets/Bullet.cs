using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    //�Ѿ˼ӷ�
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
        //�ε����� PhotonView �� �����´�
        PhotonView pv = other.GetComponent<PhotonView>();
        //���࿡ ����������
        if(pv != null)
        {
            print(pv.ViewID);
            //���࿡ ownerId �� ������ PhotonView�� id�� �����ʴٸ�
            if(pv.ViewID != ownerId)
            {
                //���࿡ �����̶��
                if(PhotonNetwork.IsMasterClient)
                {
                    //PlayerMove ������Ʈ ��������
                    PlayerMove pm = other.GetComponent<PlayerMove>();
                    //�����Դٸ� OnDamaged ����
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
