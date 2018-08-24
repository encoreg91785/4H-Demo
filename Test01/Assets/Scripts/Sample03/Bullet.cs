using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public bool IsUsing = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(IsUsing==true) transform.position += Vector3.right * Time.deltaTime * 150;
    }
}
