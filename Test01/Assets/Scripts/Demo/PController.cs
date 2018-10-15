using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PController : MonoBehaviour {
    float x = 0, y = 0, z = -1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            y = transform.position.y + Time.deltaTime;
            Debug.Log("WWWWWWWWWWWWWWWWWWW");
        }
        if (Input.GetKey(KeyCode.S))
        {
            y = transform.position.y - Time.deltaTime;
            Debug.Log("sssssssssssssssssss");
        }
        if (Input.GetKey(KeyCode.A))
        {
            x = transform.position.x - Time.deltaTime;
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
        }
        if (Input.GetKey(KeyCode.D))
        {
            x = transform.position.x + Time.deltaTime;
            Debug.Log("dddddddddddddddddddddddd");
        }
        transform.position = new Vector3(x,y,z);
    }
}
