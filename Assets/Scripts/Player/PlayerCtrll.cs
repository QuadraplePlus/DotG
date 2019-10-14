using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//클래스는 system.serializable 이라는 어트리뷰트를 명시해야 인스펙터뷰에 노출됨.
[System.Serializable]
public class Playeranim
{
    public AnimationClip Idle;
    public AnimationClip RunF;
    public AnimationClip RunB;
    public AnimationClip RunL;
    public AnimationClip RunR;
}
public class PlayerCtrll : MonoBehaviour
    //키보드 입력값을 받아오는 로직
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;
    //접근해야하는 컴포넌트는 반드시 변수에 할당 한 후 사용.
    private Transform tr;
    //이동속도변수
    [SerializeField]
    private float moveSpeed = 10.0f;
    //회전속도변수
    private float rotSpeed = 80.0f;

    //인스펙터 뷰에 표시할 애니메이션 클래스 변수
    public Playeranim playerAnim;
    //애니메이션 컴포넌트를 저장하기 위한 변수
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    { 
        //처음 수행되는 스타트 함수에서 트랜스폼 컴포넌트를 할당.
        tr = GetComponent<Transform>();

        //애니매이션 컴포넌트를 변수에 할당
        anim = GetComponent<Animation>();
        //애니매이션 컴포넌트의 애니메이션 클립을 지정하고 실행
        anim.clip = playerAnim.Idle;
        anim.Play();


    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        //Debug.Log("h =" + h.ToString());
        //Debug.Log("v =" + v.ToString());

        //전후좌우 이동방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        

        //Translate(이동방향 * 속도 *변위값 *Time.deltaTime, 기준좌표)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        //vector3.up 축을 기준으로 rotSpeed 만큼의 속도로 회전
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //키보드입력값을 기준으로 동작할 애니메이션 수행
        if (v >= 0.1f)
        {
            anim.CrossFade(playerAnim.RunF.name, 0.3f);
        }

        else if ( v <= -0.1f)
        {
            anim.CrossFade(playerAnim.RunB.name, 0.3f);
        }   
        
        else if ( h >= 0.1f)
        {
            anim.CrossFade(playerAnim.RunL.name, 0.3f);
        }

        else if ( h <= -0.1f)
        {
            anim.CrossFade(playerAnim.RunR.name, 0.3f);
        }
        else
        {
            anim.CrossFade(playerAnim.Idle.name, 0.3f);
        }
    }
}
