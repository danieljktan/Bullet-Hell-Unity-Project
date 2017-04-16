using UnityEngine;
using System.Collections;

public class Regular : Enemy
{
    public Bullet Weapon;
    private ParticleSystem RegularParticles;//prefab
    private SpriteRenderer RegularSprite;
    private Collider2D RegularCollider;
    private Animator RegularAnim;
    public static int projectilesfired = 3;
    private float x = 0.0f;

    // Use this for initialization
    private void Awake()
    {
        RegularAnim = GetComponent<Animator>();
        RegularParticles = GetComponent<ParticleSystem>();
        RegularSprite = GetComponent<SpriteRenderer>();
        RegularCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Speed = Random.Range(30.0f, 75.0f);
        Shield = 2;
        EnemyValue = 100;
        InvokeRepeating("RandomXDirection", 0.0f, Random.Range(0.50f, 2.5f));
        if(Weapon)
            InvokeRepeating("FireWeapon",0.5f,Random.Range(1.0f,5.0f));
    }

    // Update is called once per frame
    private void Update()
    {
        MoveEnermy();
    }

    private void OnTriggerEnter2D()
    {
        EnemyDamaged(1);
    }

    //private void OnTriggerStay2D()
    //{
    //    EnemyDamaged(1);
    //}

    private void RandomXDirection()
    {
        x = Random.Range(0.0f - (Speed / 2.0f), Speed / 2.0f);//needs to be BEFORE initialization of speed
        RegularAnim.SetFloat("X_MOVE", x);
    }

    protected override void MoveEnermy()
    {
        transform.position += new Vector3(x * Time.deltaTime, 0.0f - Speed * Time.deltaTime, 0.0f);
        if (OutsideX || OutsideY)
            Destroy(gameObject);
    }

    protected override void FireWeapon()
    {
        Bullet.Circle(transform.position, Weapon, projectilesfired);
    }

    protected override void DamagedAnim()
    {
        RegularParticles.Play();
    }

    protected override void KilledAnim()
    {
        RegularParticles.startSpeed += 3.0f;//Explosion randomly gets larger for no reason....
        RegularParticles.startSize += 8.0f;
        RegularParticles.Play();
    }

    protected override void DestroyEnemy()
    {
        RegularAnim.enabled = false;
        RegularCollider.enabled = false;
        RegularSprite.enabled = false;
        KilledAnim();
        CancelInvoke();
        enabled = false;
        Destroy(gameObject, RegularParticles.duration);
    }

    private void OnDestroy()
    {
        OnEnemyDestroyed();
        if (!EmptyEnemyEvent)
            print("ERROR, Items are still subscribed to enemy");
    }
}