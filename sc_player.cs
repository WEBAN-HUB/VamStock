using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sc_player : MonoBehaviour
{
    public Vector2 inputVec;
    public float playerSpeed = 5f;
    public sc_Scanner scanner;
    public sc_Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    public GameObject CameraLimit;
    public GameObject PlayerArea;

    Rigidbody2D playerRigd;
    SpriteRenderer playerSpriteRenderer;
    Animator playerAnimator;

    bool isHit = false;

    private void Awake()
    {
        playerRigd = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        scanner = GetComponent<sc_Scanner>();
        // getcomponent �Լ����� ���ڰ� true�� ������ ��Ȱ��ȭ �� ������Ʈ�� ���� �� �ִ�.
        hands = GetComponentsInChildren<sc_Hand>(true);
        CameraLimit = GameObject.Find("CameraLimit");
        PlayerArea = GameObject.Find("PlayerArea");
    }

    private void OnEnable()
    {
        playerSpeed *= sc_Character.Speed;
        playerAnimator.runtimeAnimatorController = animCon[sc_GameManager.instance.playerId-1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!sc_GameManager.instance.isLive)
            return;
    }

    // InputSystem ���� Ű �Է�
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        Vector2 moveVec = inputVec * playerSpeed * Time.deltaTime;
        playerRigd.MovePosition(playerRigd.position + moveVec);

        Vector3 cameraVec = CameraLimit.transform.position;
        cameraVec.x = this.transform.position.x;
        CameraLimit.transform.position = cameraVec;

        Vector3 areaVec = PlayerArea.transform.position;
        areaVec.x = this.transform.position.x;
        PlayerArea.transform.position = areaVec;
    }

    private void LateUpdate()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        // inpubVec.magnitude = ���� ũ��
        playerAnimator.SetFloat("Speed",inputVec.magnitude);
        if (inputVec.x != 0)
        {
            playerSpriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!sc_GameManager.instance.isLive)
            return;
        if (!collision.transform.CompareTag("Enemy"))
            return;
        if (isHit)
            return;

        StartCoroutine(HitPlayer());
        sc_GameManager.instance.health -= collision.gameObject.GetComponent<sc_Enemy>().damage;

        if(sc_GameManager.instance.health < 0)
        {
            for(int index =2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            playerAnimator.SetTrigger("Dead");
            sc_GameManager.instance.GameOver();
        }
    }

    IEnumerator HitPlayer()
    {
        Color hitColor = new Color(0.4f, 0.4f, 0.4f);
        isHit = true;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        yield return null;
        playerSpriteRenderer.color = hitColor;
        yield return wait;
        playerSpriteRenderer.color = Color.white;
        yield return wait;
        playerSpriteRenderer.color = hitColor;
        yield return wait;
        playerSpriteRenderer.color = Color.white;
        isHit = false;
    }
}
