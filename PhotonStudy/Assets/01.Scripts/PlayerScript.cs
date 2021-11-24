using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerScript : MonoBehaviour
{
    public PhotonView pv;
    public SpriteRenderer SR;
    private void Update()
    {
        if (pv.IsMine)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            transform.Translate(new Vector3(axis * Time.deltaTime * 7, 0, 0));
            if (axis != 0) pv.RPC("FlipXRPC", RpcTarget.All, axis);
        }
    }

    [PunRPC]

    void FlipXRPC(float axis)
    {
        SR.flipX = axis == 1;
    }

}
