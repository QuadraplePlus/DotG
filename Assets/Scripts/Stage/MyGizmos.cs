using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.yellow;
    public float radius = 0.1f;

    private void OnDrawGizmos()
    {
        //기즈모 색상설정
        Gizmos.color = color;
        //구체 모양의 기즈모 생성 . (인자는 생성위치, 반지름)
        Gizmos.DrawSphere(transform.position, radius);
    }
}
