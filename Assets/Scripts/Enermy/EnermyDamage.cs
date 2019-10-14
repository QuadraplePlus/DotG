using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string EnermyBulletTag = "E_BULLET";
    //생명게이지
    private float hp = 100.0f;
    //피격시 사용할 혈흔 효과
    private GameObject bloodEffect;


    // Start is called before the first frame update
    void Start()
    {
        //혈흔 효과 프리팹을 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            //혈흔 효과를 생성하는 함수 호출
            ShowBloodEffect(collision);
            //총알삭제
            Destroy(collision.gameObject);


            //생명 게이지 차감
            
            hp -= collision.gameObject.GetComponent<BulletCtrll>().damage;
            if (hp <= 0.0f)
            {
                GetComponent<EnermyAI>().state = EnermyAI.State.DIE;
            }
        }

        else if (collision.collider.tag == EnermyBulletTag)
        {
            Destroy(collision.gameObject);
        }
    }

    //혈흔 효과를 생성하는 함수
    void ShowBloodEffect(Collision collision)
    {
        //총알이 충돌한 지점 산출
        Vector3 pos = collision.contacts[0].point;
        //총알이 충돌했을때의 법선 벡터
        Vector3 _normal = collision.contacts[0].normal;
        //총알 충돌시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);

        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
