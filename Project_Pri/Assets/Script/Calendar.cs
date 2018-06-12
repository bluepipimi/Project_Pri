using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour {

    private int month = 0;

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
    public Text[] days;
    public Text monthTxt;
    
	// Use this for initialization
	void Start () {
        monthTxt.text = month.ToString();
        
	}
	
    public void IncreaseMonth()
    {
        month++;
        monthTxt.text = month.ToString();
    }

    public void DecreaseMonth()
    {
        month--;
        monthTxt.text = month.ToString();
    }

    public void PrintMonth()
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
                days[i].text = tmp[i].ToString();
            }
            else if (i >= 1 && i <= 6)
            {
                continue;
            }
            else
            {
                days[i - 6].text = tmp[i].ToString();
            }
        }
    }
}
