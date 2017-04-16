using UnityEngine;
using System.Collections;

public class Super : Enemy {
    public Bullet Weapon;
    public static int BulletsFired = 3;
    private ParticleSystem SuperParticles;//prefab
    private SpriteRenderer SuperSprite;
    private Collider2D SuperCollider;

    // Use this for initialization
    private void Awake()
    {
        SuperParticles = GetComponent<ParticleSystem>();
        SuperSprite = GetComponent<SpriteRenderer>();
        SuperCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Speed = Random.Range(15.0f, 20.0f);
        Shield = 7;
        EnemyValue = 500;
        if (Weapon)
            InvokeRepeating("FireWeapon", 0.5f, Random.Range(1.0f, 5.0f));
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

    protected override void MoveEnermy()
    {
        transform.position -= new Vector3(0.0f, Speed * Time.deltaTime, 0.0f);
        if (transform.position.y + SuperSprite.bounds.extents.y< CameraMin.y)
            Destroy(gameObject);
    }

    protected override void FireWeapon()
    {
        Bullet.Circle(transform.position, Weapon, BulletsFired);
    }

    protected override void DamagedAnim()
    {
        SuperParticles.Play();
    }

    protected override void KilledAnim()
    {
        SuperParticles.Stop();
        SuperParticles.startSpeed += 6.0f;//Explosion randomly gets larger for no reason....
        SuperParticles.startSize += 12.0f;
        SuperParticles.Play();
    }

    protected override void DestroyEnemy()
    {
        SuperCollider.enabled = false;
        SuperSprite.enabled = false;
        KilledAnim();
        CancelInvoke();
        enabled = false;
        Destroy(gameObject, SuperParticles.duration);
    }

    private void OnDestroy()
    {
        OnEnemyDestroyed();
        if (!EmptyEnemyEvent)
            print("ERROR, Items are still subscribed to enemy");
    }
}
