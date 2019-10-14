using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//내비게이션 기능을 사용하기위해 추가해야하는 네임스페이스

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{

    
    

    //순찰지점을 저장하기위한 List 타입 변수
    public List<Transform> wayPoints;
    //다음 순찰 지점의 배열의 Index
    public int nextIdx;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    //회전할 때의 속도를 조절하는 계수
    private float damping = 1.0f;



    //NaviMeshAgent컴포넌트를 저장할 변수
    private NavMeshAgent agent;
    //적 캐릭터의 Transform 컴포넌트를 저장할 변수
    private Transform enermyTr;

    //순찰 여부를 판단하는 변수
    private bool _patrolling;
    //patrolling 프로퍼티의 정의 (Getter, setter)
    //패트롤링이라는 불 타입
    public bool patrolling
    {
        get {  return _patrolling;}
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    //추적 대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;
    //traceTarget의 프로퍼티 정의
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            //추적상태의 회전 계수
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    //NaviMeshAgent의 이동속도에 대한 프로퍼티 정의 (getter)
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }


    // Start is called before the first frame update
    void Start()
    {
        //적 캐릭터의 Transform 컴포넌트를 추출후 ㅂㄴ수에 저장
        enermyTr = GetComponent<Transform>();

        //NaviMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화.
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
        //자동으로 회전하는 기능을 비활성화 
        agent.updateRotation = false;

        //하이러키 뷰의 WayPoint 게임 오 브젝트 추출 , 주어진 오브젝트를 찾지 못하면 null값을 반환  ?
        var group = GameObject.Find("WayPoint");
        if (group != null)
        {
            //하이커리의"WayPoint"  하위에 있는 모든 Transform 컴포넌트를 추출한 후 List 타입의 wayPoints 배열에 추가, 
            //
            group.GetComponentsInChildren<Transform>(wayPoints);
            //배열의 첫번쨰 항목 삭제, (WayPoint 본인 자신은 좌표값이 없으니 삭제해야함 ) 
            wayPoints.RemoveAt(0);

            //첫번째로 이동할 위치를 불규칙하게 추출
            nextIdx = Random.Range(0, wayPoints.Count);
        }

        //MoveWayPoint();
        this.patrolling = true;
    }

    //다음 목적지까지 이동명령을 내리는 함수
    void MoveWayPoint()
    {
        //최단거리 경로 계산이 끝나지 않았으면 다음을 수행하지않음
        if (agent.isPathStale) return;

        //다음 목적지를 wayPoints 배열에서 추출한 위치로 다음목적지를 지정
        agent.destination = wayPoints[nextIdx].position;
        //내비게이션 기능을 활성화해서 이동을 시작함
        agent.isStopped = false;
    }

    //주인공을 추적 할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        //목적지를 자신의 위치로 설정
        agent.destination = pos;
        agent.isStopped = false;
    }
    //순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
        //패트롤링이 아니게 되면  
        _patrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        //적캐릭터가 이동중일 때만 회전
        if (agent.isStopped == false)
        {
            //NaviMesh가 가야할 방향 벡터를 쿼터니언 타입의 각도로 변혼
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수를 사용해 점진적으로 회전
            enermyTr.rotation = Quaternion.Slerp(enermyTr.rotation, rot, Time.deltaTime * damping);
        }

        //순찰모드가 아닐 경우 이후 로직을 수행하지 않음
        if (!_patrolling) return;

        //NaviMeshAgent가 이동하고 있고 목적지에 도착했는지 여부를 계산 ( 도착 범위)
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            //다음 목적지의 배열 첨자를 계산
            //nextIdx = ++nextIdx % wayPoints.Count;
            nextIdx = Random.Range(0, wayPoints.Count);
            //다음 목적지로 이동명령 수행
            MoveWayPoint();
        }
    }
}
