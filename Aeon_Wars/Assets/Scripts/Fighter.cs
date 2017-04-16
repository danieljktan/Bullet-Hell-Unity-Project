using UnityEngine;
using System.Collections;

public class Fighter : Ship
{
    public Bullet PrimaryWeapon;
    public Bullet SecondaryWeapon;
    private const float PrimaryCost = 0.15f;//energy cost of primary weapon
    private const float SecondaryCost = 0.30f;
    private const float DamageTaken = 0.40f;

    private void Awake()
    {
        ShipName = "Seraphim";
        Speed = 90.0f;
        FireRate = 0.075f;
        ShieldRegenRate = 0.012f;
        EnergyRegenRate = 0.60f;
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

    private void FireSecondary()
    {
        Instantiate(SecondaryWeapon, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f)));
    }
}
