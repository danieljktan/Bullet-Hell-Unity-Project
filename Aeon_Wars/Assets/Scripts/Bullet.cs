using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Range(0.0f - 200.0f, 200.0f)]
    public float Speed;
    [Range(0.0f - 200.0f, 200.0f)]
    public float Acceleration;

    public bool Pierce = false;
    public bool BounceX = false;//default
    public bool BounceY = false;//default
    //fires a bullet in a circle pattern
    public static void Circle(Vector3 position,Bullet prefab,int bulletAmount)
    {
        float angleIncrement = 360.0f / bulletAmount;
        for(float i = Random.Range(0.0f,angleIncrement); i < 360.0f; i += angleIncrement)
            Instantiate(prefab, position, Quaternion.Euler(0.0f, 0.0f, i));
    }

    //fires multiple bullets in a single direction
    public static void Burst(Vector3 position, float angle, Bullet prefab,int bulletAmount)
    {
        float spacing = 0.0f;
        for(int i = 0; i < bulletAmount; i++)
        {
            Bullet b = (Bullet)Instantiate(prefab, position, Quaternion.Euler(0.0f, 0.0f, angle));
            b.Acceleration += spacing;
            spacing += 30.0f;
        }
    }

    private Vector2 velocity, acceleration;

    private void Start()
    {
        float x = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180.0f);
        float y = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180.0f);
        velocity = new Vector2(x, y).normalized * Speed;
        acceleration = new Vector2(x, y).normalized * Acceleration;
    }

    private void Update()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
        if(OutsideX && BounceX)
        {
            velocity.x *= (0.0f - 1.0f);
            acceleration.x *= (0.0f - 1.0f);
            transform.eulerAngles = new Vector3(0.0f,0.0f,180.0f-transform.eulerAngles.z);
            transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
        }
        if(OutsideY && BounceY)
        {
            velocity.y *= (0.0f - 1.0f);
            acceleration.y *= (0.0f - 1.0f);
            transform.eulerAngles *= 0.0f-1.0f;
            transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * Time.deltaTime;
        }
        if (OutsideX || OutsideY)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D()
    {
        if(!Pierce)
            Destroy(gameObject);
    }

    //Properties
    public float Angle
    {
        set
        {
            transform.eulerAngles = new Vector3(0.0f,0.0f,value);
            float x = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180.0f);
            float y = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180.0f);
            velocity = new Vector2(x, y).normalized * Speed;
            acceleration = new Vector2(x, y).normalized * Acceleration;
        }
    }

    private bool OutsideX
    {
        get
        {
            return transform.position.x < CameraMin.x || transform.position.x > CameraMax.x;
        }
    }

    private bool OutsideY
    {
        get
        {
            return transform.position.y < CameraMin.y || transform.position.y > CameraMax.y;
        }
    }

    private static Vector2 CameraMin
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        }
    }

    private static Vector2 CameraMax
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));
        }
    }
}
