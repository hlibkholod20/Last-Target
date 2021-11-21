using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("Speed Settings:")]
    public float minSpeed = 3;
    public float maxSpeed = 5;
    [Header("Rotation Settings:")]
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private float minRotation;
    private float currentSpeed;
    private float angleStart;
    private bool[] pow = new bool[40];
    private int count = 0;
    public GameObject projectile;
    public Transform startPoint;
    public enum State
    {
        Active,
        Sleeping
    }
    private static State state = State.Sleeping;
    // Start is called before the first frame update
    void Start()
    {
        pow[21] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Active)
        {

            //move enemy 
            float amtToMove1 = currentSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * amtToMove1, Space.World);

            //let it shoot
            if (opportunity())
            {
                Instantiate(projectile, transform.position, Quaternion.Euler(0,0,0));
            }

            //wrapped screen
            if (transform.position.y <= -5)
            {
                SetPositionAndSpeed();
                PlayLastLevel.missed++;
                PlayLastLevel.UpdateStats();
            }
            if (transform.position.x < -7.4f)
            {
                transform.position = new Vector3(7.3f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 7.4f)
            {
                transform.position = new Vector3(-7.3f, transform.position.y, transform.position.z);
            }

            if (PlayLastLevel.getEnemiesKilled() == 10)
            {
                state = State.Sleeping;
                transform.position = new Vector3(transform.position.x, 200, transform.position.y);
            }
        }




    }

    public void SetPositionAndSpeed()
    {
        // set new speed 
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        // set new position 
        float x = Random.Range(-6.0f, +6.0f);
        transform.position = new Vector3(x, 7.0f, 0);

        //behaviour of shoot
        projectile.GetComponent<ProjectileEnemy>().projectileSpeed = currentSpeed + projectile.GetComponent<ProjectileEnemy>().deltaSpeed;
    }

    public static void setState(State st)
    {
        state = st;
    }

    private bool opportunity()
    {
        count = (count + 1) % 40;
        return pow[count];
    }
}
