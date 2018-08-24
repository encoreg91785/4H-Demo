using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BeadItem : MonoBehaviour {
    EventTrigger eventTrigger;
    Image image;
    public Color color { set { image.color = value; } get { return image.color; } }
    public int bId = 0;
    public bool trigger = false;
    Vector2  vector2;
    bool isMove = false;
    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        image = GetComponent<Image>();
        eventTrigger= gameObject.AddComponent<EventTrigger>();
    }

    public void OnClilckDown()
    {
        transform.localScale = Vector2.one * 1.1f;
    }

    public void OnClickUp()
    {
        transform.localScale = Vector2.one;
        BeadController.instance.OnClickUp(this);
    }

    public void OnBeginDrag()
    {
        
        BeadController.instance.OnBeginDrag(this);
    }

    public void OnDrag()
    {
        BeadController.instance.OnDrag(this);
    }

    public void OnEndDrag()
    {
        BeadController.instance.OnEndDrag(this);
    }

    private void Update()
    {
        
        if (isMove==false) return;
        var v2 = transform.position;
        transform.position =Vector2.MoveTowards(v2, vector2,Time.deltaTime*2000);
        if (Vector2.Distance(v2, vector2) < 0.01f)
        {
            isMove = false;
            trigger = false;
        } 
    }

    public void MoveTo(Vector2 vector2)
    {
        isMove = true;
        this.vector2 = vector2;
    }
}
