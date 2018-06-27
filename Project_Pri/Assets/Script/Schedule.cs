using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Schedule : MonoBehaviour {

    private List<string> schedules; // 예약된 정보
    private List<string> decidedSchedules; // 결정된 스케줄
    public GameObject[] selectedSchedules; // UI에서 상순, 중순, 하순
    public Text ResultText; // 실행 결과 상자

    public Sprite arbeit1; // 알바1 그림
    public Sprite arbeit2; // 알바2 그림
    public Sprite arbeit3; // 알바3 그림
    public Sprite DoArbeit1; // 알바1을 하는 그림
    public Sprite DoArbeit2; // 알바2를 하는 그림
    public Sprite DoArbeit3; // 알바3을 하는 그림
    public Sprite first10Days; // 상순 그림
    public Sprite middle10Days; // 중순 그림
    public Sprite last10Days; // 하순 그림

    public Calendar calendar; // Calendar 클래스에 접근하기 위한 용도
    public Player player; // Player 클래스에 접근하기 위한 용도

    public GameObject PerformingPanel; // 스케줄 실행 패널
    public Image DoSchedule; // 스케줄 하는 이미지UI
    public Text HpText; // SchedulePanel 안
    public Text HpText2; // PerformingPanel 안
    public Text DetailsText;
    private int day = 0;
    private System.Random rnd;

    // Use this for initialization
    void Start () {
        schedules = new List<string>();
        decidedSchedules = new List<string>();
        HpText.text = player.HP.ToString();
        HpText2.text = player.HP.ToString();
        rnd = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
	   	
	}

    public void AddSchedule(int i) // 스케줄 추가
    {
        if(schedules.Count >= 3)
        {
            Debug.Log("스케줄이 가득 찾습니다.");
            return;
        }
        switch(i)
        {
            case 1:
                schedules.Add("arbeit1");
                break;
            case 2:
                schedules.Add("arbeit2");
                break;
            case 3:
                schedules.Add("arbeit3");
                break;
        }
        DrawSchedule();
    }

    public void DeleteSchedule() // 스케줄 제거 (클릭하면 맨 마지막 스케줄을 지운다.)
    {
        if(schedules.Count <= 0)
        {
            Debug.Log("스케줄이 비었습니다.");
        }
        else
        {
            int lastIndex = schedules.Count - 1;
            schedules.RemoveAt(lastIndex);
        }
        DrawSchedule();
    }

    public void DeleteSchedule(int i) // 스케줄 제거 (클릭하면 클릭한 스케줄을 지운다.)
    {
        // 두개의 스케줄이 예약되어 있을 때 첫번째를 클릭하면 두번쨰가 첫번쨰로 와야 한다. 
        if(schedules.Count <= 0)
        {
            Debug.Log("스케줄이 비었습니다.");
        }
        else if(schedules.Count == 1)
        {
            if(i == 1) // 상순 클릭
            {
                schedules.RemoveAt(0);
            }
        }
        else if(schedules.Count == 2)
        {
            if (i == 1) // 상순 클릭
            {
                schedules.RemoveAt(0);
            }
            else if (i == 2) // 중순 클릭
            {
                schedules.RemoveAt(1);
            }
        }
        else if(schedules.Count == 3)
        {
            if (i == 1) // 상순 클릭
            {
                schedules.RemoveAt(0);
            }
            else if (i == 2) // 중순 클릭
            {
                schedules.RemoveAt(1);
            }
            else if(i == 3) // 하순 클릭
            {
                schedules.RemoveAt(2);
            }
        }

        DrawSchedule();

    }

    private void DrawSchedule() // 스케줄 예약상황을 화면에 그려줍니다.
    {
        // 예약된 스케줄 이미지를 그려줍니다. 
        for (int i = 0; i < schedules.Count; i++)
        {
            string tmp = schedules[i];
            switch (tmp)
            {
                case "arbeit1":
                    selectedSchedules[i].GetComponent<Image>().sprite = arbeit1;
                    break;
                case "arbeit2":
                    selectedSchedules[i].GetComponent<Image>().sprite = arbeit2;
                    break;
                case "arbeit3":
                    selectedSchedules[i].GetComponent<Image>().sprite = arbeit3;
                    break;       
            }   
        }


        // 예약되지 않은 스케줄 이미지를 그려줍니다.
        if(schedules.Count == 0)
        {
            selectedSchedules[0].GetComponent<Image>().sprite = first10Days;
            selectedSchedules[1].GetComponent<Image>().sprite = middle10Days;
            selectedSchedules[2].GetComponent<Image>().sprite = last10Days;
        }
        else if(schedules.Count == 1)
        {
            selectedSchedules[1].GetComponent<Image>().sprite = middle10Days;
            selectedSchedules[2].GetComponent<Image>().sprite = last10Days;
        }
        else if(schedules.Count == 2)
        {
            selectedSchedules[2].GetComponent<Image>().sprite = last10Days;
        }
        calendar.PreviewSchedule(schedules);
    }

    public void ResetShedule() // 스케줄을 초기상태로 돌린다.
    {
        schedules.Clear();

        selectedSchedules[0].GetComponent<Image>().sprite = first10Days;
        selectedSchedules[1].GetComponent<Image>().sprite = middle10Days;
        selectedSchedules[2].GetComponent<Image>().sprite = last10Days;

        ResultText.text = string.Empty;
    }

    public void ShowResult()
    {
        if(schedules.Count < 3)
        {
            Debug.Log("스케줄을 전부 지정해주세요.");
            return;
        }
        ResultText.text = "";

        string[] result = new string[3];
        for(int i = 0; i < 3; i++)
        {
            if (schedules[i] == "arbeit1")
            {
                result[i] = "알바1";
            }
            else if (schedules[i] == "arbeit2")
            {
                result[i] = "알바2";
            }
            else if (schedules[i] == "arbeit3")
            {
                result[i] = "알바3";
            }
        }

        ResultText.text = "상순 : " + result[0] + "\n";
        ResultText.text += "중순 : " + result[1] + "\n";
        ResultText.text += "하순 : " + result[2] + "\n";
    }

    public void RunSchedules() // 스케줄 실행
    {
        // 스케줄칸에 스케줄이 꽉차있어야 한다.
        if (schedules.Count < 3)
        {
            Debug.Log("스케줄을 먼저 채워주세요.");
            return;
        }

        // 상순, 중순, 하순에 할당된 스케줄이 무엇인가?
        // 상순, 중순은 10일간이고 하순은 날짜가 달마다 다르다.
        for (int i = 0; i < 10; i++) // 상순 스케줄 저장
            decidedSchedules.Add(schedules[0]);

        for (int i = 0; i < 10; i++) // 중순 스케줄 저장
            decidedSchedules.Add(schedules[1]);

        int last10day = calendar.LastEnd - calendar.LastStart + 1; // 첫날도 포함 (+1)
        for (int i = 0; i < last10day; i++) // 하순 스케줄 저장
            decidedSchedules.Add(schedules[2]);
        //Debug.Log(calendar.LastEnd);
        //Debug.Log(calendar.LastStart);
        //for (int i = 0; i < decidedSchedules.Count; i++)
        //    Debug.Log(decidedSchedules[i]);
        //Debug.Log(decidedSchedules.Count); // 31

        InvokeRepeating("executeSchedules", 0.1f, 1.0f);

    }

    private void executeSchedules() // 스케줄을 실행한다
    {
        //Debug.Log("day " + day);
        string tmp = string.Empty;

        if (day >= decidedSchedules.Count)
        {
            Debug.Log("스케줄이 종료되었습니다.");
            PerformingPanel.SetActive(false);
            CancelInvoke("executeSchedules");
            day = 0;
            decidedSchedules.Clear();
            calendar.IncreaseMonth(); // 스케줄이 다 실행되면 다음달로 넘어간다.
            calendar.ShowCalendar();
            ResetShedule();
            HpText.text = player.HP.ToString();
        }
        else
        {
            switch (decidedSchedules[day])
            {
                case "arbeit1":
                    tmp = "알바1";
                    DoSchedule.sprite = DoArbeit1;
                    break;
                case "arbeit2":
                    tmp = "알바2";
                    DoSchedule.sprite = DoArbeit2;
                    break;
                case "arbeit3":
                    tmp = "알바3";
                    DoSchedule.sprite = DoArbeit3;
                    break;
            }
            int HpIncrement = rnd.Next(1,4); // 체력 증가량 (랜덤 1~3)
            PerformingPanel.SetActive(true);
            DetailsText.text = calendar.CurrentMonth + "월 " + (day+1) + "일, ";
            DetailsText.text += tmp + "을[를] 하였는데..\n";
            DetailsText.text += "체력이 " + HpIncrement +" 증가 하였습니다.";
            player.HP += HpIncrement;
            HpText2.text = player.HP.ToString();
            
            day++;
        }
        

        
        
            
    }
}
