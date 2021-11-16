using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyManger : MonoBehaviour
{
    //에너미 공장
    public GameObject enemyFactory;

    IEnumerator Start()
    {
        //만약에 방장이라면
        if(PhotonNetwork.IsMasterClient)
        {
            while(true)
            {
                //PhotonNetwork.Instantiate("Enemy", transform.position, Quaternion.identity);            
                //1초 기다린다
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
