using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Schedule : MonoBehaviour {

    private List<string> schedules; // 예약된 정보
    public GameObject[] bookedSchedules; // 예약된 정보 UI 개체

    public Sprite arbeit1; // 알바1 그림
    public Sprite arbeit2; // 알바2 그림
    public Sprite arbeit3; // 알바3 그림
    public Sprite first10Days; // 상순 그림
    public Sprite Middle10Days; // 중순 그림
    public Sprite Last10Days; // 하순 그림

	// Use this for initialization
	void Start () {
        schedules = new List<string>();
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
        DrawBookedSchedule();
    }

    public void DeleteSchdule(int i) // 스케줄 제거
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

        DrawBookedSchedule();

    }

    private void DrawBookedSchedule() // 스케줄 예약상황을 화면에 그려줍니다.
    {

        // 예약된 스케줄 이미지를 그려줍니다. 
        for (int i = 0; i < schedules.Count; i++)
        {
            string tmp = schedules[i];
            switch (tmp)
            {
                case "arbeit1":
                    bookedSchedules[i].GetComponent<Image>().sprite = arbeit1;
                    break;
                case "arbeit2":
                    bookedSchedules[i].GetComponent<Image>().sprite = arbeit2;
                    break;
                case "arbeit3":
                    bookedSchedules[i].GetComponent<Image>().sprite = arbeit3;
                    break;       
            }   
        }

        // 예약되지 않은 스케줄 이미지를 그려줍니다.
        if(schedules.Count == 0)
        {
            bookedSchedules[0].GetComponent<Image>().sprite = first10Days;
            bookedSchedules[1].GetComponent<Image>().sprite = Middle10Days;
            bookedSchedules[2].GetComponent<Image>().sprite = Last10Days;
        }
        else if(schedules.Count == 1)
        {
            bookedSchedules[1].GetComponent<Image>().sprite = Middle10Days;
            bookedSchedules[2].GetComponent<Image>().sprite = Last10Days;
        }
        else if(schedules.Count == 2)
        {
            bookedSchedules[2].GetComponent<Image>().sprite = Last10Days;
        }
        
    }


}
