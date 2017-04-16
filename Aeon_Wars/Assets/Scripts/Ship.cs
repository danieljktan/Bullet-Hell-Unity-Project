using UnityEngine;
using System.Collections;

public abstract class Ship : MonoBehaviour
{
    public delegate void ShipEvent(Ship reference);
    public event ShipEvent ShipInstantiated;//events to subscribe
    public event ShipEvent ShipUpdate;//events to subscribe
    public event ShipEvent ShipDestroyed;//events to subscribe
    protected abstract void FirePrimary();//needs to be overridden

    private string shipname;
    private float speed, fireRate, shieldRatio, energyRatio, shieldRegenRate, energyRegenRate;
    private Animator ShipAnim;
    protected ParticleSystem ShipParticles;
    protected Collider2D ShipCollider;
    protected SpriteRenderer ShipSprite;

    protected void DestroyShip()
    {
        transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
        ShipAnim.enabled = false;
        ShipCollider.enabled = false;
        ShipSprite.enabled = false;//turn off everything
        ShipParticles.startSpeed *= 3.0f;//got lazy....
        ShipParticles.startSpeed *= 3.0f;
        ShipParticles.Play();//play death particle effect
        OnShipDestroyed();
        Destroy(gameObject, ShipParticles.duration);//destroy ship after explosion has ended
        enabled = false;//turn the script off
    }

    protected virtual void OnShipInstantiated()
    {
        if (ShipInstantiated != null)
            ShipInstantiated(this);
    }

    protected void OnShipUpdate()
    {
        RegenerateStats();
        MoveShip();
        if (Input.GetButton(Gameplay.FireButton) && !IsInvoking("FirePrimary"))
            InvokeRepeating("FirePrimary", 0.0f, fireRate);
        else if (Input.GetButtonUp(Gameplay.FireButton)) //when space if lifed, cancel invoke
            CancelInvoke("FirePrimary");
        if (ShipUpdate != null)
            ShipUpdate(this);
    }

    protected virtual void OnShipDestroyed()
    {
        if (ShipDestroyed != null)
            ShipDestroyed(this);
    }

    protected void StartShip()
    {
        ShipParticles = GetComponent<ParticleSystem>();
        ShipAnim = GetComponent<Animator>();
        ShipCollider = GetComponent<Collider2D>();
        ShipSprite = GetComponent<SpriteRenderer>();
        ShieldRatio = 1.0f;
        EnergyRatio = 1.0f;
    }

    protected void ShipDamaged(float damage)
    {
        ShieldRatio -= damage;
        ShipParticles.Play();
        if (ShieldRatio <= 0.0f)
            DestroyShip();
        else
        {
            ShipSprite.color = Color.gray;
            ShipCollider.enabled = false;
            Invoke("TurnOnCollider", 0.5f);
        }
    }

    private void TurnOnCollider()
    {
        ShipSprite.color = Color.white;
        ShipCollider.enabled = true;
    }

    private void MoveShip()
    {
        float x = Input.GetAxisRaw(Gameplay.MovementX) * Speed * Time.deltaTime;
        float y = Input.GetAxisRaw(Gameplay.MovementY) * Speed * Time.deltaTime;//modify string value to get mouse
        Vector2 pos = transform.position;//temporary position
        pos += (new Vector2(x, y).normalized * Speed * Time.deltaTime);
        pos.x = Mathf.Clamp(pos.x, CameraMin.x, CameraMax.x);//x must be within camera range
        pos.y = Mathf.Clamp(pos.y, CameraMin.y, CameraMax.y);//y must be within camera range
        transform.position = pos;//position is temp position
        ShipAnim.SetFloat("X_MOVE", x);
    }

    private void RegenerateStats()
    {
        ShieldRatio += ShieldRegenRate * Time.deltaTime;
        EnergyRatio += EnergyRegenRate * Time.deltaTime;
    }

    //Properties
    public string ShipName
    {
        get
        {
            return shipname;
        }
        set
        {
            shipname = (value.Length <= 15) ? value : "Player";
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = Mathf.Clamp(value, 30.0f, 200.0f);
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
        set
        {
            fireRate = Mathf.Clamp(value, 0.01f, 100.0f);
        }
    }

    public float ShieldRatio
    {
        get
        {
            return shieldRatio;
        }
        set
        {
            shieldRatio = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float EnergyRatio
    {
        get
        {
            return energyRatio;
        }
        set
        {
            energyRatio = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float ShieldRegenRate
    {
        get
        {
            return shieldRegenRate;
        }
        set
        {
            shieldRegenRate = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float EnergyRegenRate
    {
        get
        {
            return energyRegenRate;
        }
        set
        {
            energyRegenRate = Mathf.Clamp(value, 0.0f, 1.0f);
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

    protected bool ShipEventsEmpty
    {
        get
        {
            return ShipInstantiated == null && ShipUpdate == null && ShipDestroyed == null;
        }
    }
}