using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Explosion:")]
    public GameObject explosionType;
    [Header("Shield settings")]
    private static bool destroyed = false;
    private float blinkRate = .1f;
    public static int shieldPoints = 100;
    private int numberOfTimesToBlink = 10;
    private int blinkCount;
    private bool[] frame = new bool[60];
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        frame[31] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldPoints < 100) regenerate();
        Player.UpdateStats();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (shieldPoints != 0)
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                enemy.SetPositionAndSpeed();
                if (shieldPoints >= 50)
                {
                    shieldPoints -= 50;
                }
                else
                {
                    shieldPoints = 0;
                }
                Player.UpdateStats();
                //StartCoroutine(Blink());
            }
            if (shieldPoints <= 0)
            {
                destroyed = true;
                Destroy(this.gameObject);
            }
        }

        else if (other.tag == "small")
        {
            if (shieldPoints != 0)
            {
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                Destroy(other.gameObject);
                if (shieldPoints >= 10)
                {
                    shieldPoints -= 10;
                }
                else
                {
                    shieldPoints = 0;
                }
                Player.UpdateStats();
                //StartCoroutine(Blink());
            }
            if (shieldPoints <= 0)
            {
                destroyed = true;
                Destroy(this.gameObject);
            }
        }

        else if (other.tag == "ProjectileEnemy")
        {
            if (shieldPoints != 0)
            {
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                Destroy(other.gameObject);
                if (shieldPoints >= 1)
                {
                    shieldPoints--;
                }
                else
                {
                    shieldPoints = 0;
                }
            }

            if (shieldPoints <= 0)
            {
                destroyed = true;
                Destroy(this.gameObject);
            }
        }

        else if (other.tag == "EnemyShip")
        {
            if (shieldPoints != 0)
            {
                EnemyShip enemy = other.gameObject.GetComponent<EnemyShip>();
                Instantiate(explosionType, transform.position + new Vector3(0, 2, 0), transform.rotation);
                enemy.SetPositionAndSpeed();
                if (shieldPoints >= 50)
                {
                    shieldPoints -= 50;
                }
                else
                {
                    shieldPoints = 0;
                }
                Player.UpdateStats();
                //StartCoroutine(Blink());
            }
            if (shieldPoints <= 0)
            {
                destroyed = true;
                Destroy(this.gameObject);
            }
        }

    }

    public static bool isDestroyed()
    {
        if (destroyed) return true;
        else return false;
    }

    private void regenerate()
    {
        i = (i + 1) % 60;
        if (shieldPoints < 100 && frame[i])
        {
            if (shieldPoints < 95) shieldPoints += 5;
            else shieldPoints = 100;
        }    
    }

    private IEnumerator Blink()
    {
        while (blinkCount < numberOfTimesToBlink)
        {
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
            if (gameObject.GetComponent<Renderer>().enabled)
            {
                blinkCount++;
            }
            yield return new WaitForSeconds(blinkRate);
        }
    }


}
