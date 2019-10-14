using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enermyTag = "ENERMY";
    private float initHp = 100.0f;
    public float currHp;

    //델리게이트 및 이벤트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    
    // Start is called before the first frame update
    void Start()
    {
        currHp = initHp;
    }
    //충돌한 콜라이더의 이즈 트리거 옵션이 체크됐을때 발생
    void OnTriggerEnter(Collider collider)
    {
        //충돌한 콜라이더이ㅡ 태그가 BULLET이면 PLAYER의 currhp를 차감
        if (collider.tag == bulletTag)
        {
            Destroy(collider.gameObject);

            currHp -= 10.0f;
            Debug.Log("Player HP = " + currHp.ToString());

            //player의 생명이 0 이하면 사망처리
            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }


    //플레이어 의 사망처리 루틴
    void PlayerDie()
    {

        OnPlayerDie();
        Debug.Log("Player Die!!!");
        //에너미 태그로 지정된 모든 적 캐릭터를 추출해 배열에 저장
       // GameObject[] enermies = GameObject.FindGameObjectsWithTag(enermyTag);

       // //배열의 처음부터 순회하면서 적 캐릭터이ㅡ OnplayerDie 함수 호출
       //for ( int i = 0; i < enermies.Length; i++)
       //{
       //    enermies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
       //}    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
