using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject itemPrefab2;
    public int count = 5; // 생성할 개수  

    public float timeBetSpawnMin = 0.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 1.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = 2f; // 배치할 위치의 최소 y값
    public float yMax = 3.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] items; // 미리 생성한 아이템
    private int curItem = 0; // 사용할 현재 순번의 아이템

    private Vector2 poolPosition = new Vector2(20, 0); // 초반에 생성된 아이템들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start()
    {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        items = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            items[i] = Instantiate(itemPrefab, poolPosition, Quaternion.identity);
            items[i] = Instantiate(itemPrefab2, poolPosition, Quaternion.identity);
        }
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;

    }
        
    void Update()
    {        
        //게임오버일때는 업데이트 진행하지 않고 반환
        if (GameManager.instance.isGameover)
        {
            return;
        }
        if (Time.time - lastSpawnTime >= timeBetSpawn)  
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // 랜덤한 위치에 배치
            float yPos = Random.Range(yMin, yMax);

            items[curItem].SetActive(false);
            items[curItem].SetActive(true);

            items[curItem].transform.position = new Vector2(xPos, yPos);
            curItem++;

            if (curItem >= count)
            {
                curItem = 0;
            }
        }
    }
}
