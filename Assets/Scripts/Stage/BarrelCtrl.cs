using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    //폭발효과 프리팹을 저장할 변수
    public GameObject expEffect;
    //찌그러진 드럼통의 메쉬를 저장할 배열
    public Mesh[] meshes;
    //드럼통의 텍스쳐를 저장할 배열
    public Texture[] textures;

    //총알이 맞은 횟수
    private int hitcount = 0;
    //Rigidbody 컴포넌트를 저장할 변수
    private Rigidbody rb;
    //meshFilter 컴포넌트를 저장할 변수
    private MeshFilter meshFilter;

    //MeshRender 컴포넌트를 저장할 변수
    private MeshRenderer renderer;
    //Audio 컴포넌트 저장할 변수
    private AudioSource audio;

    //폭발반경
    public float expRadius = 10.0f;
    //폭발음 오디오 클립
    public AudioClip expSfx;
    //Shake 클래스를 저장할 함수
    public Shake shake;

    

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody 컴포넌트를 추출 후 저장
        rb = GetComponent<Rigidbody>();
        //meshFilter 컴포넌트를 추출 후 저장
        meshFilter = GetComponent<MeshFilter>();

        //MeshRenderer 컴포넌트를 추출 후 저장.
        renderer = GetComponent<MeshRenderer>();

        //난수를 발생시켜 불규칙적인 텍스쳐를 적용. 
        renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];

        //Audio 컴포넌트를 추출해 저장
        audio = GetComponent<AudioSource>();

        //Shake 스크립트를 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    //충돌이 발생했을때 한번 호출되는 콜 백 함수.
    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 게임 오브젝트의 태그 비교
        if (collision.collider.CompareTag("BULLET"))
        {
            // 충돌한 횟수를 증가시키고  20발 이상 맞았는지 확인.
            if (++hitcount == 7)
            {
                ExpEffect();                      
            }

        }
    }

    //폭발 효과를 처리 할 함수,
    void ExpEffect()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);
        //Rigidibody 컴포넌트의 mass를 1.0로 수정해 가볍게 만듦
        rb.mass = 1.2f;
        
        
        //위로 솟구치는 힘을 가함
        rb.AddForce(Vector3.up * 1000.0f);

        //폭발력 생성
        IndirectDamage(transform.position);
        

        //난수를 발생
        int idx = Random.Range(0, meshes.Length);
        //찌그러진 메쉬를 적용
        meshFilter.sharedMesh = meshes[idx];
        //콜라이더 변경
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        //폭발음 발생
        audio.PlayOneShot(expSfx, 1.0f);
        //Shake효과 호출
        StartCoroutine(shake.ShakeCamera(0.3f, 0.4f, 0.2f));

        //폭발력을 전달하는 함수
        void IndirectDamage(Vector3 pos)
        {
            //주변에 있는 모든 드럼통 추출   Physics.OverlapSphere(위치 , 반경 , 대상 레이어) ;     //십진수 1 << 9  , 비교를 할땐 연산자 두개를, 연산할땐 연산자 하나를.
            Collider[] colliders = Physics.OverlapSphere(pos, expRadius, 1 << 9);

            foreach (var collision in colliders) 
        {
            //폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트 추출
            var _rb = collision.GetComponent<Rigidbody>();
                //드럼통의 무게를 가볍게 함
                _rb.mass = 1.2f;
                //폭발력 전달
                _rb.AddExplosionForce(1200.0f, pos, 1000.0f);
        }
    }
    // Update is called once per frame
    void Update()
        {

        }
    }
}
