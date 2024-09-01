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

        // Animator.GetCurrentAnimatorStateinfo ���� ���� ������ �������� �Լ�
        // �Ű� ���� = �ִϸ����� ���̾��� Index
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

    // ��ũ��Ʈ Ȱ��ȭ �� ȣ��Ǵ� �̺�Ʈ �Լ�
    private void OnEnable()
    {
        target = sc_GameManager.instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;

        isLive = true;
        coll.enabled = true;
        // ������ �ٵ� ���� ���� ����
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
            // ������ �ٵ� ���� ���� ����
            rigid.simulated = false;
            spriter.sortingOrder = 5;
            animator.SetBool("Dead",true);
            sc_GameManager.instance.kill++;
            sc_GameManager.instance.GetExp(1);

            if(sc_GameManager.instance.isLive)
                sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Dead);
        }
    }


    // IEnumerator �ڷ�ƾ���� ��ȯ�� �������̽�
    IEnumerator Knockback()
    {
        // yield �ڷ�ƾ�� ��ȯ Ű����
        // yield return new WaitForSeconds(2f); // 2�� ����
        // ������ null �̸� 1 ������ ����
        yield return wait; // ���� �ϳ��� ���� ������ ������
        
        Vector3 playerPos = sc_GameManager.instance.Player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockbackPower,ForceMode2D.Impulse);
    }

    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
