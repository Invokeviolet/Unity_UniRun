using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip; // 사망시 재생할 오디오 클립
    public float jumpForce = 700f; // 점프 힘
    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태

    public float NoDieTime; // 현재 상태    
    public bool isUnbeatTime; // 무적상태인지 체크해주는 애

    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

    private void Start()
    {
        // 초기화
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        
    }
   
    private void Update()
    {        
        if (isDead) // 사망 시  처리를 더 이상 진행하지 않고 종료
        {
            return;
        }

        // 사용자 입력을 감지하고 점프하는 처리
        // 0 -> 마우스 왼쪽 버튼 / 1 -> 마우스 오른쪽 버튼 / 2 -> 마우스 스크롤 버튼

        if (Input.GetMouseButtonDown(0) && jumpCount < 2) // 마우스 왼쪽 버튼 클릭 && 최대 점프 횟수가 2에 도달하지 않았다면
        {
            jumpCount++; // 점프 횟수 증가
            playerRigidbody.velocity = Vector2.zero; // 점프 직전에 속도를 순간적으로 제로(0,0)로 변경
            playerRigidbody.AddForce(new Vector2(0, jumpForce)); // 리지드바디에 위쪽으로 힘 주기
            playerAudio.Play(); // 오디오 소스 재생
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0) // 마우스 클릭하고 손을 떼는 순간 && y축이 양수(위로 상승 중)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f; // 현재 속도를 절반으로 변경
        }

        NoDieTime+= Time.deltaTime;
        GameManager.instance.UpdatePowerTime(NoDieTime); // 값을 할당해줄 함수를 선언하고, () 안에 매개변수 적어
        if (NoDieTime>5f && isUnbeatTime == true) // 무적상태 해제하기 위한 조건
        {
            isUnbeatTime = false;
            GameManager.instance.OffPowerSlider();
        }
        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
    }
   
    public void Die()
    {
        // 사망 처리
        animator.SetTrigger("Die"); // 애니메이터의 Die 트리거 파라미터를 셋
        playerAudio.clip = deathClip; // 오디오 소스에 할당된 오디오 클립을 deathClip으로 변경
        playerAudio.Play(); // 사망 효과음 재생

        playerRigidbody.velocity = Vector2.zero; // 속도를 제로(0, 0)로 변경
        isDead = true; // 사망 상태를 true로 변경

        GameManager.instance.OnPlayerDead();
    }
    // SetTrigger() Set할때 true가 되었다가 곧바로 false가 되기 때문에 값을 지정하지 않음.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Dead" && !isDead && isUnbeatTime ==false) // 태그 == Die && 죽은 상태가 아닐때
        {
            Die(); //사망
        }
        if (other.tag == "DeadZone") //무적상태여도 떨어지면 죽음
        {
            Die(); //사망
        }
        if (other.tag == "NoDie") //이 태그가 붙은 아이템을 먹으면
        {
            Debug.Log("NoDie");
            UnbeatTime();
            GameManager.instance.OnPowerSlider(); // PowerSlider 표시
        }
    }
    public void UnbeatTime()
    {        
        isUnbeatTime = true; //무적상태 ON
        NoDieTime = 0; //무적상태시간 초기화
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true; // true로 변경하고, 누적 점프 횟수를 0으로 리셋
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 캐릭터가 어떤 콜라이더 (바닥)에서 벗어나면 false로 변경
        isGrounded = false;
    }
}