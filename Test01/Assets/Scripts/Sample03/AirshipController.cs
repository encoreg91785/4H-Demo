using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MonoBehaviour {
    [SerializeField]
    GameObject bullet,bulletPoolObjs;
    List<GameObject> usingBullets = new List<GameObject>();
    Queue<Bullet> bulletsPool = new Queue<Bullet>();
    float moveSpeed = 300;
	// Use this for initialization
	void Awake () {
        InvokeRepeating("OutOfScreenBullet",1,1);
        InstantiateBullet();
    }
	
	// Update is called once per frame
	void Update () {
        AirshipMove();
        Attack();
    }

    void OutOfScreenBullet()
    {
        var intList = new List<int>();
        for (int i = 0; i < usingBullets.Count; i++)
        {
            var v3 = usingBullets[i].transform.localPosition;
            if (v3.x > Screen.width / 2 || v3.x < -Screen.width / 2|| v3.y < -Screen.height|| v3.x > Screen.height)
            {
                intList.Add(i);
            }
        }
        for (int i = 0; i < intList.Count; i++)
        {
            var go = usingBullets[intList[i]].GetComponent<Bullet>();
            Recycle(go);
        }
    }

    float coolDwonTime = 0.5f;
    float attackTime = 0;
    void Attack()
    {
        if (Input.GetKey(KeyCode.Space)&& attackTime > coolDwonTime)
        {
            Fire();
            attackTime = 0;
        }
        attackTime += Time.deltaTime;
    }

    void Fire()
    {
        var go = GetBullet();
        usingBullets.Add(go);
        go.gameObject.transform.position = transform.position;
    }

    void AirshipMove()
    {
        Vector3 vector3 = Vector3.zero;
        var dt = Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            vector3.y += dt * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vector3.y -= dt * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector3.x += dt * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            vector3.x -= dt * moveSpeed;
        }
        transform.localPosition += vector3;
        transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, -Screen.width/2, Screen.width/2), Mathf.Clamp(transform.localPosition.y, -Screen.height / 2, Screen.height/2));
    }

    GameObject GetBullet()
    {
        if (bulletsPool.Count == 0) InstantiateBullet();
        var go = bulletsPool.Dequeue();
        go.IsUsing = true;
        go.gameObject.SetActive(true);
        return go.gameObject;
    }

    void Recycle(Bullet go)
    {
        go.IsUsing = false;
        go.gameObject.SetActive(false);
        bulletsPool.Enqueue(go);
    }

    private void InstantiateBullet()
    {
        for (int i = 0; i < 2; i++)
        {
            var go = Instantiate(this.bullet);
            go.SetActive(false);
            var bullet = go.GetComponent<Bullet>();
            bullet.IsUsing = false;
            bulletsPool.Enqueue(bullet);
            go.transform.SetParent(bulletPoolObjs.transform);
        }
    }
}
