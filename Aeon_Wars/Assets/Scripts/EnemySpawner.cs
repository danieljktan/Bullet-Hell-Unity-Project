using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] EnemiesArray1;
    public Enemy[] EnemiesArray2;
    public delegate void SpawnerEvent(Enemy e);
    public delegate void EventSpawner(EnemySpawner Spawner);
    public event SpawnerEvent EnemySpawned;
    public event EventSpawner ClearEnemies;

    public void Register(Ship ShipObject)
    {
        enabled = true;
        ShipObject.ShipInstantiated += OnShipInstantiated;
        ShipObject.ShipDestroyed += OnShipDestroyed;
    }

    public void clear()
    {
        if (ClearEnemies != null)
        {
            print("Clearing enemies...");
            ClearEnemies(this);
        }
        else
            print("There are no enemies to clear");
    }

    private void OnShipInstantiated(Ship ShipObject)
    {
        Regular.projectilesfired = 3;
        Super.BulletsFired = 2;
        InvokeRepeating("SpawnEnemyRegular", 5.0f, 3.0f);
        InvokeRepeating("SpawnEnemySuper", 30.0f, 12.0f);
        InvokeRepeating("IncrementSpawner", 45.0f, 30.0f);
        ShipObject.ShipInstantiated -= OnShipInstantiated;
        print("Spawning Enemy Ships.");
    }

    private void OnShipDestroyed(Ship ShipObject)
    {
        CancelInvoke();
        ShipObject.ShipDestroyed -= OnShipDestroyed;
        print("Stopping enemy spawn");
        enabled = false;
    }

    private void IncrementSpawner()
    {
        Regular.projectilesfired += (Regular.projectilesfired < 24) ? 3 : 0;
        Super.BulletsFired += (Super.BulletsFired < 15) ? 2 : 0;
    }

    private void SpawnEnemyRegular()
    {
        int random = Random.Range(3, 7);
        for (int i = 0; i < random; i++)
        {
            Enemy e = (Enemy)Instantiate(EnemiesArray1[RandIdx1], RandTopPosition, Quaternion.identity);
            OnEnemySpawned(e);
            e.EnemyDestroyed += OnEnemyDestroyed;
            ClearEnemies += e.OnClearEnemies;
        }
    }

    private void SpawnEnemySuper()
    {
        int random = Random.Range(1, 3);
        for (int i = 0; i < random; i++)
        {
            Enemy e = (Enemy)Instantiate(EnemiesArray2[RandIdx2], RandTopPosition, Quaternion.identity);
            OnEnemySpawned(e);
            e.EnemyDestroyed += OnEnemyDestroyed;
            ClearEnemies += e.OnClearEnemies;
        }
    }

    private void OnEnemyDestroyed(Enemy e)
    {
        e.EnemyDestroyed -= OnEnemyDestroyed;
        ClearEnemies -= e.OnClearEnemies;
    }

    protected virtual void OnEnemySpawned(Enemy e)
    {
        if (EnemySpawned != null)
            EnemySpawned(e);
        else
            print("There is no one subscribed to the enemy spawner");
    }

    //Properties
    private static Vector2 CameraMin
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        }
    }

    private static Vector2 CameraMax
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        }
    }

    private static Vector2 Offset
    {
        get
        {
            return (CameraMax - CameraMin) / 16.0f;
        }
    }

    private int RandIdx1
    {
        get
        {
            return Random.Range(0, EnemiesArray1.Length);
        }
    }

    private int RandIdx2
    {
        get
        {
            return Random.Range(0, EnemiesArray2.Length);
        }
    }


    private static Vector3 RandTopPosition
    {
        get
        {
            return new Vector3(Random.Range(CameraMin.x + Offset.x, CameraMax.x - Offset.x), CameraMax.y, 0.0f);
        }
    }
}