using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    ////오디오 컴포넌트 추출
    public AudioSource audio;
    ////타격음 오디오 클립.
    public AudioClip metalHitSfx;


    //스파크 효과 프리팹 저장할 함수
    public GameObject sparkEffect;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    //충돌이 시작할 때 발생하는 이벤트
    private void OnCollisionEnter(Collision collision)
    {

        
        //충돌한 게임 오브젝트의 태그값 비교
        if (collision.collider.tag == "BULLET")
        {
            //스파크 효과 함수 추출
            ShowEffect(collision);
            //충돌한 게임 오브젝트 삭제 , 게임 오브젝트 뒤에 숫자입력시. 지연가능
            Destroy(collision.gameObject);
            audio.PlayOneShot(metalHitSfx, 0.2f);
        }
    }

    

    void ShowEffect(Collision collision)
    {
        //충돌지점의 정보 추출
        ContactPoint contact = collision.contacts[0];
        //법선 벡터가 이루는 회전각도를 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //스파크 효과를 생성
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rot);
        //스파크 효과의 부모를 드럼통 또는 벽으로 설정
        spark.transform.SetParent(this.transform);
    }
}
