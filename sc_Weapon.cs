using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Weapon : MonoBehaviour
{
    public int id;
    public int PF_Id;
    public float damage;
    public int count;
    public float speed;

    private float timer;
    sc_player player;

    private void Awake()
    {
        player = sc_GameManager.instance.Player;
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damange, int count)
    {
        this.damage = damange * sc_Character.Damage; 
        this.count += count;
        if(id == 0)
        {
            doGen();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(sc_ItemData data)
    {
        // basic set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        // property set
        id = data.itemId;
        damage = data.baseDamage * sc_Character.Damage;
        count = data.baseCount + sc_Character.Count;
        
        for(int index=0; index < sc_GameManager.instance.PoolManager.PF_Object.Length; index++)
        {
            if(data.projectile == sc_GameManager.instance.PoolManager.PF_Object[index])
            {
                PF_Id = index;
                break;
            }
        }
        
        switch (id)
        {
            case 0:
                //회전속도
                speed = 150 * sc_Character.WeaponSpeed;
                doGen();
                break;
            default:
                // 연사속도 (speed 시간초마다 발사)
                speed = 0.5f * sc_Character.WeaponRate;
                break;
        }

        // Gand set
        sc_Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // 특정 함수 호출을 모든 자식에게 방송하는 함수
        // BrodcastMessage 두번째 인자 값으로 SendMessageOptions 추가 가능
        // DontRequireReceiver로 필수가 아니도록 변경
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void doGen()
    {
        for (int index = 0; index < count; index++) 
        {
            Transform bullet;
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = sc_GameManager.instance.PoolManager.Get(PF_Id).transform;
                bullet.parent = this.transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<sc_Bullet>().Init(damage, -100); // -1 is Infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.targetTransform)
            return;

        Vector3 targetPos = player.scanner.targetTransform.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = sc_GameManager.instance.PoolManager.Get(PF_Id).transform;
        bullet.position = transform.position;
        // FromToRotation 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up,dir);
        bullet.GetComponent<sc_Bullet>().Init(damage, count, dir);

        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Range);
    }
}
