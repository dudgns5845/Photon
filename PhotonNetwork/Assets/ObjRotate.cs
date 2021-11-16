using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    //����
    Vector3 rot;

    //ȸ�����ɿ���
    public bool canH;
    public bool canV;

    //ȸ�� �ӷ�
    public float rotSpeed = 200;

    void Start()
    {
        rot = transform.localEulerAngles;
    }

    void Update()
    {
        //���콺�� �������� ����
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //������ ������Ű��
        if(canV) rot.x += -my * rotSpeed * Time.deltaTime;
        if(canH) rot.y += mx * rotSpeed * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, -60, 60);

        //������ ������ �����Ű��
        transform.localEulerAngles = rot;
    }
}
