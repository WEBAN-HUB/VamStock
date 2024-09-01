using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive;

    Animator animator;
    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    float knockbackPower = 3f;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sc_GameManager.instance.isLive)
            return;
    }

    private void FixedUpdate()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        // Animator.GetCurrentAnimatorStateinfo 현재 상태 정보를 가져오는 함수
        // 매개 변수 = 애니메이터 레이어의 Index
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextvec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextvec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        if (!isLive) 
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 스크립트 활성화 시 호출되는 이벤트 함수
    private void OnEnable()
    {
        target = sc_GameManager.instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;

        isLive = true;
        coll.enabled = true;
        // 리지드 바디 물리 적용 끄기
        rigid.simulated = true;
        spriter.sortingOrder = 6;
        animator.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        damage = data.damage;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<sc_Bullet>().damage;
        StartCoroutine(Knockback());
        if(health > 0)
        {   //.. Live, Hit Action
            animator.SetTrigger("Hit");
            sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Hit);
        }
        else
        {   // ..Die
            isLive = false;
            coll.enabled = false;
            // 리지드 바디 물리 적용 끄기
            rigid.simulated = false;
            spriter.sortingOrder = 5;
            animator.SetBool("Dead",true);
            sc_GameManager.instance.kill++;
            sc_GameManager.instance.GetExp(1);

            if(sc_GameManager.instance.isLive)
                sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Dead);
        }
    }


    // IEnumerator 코루틴만의 반환형 인터페이스
    IEnumerator Knockback()
    {
        // yield 코루틴의 반환 키워드
        // yield return new WaitForSeconds(2f); // 2초 쉬기
        // 리턴이 null 이면 1 프레임 쉬기
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        
        Vector3 playerPos = sc_GameManager.instance.Player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockbackPower,ForceMode2D.Impulse);
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
