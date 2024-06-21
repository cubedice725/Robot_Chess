using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        // 왼쪽 마우스 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // 메인 카메라를 통해 마우스 클릭한 곳의 ray 정보를 가져옴
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray와 닿은 물체가 있는지 검사
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
