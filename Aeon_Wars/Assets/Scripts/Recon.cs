using UnityEngine;
using System.Collections;

public class Recon : Ship {
    public Bullet PrimaryWeapon;
    private const float PrimaryCost = 0.25f;//energy cost of primary weapon
    private const float DamageTaken = 0.80f;

    private void Awake()
    {
        ShipName = "Sophia";
        Speed = 110.0f;
        FireRate = 0.10f;
        ShieldRegenRate = 0.10f;
        EnergyRegenRate = 0.80f;
        StartShip();
    }

    private void Start()
    {
        OnShipInstantiated();
    }

    private void Update()
    {
        OnShipUpdate();
    }

    private void OnTriggerEnter2D()
    {
        ShipDamaged(DamageTaken);
    }

    protected override void FirePrimary()
    {
        if (EnergyRatio >= PrimaryCost)
        {
            EnergyRatio -= PrimaryCost;
            for(float i = 45.0f; i < 150.0f; i+= 45.0f)
                FireBullet(i);
        }
    }

    private void FireBullet(float angle)
    {
        Instantiate(PrimaryWeapon, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
    }
}
