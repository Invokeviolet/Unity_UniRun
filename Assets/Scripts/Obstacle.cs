using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Platform
{
    public int dir = 0;
    public float speed = 10f; // 이동 속도    
    public float right;
    public float left;
    void Start()
    {

    }
    private void Update()
    {
        right = speed * Time.deltaTime;
        left = -speed * Time.deltaTime;
        //if (GameManager.instance.isGameover) { return; }

        // 초당 speed의 속도로 오른쪽으로 평행이동
        
        if (transform.localPosition.x > 4f)
        {
            dir = 1;
        }
        if (transform.localPosition.x <= -4f)
        {
            dir = 0;
        }

        switch (dir)
        {
            case 0:
                transform.Translate(new Vector2(right, 0));
                break;
            case 1:
                transform.Translate(new Vector2(left, 0));
                break;
        }
    }
}
