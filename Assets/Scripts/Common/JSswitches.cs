using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSswitches : MonoBehaviour
{
    //Menu 클라스를 참조하기 위한 변수
    public Menu menu;
    //on/off 스위치의 위치값
    private Vector3 offSwitchPos;
    private Vector3 onSwitchPos;
    //True = 스위치 온, False = 스위치 오프
    public static bool isOn;
    //색깔을 바꿀 타겟
    public GameObject targetSwitch;
    public RectTransform handleRecctTransform;
    //이미지 와 컬러 변수
    Image image;
    Color switchAlpha;
    private void Start()
    {
        //시작 시 온
        isOn = true;  
        image = targetSwitch.GetComponent<Image>();
        switchAlpha = image.color;

        offSwitchPos = new Vector3(16, 0);
        onSwitchPos = new Vector3(-16, 0);
        handleRecctTransform.anchoredPosition = onSwitchPos;
    }   
    public void OnClickSwitch()
    {      
        //todo : isOn를 변경시킬 코드 작성
        if (isOn == false)
        {           
            SwitchOn();
            Debug.Log("on");
        }
        else
        {
            SwitchOff();
            Debug.Log("off");
        }
        menu.SwitchIsOn(isOn);
    }
    public void SwitchOn()
    {
        isOn = true;
        handleRecctTransform.anchoredPosition = onSwitchPos;
        switchAlpha.a = 1f;
        image.color = switchAlpha;
    }
    public void SwitchOff()
    {
        isOn = false;
        handleRecctTransform.anchoredPosition = offSwitchPos;
        switchAlpha.a = 0.2f;
        image.color = switchAlpha;
    }
}