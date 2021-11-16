using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyManger : MonoBehaviour
{
    //���ʹ� ����
    public GameObject enemyFactory;

    IEnumerator Start()
    {
        //���࿡ �����̶��
        if(PhotonNetwork.IsMasterClient)
        {
            while(true)
            {
                //PhotonNetwork.Instantiate("Enemy", transform.position, Quaternion.identity);            
                //1�� ��ٸ���
                yield return new WaitForSeconds(2);
            }
        }
        else
        {
            gameObject.SetActive(false);
            yield return null;
        }
    }

    void Update()
    {
        if(PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.Space))
        {
            PhotonNetwork.Instantiate("Enemy", transform.position, Quaternion.identity);
        }
    }
}
