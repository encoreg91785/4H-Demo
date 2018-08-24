using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    [SerializeField]
    RawImage backGround;

    private void Start()
    {
        InvokeRepeating("BackGroundLoop", 0, 0.05f);   
    }

    private void Update()
    {
        
    }

    void BackGroundLoop()
    {
        var rect = backGround.uvRect;
        rect.x += 0.005f;
        backGround.uvRect = rect;
    }
}
