using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알 발사와재장전 오디오 클립을 저장할 구조체,
[System.Serializable]
public struct PlayerSfx      //인스펙터 뷰에 있는 PlayerSfx 항목, fire , reload 배열 생성.    
{
    public AudioClip[] fire;
    public AudioClip[] reload;

} //AudioClip 저장할 오디오 파일

public class FireCtrl : MonoBehaviour
{
    //무기타입 
    public enum WeaponType    //착용중인 무기의 종류를 콤보박스 형태로 설정가능
    {
        RiFLE = 0 ,
        SHOTGUN
    }

    //현재 주인공이 들고있는 무기를 저장할 변수
    public WeaponType currWeapon = WeaponType.RiFLE;  
    
    private Vector3 originPos;

    [SerializeField]
    //총알 프리팹
    public GameObject bullet;
    public Transform firePos;
    
    public ParticleSystem cartridge;
    private ParticleSystem muzzleFlash;


    //AudioSource오디오 콤포넌트를 저장할 변수 
    private AudioSource audio;
    //오디오 클립을 저장할 변수
    public PlayerSfx playerSfx;

    public float shootCount = 0f;
    public float delayTimer = 0f;
    public float shootDelay = 1f;
    //반동세기
    public float retroActionForce;



    //셰이크를 저장할 함수
    private Shake shake;    


    // Start is called before the first frame update
    void Start()
    {
        

        //FirePos 하위에 있는 컴포넌트 추출 , 스스로 FirePos의 <ParticleSystem>를 추출.
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();

        originPos = Vector3.zero;
        //Audio콤포넌트 추출, 
        audio = GetComponent<AudioSource>();
        //Shake 스크립트 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && delayTimer > shootDelay)
        {
            Fire();           
            ++shootCount;
            delayTimer = 0f;
        }
    }

    void Fire()
    {
        //Shake효과 추출
        //StartCoroutine(shake.ShakeCamera(0.001f,0.007f,0.008f));

        //Bullet 프로필 동적으로 생성  
        Instantiate(bullet, firePos.position, firePos.rotation);
        cartridge.Play();
        //<ParticleSystem> 중 muzzleFlash 실행.
        muzzleFlash.Play();
        //StartCoroutine(RetroAction());
        //발사 함수가 실행 될때 마다 총기 사운드를 발생시키는 함수 실행.
        FireSfx();
        //총기반동

    }

    //IEnumerator RetroAction()
    //{
    //    Vector3 recoilBack = new Vector3(currGun.retroActionForce, originPos.y,originPos.z);

    //    currGun.transform.localPosition = originPos;

    //    while(currGun.transform.localPosition.x 
    //        <= currGun.retroActionForce - 0.03f)
    //    {
    //        currGun.transform.localPosition =
    //            Vector3.Lerp(currGun.transform.localPosition, recoilBack, 0.35f);
    //        yield return null;
    //    }

    //    while ( currGun.transform.localPosition != originPos)
    //    {
    //        currGun.transform.localPosition =
    //            Vector3.Lerp(currGun.transform.localPosition, originPos, 0.1f);
    //        yield return null;
    //    }
    //}

    void FireSfx()
    {
        //현재 들고있는 무기의 오디오 클립을 가져오기. [int] << 착용중인 무기 배열 0 = 라이플, 에 맞춰 그 배열에 속한 사운드 출력
        var _sfx = playerSfx.fire[(int)currWeapon];
        //사운드 발생 ,사운드스케일  = 0.0f ~ 1.0f
        audio.PlayOneShot(_sfx, 2.0f);
    }

}
