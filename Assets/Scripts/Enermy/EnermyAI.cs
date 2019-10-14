using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnermyAI : MonoBehaviour
{

    //적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL, TRACE, ATTACK, DIE
    }

    //상태를 저장할 변수
    public State state = State.PATROL;

    //Animator 컴포넌트를 저장할 변수
    private Animator animator;

    //주인공의 위치를 저장할 변수
    private Transform playerTr;

    //적 캐릭터의 위치를 저장할 변수
    private Transform enermyTr;

    //공격 사정거리 
    public float attackDist = 5.0f;
    //추적 사정거리
    public float traceDist = 10.0f;

    //사망 여부를 판단할 변수
    public bool isDie = false;

    //코루틴에서 사용할 지연시간 변수
        
    private WaitForSeconds ws;

    //이동을 제어하는 MoveAgent 클래스를 저장할 변수
    private MoveAgent moveAgent;

    //총알 발사를 제어하는 EnermyFire 클래스를 저장할 변수
    private EnermyFire enermyFire;

    //애니메이터 컨트롤러에 정희한 파라미터의 해시값을 미리 추추
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    private EnermyDamage enermyDamage;

    void Awake()
    {
        //주인공 게임 오브젝트 추출
        var _player = GameObject.FindGameObjectWithTag("PLAYER");

        //주인공의 Transform 컴포넌트 추출
        if (_player != null)
        {
            playerTr = _player.GetComponent<Transform>();
        }

        enermyTr = GetComponent<Transform>();
        //이동을 제어하는 MoveAgent 클래스를 추출
        
        moveAgent = GetComponent<MoveAgent>();

        //Animator 컴포넌트추출
        animator = GetComponent<Animator>();

        //총알 발사를 제어하는Enermy 클래스를 추출
        enermyFire = GetComponent<EnermyFire>();

        //코루틴 지연시간 생성
        ws = new WaitForSeconds(0.3f);

        //Cycle Offset 값을 불규칙하게 변경
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        //Speed값을 랜덤하게 변경
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
        
        
    }

    void OnEnable()
    {
        //CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
        //Action 코루틴 함수 실행
        StartCoroutine(Action());

        Damage.OnPlayerDie += this.OnPlayerDie;
    }

    void OnDisable()
    {
        //event : 객체외에서 무언가를 알리기 위해서 씀
        Damage.OnPlayerDie -= this.OnPlayerDie;
    }

    //적 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState()
    {
        //적 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie)
        {
            //상태가 사망이면 코루틴 함수를 종료시킴
            if (state == State.DIE) yield break;

            //주인공과 적 캐릭터간의 거리를 계산
            float dist = Vector3.Distance(playerTr.position, enermyTr.position);

            //공격이 사정거리 이내인 경우
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }

            //추적 사정거리 이내인 경ㅇ
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            //0.3초간 대기하는 동안 제어권 양보
            yield return ws;
        }
    }

    //상태에 따라적 캐릭터 행동을 처리하는 코루틴 함수
    IEnumerator Action()
    {
        //적 캐릭터가 사망할 때 까지 무한루프
        while(!isDie)
        {
            yield return ws;
            //상태에 따라 분기 처리
            switch (state)
            {
                case State.PATROL:
                    //총알 발사 정지
                    enermyFire.isFire = false;
                    //순찰모드를 활성화
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                    //주인공의 위치를 넘겨 추적모드로 변경                 
                case State.TRACE:
                    //총알 발사 정지
                    enermyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:                 
                    //순찰 및 추적을 정지
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    //총알 발사 시작
                    if (enermyFire.isFire == false)
                    {
                        enermyFire.isFire = true;
                    }
                    break;
                case State.DIE:
                    isDie = true;                   
                    //순찰 및 추적을 정지
                    moveAgent.Stop();
                    enermyFire.isFire = false;
                    //사망 애니메이션의 종류 인덱스 랜덤지정
                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    //사망 애니메이션 실행
                    animator.SetTrigger(hashDie);
                    //Capshule collider 컴포넌트를 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
                    
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Speed파라미터에 이동속도를 전달
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
    
    public void OnPlayerDie()
    {
        if (!isDie)
        {
            moveAgent.Stop();
            enermyFire.isFire = false;
            //모든 코루틴 함수 종료
            StopAllCoroutines();

            animator.SetTrigger(hashPlayerDie);
        }
    }
}
