using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;

public class Calendar : MonoBehaviour {

    private int currentMonth = 0;

    private int[] march = {
        3,  1,  10, 11, 20, 21, 31,
        0,  0,  0,  1,  2,  3,  4,
        5,  6,  7,  8,  9,  10, 11,
        12, 13, 14, 15, 16, 17, 18,
        19, 20, 21, 22, 23, 24, 25,
        26, 27, 28, 29, 30, 31, 0,};

    private int[] april =
    {
        4,   1,   10,  11,  20,  21,  29,
        0,   0,   0,   0,   0,   0,   1,
        2,   3,   4,   5,   6,   7,   8,
        9,   10,  11,  12,  13,  14,  15,
        16,  17,  18,  19,  20,  21,  22,
        23,  24,  25,  26,  27,  28,  29,

    };
    public Text[] dayTxts; // 36개
    public Image[] dayImages; // 35개 (일자 이미지 부분이다. 월부분이 빠져 -1이 되었다.)
    public Sprite arbeit1; // 알바1 그림
    public Sprite arbeit2; // 알바2 그림
    public Sprite arbeit3; // 알바3 그림
    public Text monthTxt; // 몇월인지 보여준다.

    private List<string> dates = new List<string>(); // 날짜 정보가 들어있음

    private int firstStart; // 상순시작지점
    private int firstEnd; // 상순끝지점
    private int middleStart; // 중순시작지점
    private int middleEnd; // 중순끝지점
    private int lastStart; // 하순시작지점
    private int lastEnd; // 하순끝지점


    public int LastStart
    {
        get { return lastStart; }
    }

    public int LastEnd
    {
        get { return lastEnd; }
    }
    
    public int CurrentMonth
    {
        get { return currentMonth; }
    }

	// Use this for initialization
	void Start () {
        monthTxt.text = currentMonth.ToString();
	}
	
    public void IncreaseMonth()
    {
        currentMonth++;
        monthTxt.text = currentMonth.ToString();
    }

    public void DecreaseMonth()
    {
        currentMonth--;
        monthTxt.text = currentMonth.ToString();
    }

    public void PrintMonth() // 배열로 된 날짜 출력
    {
        //Debug.Log("배열 길이 " + march.Length);

        int[] tmp;
        
        if(monthTxt.text == "3")
        {
            tmp = march; // 배열 복사 방법 1
        }
        else if(monthTxt.text == "4")
        {
            tmp = (int[]) april.Clone(); // 배열 복사 방법 2
        }
        else
        {
            return;
        }

        for (int i = 0; i < tmp.Length; i++)
        {
            if (i == 0)
            {
                dayTxts[i].text = tmp[i].ToString();
            }
            else if (i >= 1 && i <= 6)
            {
                continue;
            }
            else
            {
                dayTxts[i - 6].text = tmp[i].ToString();
            }
        }
    }

    public void ShowMonth() // Json으로 받아온 날짜 출력
    {
        currentMonth = Int32.Parse(monthTxt.text); // string -> int 변환
        if (currentMonth != 3 && currentMonth != 4)
        {
            for (int i = 0; i < dayTxts.Length; i++)
                dayTxts[i].text = "0";
            // 달력 이미지를 초기화 한다.
            for (int i = 0; i < dayImages.Length; i++)
            {
                dayImages[i].sprite = null;
            }
            return; // 일단 3,4월로 제한을 줌
        }
            
        JsonReader(monthTxt.text);

        for(int i=0; i<dates.Count; i++)
        {
            if(i == 0)
            {
                dayTxts[i].text = dates[i]; // 달 출력
            }
            else if(i >= 1 && i <= 6)
            {
                continue; // 일자정보에서 1번 ~ 6번은 불러오지 않음
            }
            else
            {
                dayTxts[i - 6].text = dates[i]; // 7번 ~ 마지막번 날짜 불러옴
            }
        }

        // 달력 이미지를 초기화 한다.
        for(int i =0; i< dayImages.Length; i++)
        {
            dayImages[i].sprite = null;
        }
    }

    private void JsonReader(string monthNum)
    {
        TextAsset file = Resources.Load(monthNum) as TextAsset;
        string JsonStrings = file.ToString();
        //Debug.Log(JsonStrings);
        JsonData CalendarData = JsonMapper.ToObject(JsonStrings);
        //Debug.Log(CalendarData[0][6]);

        dates.Clear(); // 기존에 있는 것들을 비운다.
        for (int i = 0; i < CalendarData.Count; i++) // 달력의 가로줄 하나에 접근한다.
        {
            for (int j = 0; j < CalendarData[0].Count; j++) // 날짜에 접근한다.
            {
                dates.Add(CalendarData[i][j].ToString());
            }
        }
    }

    public void PreviewSchedule(List<string> schedules)
    {

        // 달력에 예약할 스케줄을 미리 표시한다.
        //  - 리스트는 1차원 배열이다.

        //  - 상순, 중순은 10일씩이지만 하순은 줄었다 늘었다 한다.
        if (currentMonth != 3 && currentMonth != 4) return; // 일단 3,4월로 제한을 줌
        else if (dates.Count == 0) return; // 날짜 배열이 비어있으면 안된다.

        // - 어디까지가 상순, 중순, 하순인지 알아낸다. 
        //  - dates List에서 상순(1,2), 중순(3,4), 하순(5,6)이다.
        // - 상순, 중순, 하순에 해당하는 날짜를 알아낸다.
        // dates 리스트를 dayImages 배열 기준으로 바꾸기 위해 -7 을 해준다.
        firstStart = dates.IndexOf(dates[1], 7) - 7; // 상순시작지점
        firstEnd = dates.IndexOf(dates[2], 7) - 7;  // 상순끝지점
        middleStart = dates.IndexOf(dates[3], 7) - 7; // 중순시작지점
        middleEnd = dates.IndexOf(dates[4], 7) - 7; // 중순끝지점
        lastStart = dates.IndexOf(dates[5], 7) - 7; // 하순시작지점
        lastEnd = dates.IndexOf(dates[6], 7) - 7; // 하순끝지점

        // - 예약된 스케줄 정보를 알아낸다. (함수의 인수로 받아옴)
        // - 스케줄에 따라 상순, 중순, 하순에 해당하는 날짜에 일정에 맞는 그림을 표시한다.

        for (int i = 0; i < dayImages.Length; i++)
        {
            if (i >= firstStart && i <= firstEnd && schedules.Count >= 1) // 상순
            {
                switch (schedules[0])
                {
                    case "arbeit1":
                        dayImages[i].sprite = arbeit1;
                        break;
                    case "arbeit2":
                        dayImages[i].sprite = arbeit2;
                        break;
                    case "arbeit3":
                        dayImages[i].sprite = arbeit3;
                        break;
                }

            }
            else if (i >= middleStart && i <= middleEnd && schedules.Count >= 2) // 중순
            {
                switch (schedules[1])
                {
                    case "arbeit1":
                        dayImages[i].sprite = arbeit1;
                        break;
                    case "arbeit2":
                        dayImages[i].sprite = arbeit2;
                        break;
                    case "arbeit3":
                        dayImages[i].sprite = arbeit3;
                        break;
                }
            }
            else if (i >= lastStart && i <= lastEnd && schedules.Count >= 3) // 하순
            {
                switch (schedules[2])
                {
                    case "arbeit1":
                        dayImages[i].sprite = arbeit1;
                        break;
                    case "arbeit2":
                        dayImages[i].sprite = arbeit2;
                        break;
                    case "arbeit3":
                        dayImages[i].sprite = arbeit3;
                        break;
                }
            }
            else // 기타
                dayImages[i].sprite = null;
        }

        // 날짜 칸에 글씨를 다시 그려준다.
        for (int i = 0; i < dates.Count; i++)
        {
            if (i == 0)
            {
                dayTxts[i].text = dates[i]; // 달 출력
            }
            else if (i >= 1 && i <= 6)
            {
                continue; // 일자정보에서 1번 ~ 6번은 불러오지 않음
            }
            else
            {
                dayTxts[i - 6].text = dates[i]; // 7번 ~ 마지막번 날짜 불러옴
            }
        }

        // 날짜 일정 이미지를 보여준 칸에는 숫자를 지워야 된다.
        for (int i = 0; i < dayTxts.Length; i++)
        {
            if (i >= firstStart + 1 && i <= firstEnd + 1 && schedules.Count >= 1)
            {
                dayTxts[i].text = String.Empty;
            }
            else if (i >= middleStart + 1 && i <= middleEnd + 1 && schedules.Count >= 2)
            {
                dayTxts[i].text = String.Empty;
            }
            else if (i >= lastStart + 1 && i <= lastEnd + 1 && schedules.Count >= 3)
            {
                dayTxts[i].text = String.Empty;
            }       
        }
    }


}
