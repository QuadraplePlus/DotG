using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegate : MonoBehaviour
{
    //델리게이트에 연결할 함수의 원형 정의
    delegate void CalNumDelegate(int Num);

    //델리게이트 변수 선언
    CalNumDelegate calNum;

    // Start is called before the first frame update
    void Start()
    {
        //calNum델리게이트 변수에 OnePlusNum함수 연결
        calNum = OnePlusNum;
        //함수 호출
        calNum(4);

        //clanum델리게이트 ㅂㄴ수에 PowerNum 함수 연결
        calNum = PowerNum;
        calNum(5);
    }

    void OnePlusNum(int Num)
    {
        int result = Num + 1;
        Debug.Log("원 플러스 는" + result);
    }
    void PowerNum(int Num)
    {
        int reslut = Num * Num;
        Debug.Log("파워 는 " + reslut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
