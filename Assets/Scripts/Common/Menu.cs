using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject settiingsPanel;

    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBgm();
        //패널 비활성화
        settiingsPanel.SetActive(false);
    }
    //게임시작
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        Debug.Log("로드됨");
    }

    //설정열기
    public void OpenSettings()
    {
        settiingsPanel.SetActive(true);
    }
    //설정 닫기
    public void CloseSettings()
    {
        settiingsPanel.SetActive(false);
    }
    //참조하려는 Switch클라스를 위한 함수(bool 타입 매개변수)
    public void SwitchIsOn(bool isOn)
    {
        if (isOn == true)
        {
            PlayBgm();
        }
        else
        {
            StopBgm();
        }
    }
    public void PlayBgm()
    {
        audioSource.Play();
        Debug.Log("음악켜짐");
    }
    public void StopBgm()
    {
        audioSource.Stop();
        Debug.Log("음악꺼짐");
    }
}