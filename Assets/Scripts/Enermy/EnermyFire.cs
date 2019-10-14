using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyFire : MonoBehaviour
{
    //Audio 컴포넌트를 저장할 변수
    private AudioSource audio;
    //Animator컴포넌트를 저장할 변수
    private Animator animator;
    //주인공 캐릭터의 Transform 컴포넌트
    private Transform playerTr;
    //적 캐릭터의 Trasform 컴포넌트
    private Transform enermyTr;
    //
    private Transform EFirePos;

    //애니메이터 컨트롤러에 정희한 파라미터 해시값을 미리 추출
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    //다음 발사할 시간 계산용 변수
    private float nextFire = 0.0f;
    //총알 발사 간격
    private readonly float fireRate = 0.1f;
    //주인공을 향해 회전할 속도 계수
    private readonly float damping = 10.0f;

    //재장전 시간
    private readonly float reloadTime = 2.0f;

    //탄창의 최대 총알 수
    private readonly int maxBullet = 30;
    //초기 총알 수
    private int currBullet = 30;

    //재장전 여부
    private bool isReload = false;
    //재장전 시간 동안 기다릴 변수 선언
    private WaitForSeconds wsReload;

    //총알 발사 ㅇ ㅕ부를 판단할 변수
    public bool isFire = false;
    //총알 발사 사운드를 저장할 변수
    public AudioClip fireSfx;
    //재자언 사운드를 저장할 변수
    public AudioClip reloadSfx;

    //public ParticleSystem muzzle;

    //적캐릭터 총알 프리펩
    public GameObject Bullet;
    public Transform firePos;

    //MuzzleFlash 의 MeshRenderer 컴포넌트를 저장할 변수
    public MeshRenderer muzzleFlash;


    // Start is called before the first frame update
    void Start()
    {
        
        //컴포넌트 추출 및 저장
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enermyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

        muzzleFlash.enabled = false;

        //muzzle = EFirePos.GetComponentInChildren<ParticleSystem>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReload && isFire)
        {
            //현재 시간ㄴ이 다음 발사 시간보다 큰지를 확인
            if (Time.time >= nextFire)
            {
                Fire();
                //다음 발사 시간 계산
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.15f);
            }

            //주인공이 있는 위치까지의 회전각도 계산
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enermyTr.position);
            //보간 함수를 사용해 점진적으로 회전
            enermyTr.rotation = Quaternion.Slerp(enermyTr.rotation, rot, Time.deltaTime * damping);
        }

    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 0.2f);
        //muzzle.Play();
        //총구 화염 효과 코루틴 호출
        StartCoroutine(ShowMuzzleFlash());

        //총알 생성
        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        //일정 시간이 지난 후 삭제
        Destroy(_bullet, 3.0f);
        //남은 초 ㅇ알로 재장전 여부를 계산
        isReload = (--currBullet % maxBullet == 0);
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash활성화
        muzzleFlash.enabled = true;

        //불규칙한 회전 각도 계산
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        //MuzzleFlash z축으로 회전
        muzzleFlash.transform.localRotation = rot;
        ///muzzleFlash 의 스케일을 불규칙하게 조정
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(0.5f, 0.8f);

        //텍스쳐의 Offset속성에 적용할 불규칙한 값을 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //muzzelFlash 의 머터리얼의 Offset의 갑을 적용
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        //muzzleFalsh 가 보일동안 잠시 대기
        yield return new WaitForSeconds(Random.Range(0.00005f, 0.0002f));
        //muzzleFlash 비홠ㅇ화
        muzzleFlash.enabled = false;    
    }


    IEnumerator Reloading()
    {

        //muzzleFlash 비활성화
        muzzleFlash.enabled = false;
        //재장전 애니메이션 실행
        animator.SetTrigger(hashReload);
        //재장전 사운드 발생
        audio.PlayOneShot(reloadSfx, 1.0f);

        //재장전 시간만큼 대기하는 동안 제어권 양보
        yield return wsReload;
        //총알의 개수 초기화
        currBullet = maxBullet;
        isReload = false;
    }
}


