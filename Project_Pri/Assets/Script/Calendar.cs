using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;

public class Year
{
    private int id; // ID
    private int game_year; // 년도
    private int game_month; // 달
    private int day1_min; // 상순 시작
    private int day1_max; // 상순 끝
    private int day2_min; // 중순 시작
    private int day2_max; // 중순 끝
    private int day3_min; // 하순 시작
    private int day3_max; // 하순 끝
    private List<int> daysList; // 일자 리스트

    public Year(int id, int game_year, int game_month, 
        int day1_min, int day1_max, int day2_min, int day2_max,
        int day3_min, int day3_max, List<int> days)
    {
        this.id = id;
        this.game_year = game_year;
        this.game_month = game_month;
        this.day1_min = day1_min;
        this.day1_max = day1_max;
        this.day2_min = day2_min;
        this.day2_max = day2_max;
        this.day3_min = day3_min;
        this.day3_max = day3_max;
        //daysList = days; // 얉은 복사 : days를 Clear하면 daysList도 Clear되버린다.
        //Debug.Log("생성자안에서 : " + days.Count); // 42
        daysList = new List<int>();

        for(int i=0; i<days.Count; i++)
        {
            daysList.Add(days[i]); // 깊은 복사
        }
    }

    public int GAME_YEAR
    {
        get { return game_year; }
    }
    public int GAME_MONTH
    {
        get { return game_month; }
    }
    public int DAY1_MIN
    {
        get { return day1_min; }
    }
    public int DAY1_MAX
    {
        get { return day1_max; }
    }
    public int DAY2_MIN
    {
        get { return day2_min; }
    }
    public int DAY2_MAX
    {
        get { return day2_max; }
    }
    public int DAY3_MIN
    {
        get { return day3_min; }
    }
    public int DAY3_MAX
    {
        get { return day3_max; }
    }
    public List<int> DAYSLIST
    {
        get { return daysList; }
    }
}


public class Calendar : MonoBehaviour {

    private int currentYear; // 현재 년도
    private int currentMonth; // 현재 달

    private List<Year> yearList = new List<Year>();

    
    public Text[] dayTxts; // 36개
    public Image[] dayImages; // 35개 (일자 이미지 부분이다. 월부분이 빠져 -1이 되었다.)
    public Sprite arbeit1; // 알바1 그림
    public Sprite arbeit2; // 알바2 그림
    public Sprite arbeit3; // 알바3 그림
    public Text yearTxt; // 년도 출력
    public Text monthTxt; // 월 출력 
    public Text yearTxt2; // 년도(디버깅용)
    public Text monthTxt2; // 월(디버깅용)
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
        JsonLoader();
        currentYear = 190;
        currentMonth = 1;
        yearTxt2.text = currentYear.ToString();
        monthTxt2.text = currentMonth.ToString();
        ShowCalendar();
	}
	
    public void IncreaseMonth()
    {
        if(currentYear == 197 && currentMonth == 1)
        {
            Debug.Log("마지막 날입니다.");
            return;
        }
        if(currentMonth < 12)
        {
            currentMonth++;
        }
        else
        {
            currentMonth = 1;
            currentYear++;
            
        }
        yearTxt2.text = currentYear.ToString();
        monthTxt2.text = currentMonth.ToString();
    }

    public void DecreaseMonth()
    {
        if(currentYear == 190 && currentMonth == 1)
        {
            Debug.Log("가장 첫해의 첫달입니다.");
            return;
        }

        Debug.Log("가장 첫해의 첫달입니다.");
        if (currentMonth > 0)
        {
            currentMonth--;
        }
        else
        {
            currentMonth = 12;
            currentYear--;
        }
        yearTxt2.text = currentYear.ToString();
        monthTxt2.text = currentMonth.ToString();
    }

    private void JsonLoader()
    {
        // 우선 Asset 폴더아래에 Resources 폴더안에 calendar Json 파일을 만들어 넣어야 한다.
        TextAsset file = Resources.Load<TextAsset>("calendar"); // 확장자를 안쓴다.
        string JsonStrings = file.ToString();
        //Debug.Log(JsonStrings);

        JsonData yearData = JsonMapper.ToObject(JsonStrings);
        //Debug.Log("ID : " + yearData[0]["ID"]);
        //Debug.Log("GAME_YEAR : " + yearData[0]["GAME_YEAR"]);
        //Debug.Log("GAME_MONTH : " + yearData[0]["GAME_MONTH"]);
        //Debug.Log("DAY1 : " + yearData[0]["DAY1"]); // DAY1 ~ DAY42
        //Debug.Log("count of GAME_YEAR : " + yearData.Count);

        List<int> tmpDays = new List<int>();

        for (int i = 0; i < yearData.Count; i++) // yearData의 한행에 접근
        {
            for (int j = 1; j <= 42; j++) // 한행의 1일 ~ 42일에 접근
            {
                // DAY? 에 접근해서 String으로 변환후 Int로후 변환 임시 리스트에 Add
                int tmpOneDay = JsonDataToInt(yearData[i]["DAY" + j.ToString()]);
                tmpDays.Add(tmpOneDay);
            }
            //Debug.Log("tmpDays : " + tmpDays.Count); // 42
            Year tmpYear = new Year(JsonDataToInt(yearData[i]["ID"]),
                JsonDataToInt(yearData[i]["GAME_YEAR"]),
                JsonDataToInt(yearData[i]["GAME_MONTH"]),
                JsonDataToInt(yearData[i]["DAY1_MIN"]),
                JsonDataToInt(yearData[i]["DAY1_MAX"]),
                JsonDataToInt(yearData[i]["DAY2_MIN"]),
                JsonDataToInt(yearData[i]["DAY2_MAX"]),
                JsonDataToInt(yearData[i]["DAY3_MIN"]),
                JsonDataToInt(yearData[i]["DAY3_MAX"]),
                tmpDays); // DAY1 ~ 42를 넣어준다.

            //Debug.Log("tempYear[" + i + " ].DAYLIST.Count : " + tmpYear.DAYSLIST.Count);
            yearList.Add(tmpYear);
            //Debug.Log("tmpDays.Count: " + tmpDays.Count);
            tmpDays.Clear(); // 리스트를 비워줘야 쌓이지 않는다.
        }

        //Debug.Log(yearList.Count); // 85
        //Debug.Log(yearList[0].DAYSLIST.Count); // 42
    }

    public void ShowCalendar() // Json으로 받아온 날짜 출력
    {
        yearTxt.text = currentYear.ToString();
        monthTxt.text = currentMonth.ToString();

        List<int> tmpDays = GetDaysOfThisMonth();

        //Debug.Log(tmpDays.Count);

        for (int i = 0; i < tmpDays.Count; i++)
        {
            if (tmpDays[i] == 0)
            {
                dayTxts[i].text = String.Empty;
            }
            else
            {
                dayTxts[i].text = tmpDays[i].ToString();
            }
        }

        // 다써서 필요없는 스케줄 미리보기 이미지를 지워준다.
        for(int i = 0; i<dayImages.Length; i++)
        {
            dayImages[i].sprite = null;
        }
    }

    private List<int> GetDaysOfThisMonth() // 이번달의 DAY1~DAY42를 구한다.
    {
        List<int> tmpDays = new List<int>();

        for (int i = 0; i < yearList.Count; i++) // 년도를 쭉 검색한다.
        {
            // 현재년도와 현재달에 해당하는 자료를 찾는다.
            if (currentYear == yearList[i].GAME_YEAR && currentMonth == yearList[i].GAME_MONTH)
            {
                //Debug.Log("1");
                //Debug.Log(yearList[i].DAYSLIST.Count);
                tmpDays = yearList[i].DAYSLIST; // 리스트 복사
                break; // For문을 빠져나온다.
            }
        }

        return tmpDays;
    }

    private int JsonDataToInt(JsonData json)
    {
        return Int32.Parse(json.ToString());
    }

    private void JsonReader(string monthNum)
    {
        TextAsset file = Resources.Load(monthNum) as TextAsset;
        string JsonStrings = file.ToString();
        Debug.Log(JsonStrings);
    }

    public void PreviewSchedule(List<string> schedules) 
    {
        // 달력에 예약할 스케줄을 미리 표시한다.
        // 스케줄 정보가 인수로 들어간다.

        // 현재 년 월의 년도 정보를 받아온다.
        Year tmpYear = null;

        for (int i = 0; i < yearList.Count; i++) // 년도를 쭉 검색한다.
        {
            // 현재년도와 현재달에 해당하는 Year 인스턴스를 찾는다.
            if (currentYear == yearList[i].GAME_YEAR &&
                currentMonth == yearList[i].GAME_MONTH)
            {
                //Debug.Log("1");
                //Debug.Log(yearList[i].DAYSLIST.Count);
                tmpYear = yearList[i]; // 리스트 얉은 복사
                break; // For문을 빠져나온다.
            }
        }
        // 상순, 중순, 하순에 해당하는 날짜를 알아낸다.
        firstStart = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY1_MIN); // 상순 첫날 인덱스
        firstEnd = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY1_MAX); // 상순 마지막날 인덱스
        middleStart = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY2_MIN); // 중순 첫날 인덱스
        middleEnd = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY2_MAX); // 중순 마지막날 인덱스
        lastStart = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY3_MIN); // 하순 첫날 인덱스
        lastEnd = tmpYear.DAYSLIST.IndexOf(tmpYear.DAY3_MAX); // 하순 마지막날 인덱스

        Debug.Log("DAY3_MAX : " + tmpYear.DAY3_MAX);
        Debug.Log(firstStart);
        Debug.Log(firstEnd);
        Debug.Log(middleStart);
        Debug.Log(middleEnd);
        Debug.Log(lastStart);
        Debug.Log(lastEnd);

        // 스케줄 이미지 처리
        for (int i = 0; i < dayImages.Length; i++)
        {
            // 상순 이미지를 표시한다. (DAY1_MIN ~ DAY1_MAX)
            if (i >= firstStart && i <= firstEnd && schedules.Count >= 1)
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
            // 중순 이미지를 표시한다. (DAY2_MIN ~ DAY2_MAX)
            else if (i >= middleStart && i <= middleEnd && schedules.Count >= 2)
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
            // 하순이미지를 표시한다. (DAY3_MIN ~ DAY3_MAX)
            else if(i >= lastStart && i <= lastEnd && schedules.Count >= 3)
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
            else
            {
                dayImages[i].sprite = null;
            }  
        }


        // 날짜 칸에 글씨를 다시 그려준다.
        List<int> tmpDays = GetDaysOfThisMonth();

        for (int i = 0; i < tmpDays.Count; i++)
        {
            if (tmpDays[i] == 0)
            {
                dayTxts[i].text = String.Empty;
            }
            else
            {
                dayTxts[i].text = tmpDays[i].ToString();
            }
        }

        // 날짜 일정 이미지를 보여준 칸에는 숫자를 지워야 된다.
        for (int i = 0; i < dayTxts.Length; i++)
        {
            if (i >= firstStart && i <= firstEnd && schedules.Count >= 1)
            {
                dayTxts[i].text = String.Empty;
            }
            else if (i >= middleStart && i <= middleEnd && schedules.Count >= 2)
            {
                dayTxts[i].text = String.Empty;
            }
            else if (i >= lastStart && i <= lastEnd && schedules.Count >= 3)
            {
                dayTxts[i].text = String.Empty;
            }
        }
    }


}
