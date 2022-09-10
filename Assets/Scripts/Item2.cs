using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2 : Platform
{    
    public GameObject[] Items;
    public GameObject PowerBar;
    private bool touched = false; // 플레이어 캐릭터가 터치했나?    
    public float speed = 3f;

    public int dir = 0;    
    public float up;
    public float down;

    void Start()
    {
        
    }
    void Update()
    {
        gameObject.SetActive(true);

        up = speed * Time.deltaTime;
        down = -speed * Time.deltaTime;
        
        // 초당 speed의 속도로 위아래로 움직임

        if (transform.localPosition.y > 4f)
        {
            dir = 1;
        }
        if (transform.localPosition.y <= 2f)
        {
            dir = 0;
        }

        switch (dir)
        {
            case 0:
                transform.Translate(new Vector2(0, up));
                break;
            case 1:
                transform.Translate(new Vector2(0, down));
                break;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Player" && !touched) // 태그 == Die && 죽은 상태가 아닐때
        {
            touched = true;
            gameObject.SetActive(false);
            //일정시간동안 무적상태
           
        }
    }
}
