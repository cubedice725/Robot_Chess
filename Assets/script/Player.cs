using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        // ���� ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray�� ���� ��ü�� �ִ��� �˻�
            if (Physics.Raycast(ray, out hit, 100f))
            {
                print(hit.transform.name);
            }
        }

    }
    public void PlayerMove()
    {

    }
}
