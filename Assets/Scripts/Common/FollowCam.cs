using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target; //추적할 대상
    public float moveDamping = 15.0f; // 이동속도 계수
    public float rotateDamping = 10.0f; //회전속도 계수
    public float distance = 5.0f; //추적 대상간의 거리
    public float height = 4.0f; //추적 대상과의 높이
    public float targetOffset = 2.0f; //추적 좌표의 오프셋
    private float x = 0.0f;
    private float y = 0.0f;
    //cameraRig 의 Transform  컴포넌트
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        //CameraRig 의 컴포넌트 추출
        tr = GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked; //커서 고정

        Vector3 angles = transform.eulerAngles;

        x = angles.y;

        y = angles.x;
    }

    //주인공이 이동 로직 완료된 후 처리하기 위해 LateUpdate 사용
    void LateUpdate()
    {
        //카메라의 높이와 거리를 계산/
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        //이동할때의 속도 계수를 적용
        tr.position = Vector3.Slerp(tr.position, camPos, moveDamping);
        //회전 할 때의 속도 계수를 적용
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);

        //카메라를 추적 대상으로 z축 회전시킴
        tr.LookAt(target.position + (target.up * targetOffset));
        
    }
    //추적할 좌표를 시각적을 표현
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //추적 및 시야를 맞출 위ㅣ를 표시
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);
        //메인 카메라와 추적지점 간의 선을 표시
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
    // Update is called once per frame
    void Update()
    {

    }
}

    


