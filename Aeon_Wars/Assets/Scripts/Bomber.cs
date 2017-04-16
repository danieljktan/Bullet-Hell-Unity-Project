using UnityEngine;
using System.Collections;

public class Bomber : Ship {
    public Bullet PrimaryWeapon;
    private const float PrimaryCost = 0.20f;//energy cost of primary weapon
    private const float DamageTaken = 0.30f;

    private void Awake()
    {
        ShipName = "Petrus";
        Speed = 80.0f;
        FireRate = 0.10f;
        ShieldRegenRate = 0.015f;
        EnergyRegenRate = 0.40f;
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
            Instantiate(PrimaryWeapon, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f)));
        }
    }
}
