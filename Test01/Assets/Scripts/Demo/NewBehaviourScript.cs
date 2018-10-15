using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public Image aaa;
    public Text text;
    private float colorFloat = 0;
    bool fadeOut =true;
    //private void Awake()
    //{
        
    //}
    //private void Start()
    //{
        
    //}
    private void Update()
    {
        if (fadeOut)
        {
            colorFloat -= 1 * Time.deltaTime;
            //colorFloat = colorFloat - 1 * Time.deltaTime;
            //int ccc = 5;
            //ccc = ccc - 2;
            //ccc -= 2;
        }
        else
        {
            colorFloat += 1 * Time.deltaTime;
        }
        
        if (colorFloat > 1)
        {
            colorFloat = 1;
        }
        else if (colorFloat < 0)
        {
            colorFloat = 0;
        }  
        aaa.color = new Color(colorFloat,1,1,1);
    }

    private void FixedUpdate()
    {
        
    }
    public void ChangeColorBlue()
    {
        fadeOut = true;
       // aaa.color = Color.blue;
    }

    public void ColorRed()
    {
        fadeOut = false;
       // aaa.color = Color.green;
    }
}
