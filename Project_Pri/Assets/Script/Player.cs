using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int hp;

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    void Awake()
    {
        hp = 30;
    }
    // Use this for initialization
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
