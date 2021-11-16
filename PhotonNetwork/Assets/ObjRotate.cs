using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    //각도
    Vector3 rot;

    //회전가능여부
    public bool canH;
    public bool canV;

    //회전 속력
    public float rotSpeed = 200;

    void Start()
    {
        rot = transform.localEulerAngles;
    }

    void Update()
    {
        //마우스의 움직임을 받자
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //각도를 누적시키자
        if(canV) rot.x += -my * rotSpeed * Time.deltaTime;
        if(canH) rot.y += mx * rotSpeed * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, -60, 60);

        //누적된 각도를 적용시키자
        transform.localEulerAngles = rot;
    }
}
