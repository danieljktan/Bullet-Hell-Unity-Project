using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public delegate void EnemyEvent(Enemy e);
    public event EnemyEvent EnemyDestroyed;
    protected abstract void MoveEnermy();//needs to be overidden
    protected abstract void FireWeapon();//needs to be overidden
    protected abstract void DamagedAnim();//needs to be overidden
    protected abstract void KilledAnim();
    protected abstract void DestroyEnemy();
    private float speed;
    private int shield;
    private int enemyValue;

    public void OnClearEnemies(EnemySpawner Spawner)
    {
        Spawner.ClearEnemies -= OnClearEnemies;
        OnEnemyDestroyed();
        Destroy(gameObject);//OnDestroy handled in child class
    }

    protected virtual void OnEnemyDestroyed()
    {
        if (EnemyDestroyed != null)
            EnemyDestroyed(this);
    }

    protected void EnemyDamaged(int dmg)
    {
        Shield -= dmg;
        DamagedAnim();
        if (shield <= 0)
            DestroyEnemy();
    }

    //Properties
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = Mathf.Clamp(value, 0.0f - 150.0f, 150.0f);
        }
    }

    public int EnemyValue
    {
        get
        {
            return enemyValue;
        }
        set
        {
            enemyValue = value > 100 ? value : 100;
        }
    }

    public int Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = (value > 0) ? value : 0;
        }
    }

    protected bool OutsideX
    {
        get
        {
            return transform.position.x < CameraMin.x || transform.position.x > CameraMax.x;
        }
    }

    protected bool OutsideY
    {
        get
        {
            return transform.position.y < CameraMin.y || transform.position.y > CameraMax.y;
        }
    }

    static protected Vector2 CameraMin
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        }
    }

    static protected Vector2 CameraMax
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));
        }
    }

    protected bool EmptyEnemyEvent
    {
        get
        {
            return EnemyDestroyed == null;
        }
    }

    public bool OffScreen
    {
        get { return OutsideX || OutsideY; }
    }

    public bool Killed
    {
        get { return Shield <= 0; }
    }
}