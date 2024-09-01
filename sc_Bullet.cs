using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Bullet : MonoBehaviour
{
    public float damage;
    public int per;


    private float speed = 15f;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// Init sc_Bullet.
    /// (per) parameter value -1 is Infinity Per
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="per"> -1 is Infinity Per</param>
    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per >= 0)
        {
            rigid.velocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per <= -1)
        {
            rigid.velocity = Vector2.zero;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
