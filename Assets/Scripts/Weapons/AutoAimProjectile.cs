using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoAimProjectile : MonoBehaviour
{
    [SerializeField]
    [Header("Projectile Variables:")]
    private float projectileSpeed;
    public GameObject explosionPrefab;
 
   
   
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Enemy") != null)
        {
            GameObject nearest = GameObject.Find("Enemy");
            Vector3 toEnemy = nearest.transform.position - transform.position;
            float angleToEnemyY = Vector3.Dot(transform.up, toEnemy.normalized);
            float angleToEnemyX = Vector3.Dot(transform.right, toEnemy.normalized);
            float amtToMove = projectileSpeed * Time.deltaTime;
            transform.Translate(new Vector3(angleToEnemyX, angleToEnemyY, 0) * amtToMove, Space.World);
            if (transform.position.y > 7.5f)
            {
                Destroy(this.gameObject);
            }
        }

        else if (GameObject.Find("EnemyShip"))
        {
            GameObject nearest = GameObject.Find("EnemyShip");
            Vector3 toEnemy = nearest.transform.position - transform.position;
            float angleToEnemyY = Vector3.Dot(transform.up, toEnemy.normalized);
            float angleToEnemyX = Vector3.Dot(transform.right, toEnemy.normalized);
            float amtToMove = projectileSpeed * Time.deltaTime;
            transform.Translate(new Vector3(angleToEnemyX, angleToEnemyY, 0) * amtToMove, Space.World);
            if (transform.position.y > 7.5f)
            {
                Destroy(this.gameObject);
            }
        }

        else if (GameObject.Find("minienemies"))
        {
            GameObject nearest = GameObject.Find("minienemies");
            Vector3 toEnemy = nearest.transform.position - transform.position;
            float angleToEnemyY = Vector3.Dot(transform.up, toEnemy.normalized);
            float angleToEnemyX = Vector3.Dot(transform.right, toEnemy.normalized);
            float amtToMove = projectileSpeed * Time.deltaTime;
            transform.Translate(new Vector3(angleToEnemyX, angleToEnemyY, 0) * amtToMove, Space.World);
            if (transform.position.y > 7.5f)
            {
                Destroy(this.gameObject);
            }
        }

        else if (GameObject.Find("Advanced Enemy") != null)
        {
            if (MainBoss.cantBeKilled())
            {
                GameObject nearest = GameObject.Find("Advanced Enemy");
                Vector3 toEnemy = nearest.transform.position - transform.position;
                float angleToEnemyY = Vector3.Dot(transform.up, toEnemy.normalized);
                float angleToEnemyX = Vector3.Dot(transform.right, toEnemy.normalized);
                float amtToMove = projectileSpeed * Time.deltaTime;
                transform.Translate(new Vector3(angleToEnemyX, angleToEnemyY, 0) * amtToMove, Space.World);
                if (transform.position.y > 7.5f)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                GameObject nearest = GameObject.Find("Main Boss");
                Vector3 toEnemy = nearest.transform.position - transform.position;
                float angleToEnemyY = Vector3.Dot(transform.up, toEnemy.normalized);
                float angleToEnemyX = Vector3.Dot(transform.right, toEnemy.normalized);
                float amtToMove = projectileSpeed * Time.deltaTime;
                transform.Translate(new Vector3(angleToEnemyX, angleToEnemyY, 0) * amtToMove, Space.World);
                if (transform.position.y > 7.5f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            // update game ui
            Player.score += 100;
            Player.UpdateStats();
            if (Player.score >= Player.scoreToGoal)
            {
                Player.ResetStats();
                SceneManager.LoadScene("Win");
            }
            else
            {

                // play destroy animation 
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                enemy.minSpeed += 0.5f;
                enemy.maxSpeed += 1.0f;
                enemy.SetPositionAndSpeed();
                Destroy(gameObject);
            }

        }

        else if (other.tag == "EnemyShip")
        {
            Player.score += 100;
            Player.UpdateStats();
            EnemyShip enemy = other.GetComponent<EnemyShip>();
            if (Player.score >= Player.scoreToGoal)
            {
                Player.ResetStats();
                SceneManager.LoadScene("Win");
            }
            else
            {

                // play destroy animation
                if (enemy != null)
                {
                    Instantiate(explosionPrefab, transform.position, transform.rotation);
                    enemy.minSpeed += 0.5f;
                    enemy.maxSpeed += 1.0f;
                    enemy.SetPositionAndSpeed();
                    Destroy(gameObject);
                }
            }
        }

        else if (other.tag == "minienemies")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }


        else if (other.tag == "Advanced Enemy")
        {
            // update game ui
            PlayLastLevel.increaseEnemyCount();
            AdvancedEnemy enemy = other.GetComponent<AdvancedEnemy>();

            Instantiate(explosionPrefab, transform.position, transform.rotation);
            enemy.minSpeed += 0.5f;
            enemy.maxSpeed += 1.0f;
            enemy.SetPositionAndSpeed();
            Destroy(gameObject);
        }


        else if (other.tag == "Main Boss")
        {
            if (MainBoss.getPhase() == 1 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                MainBoss.setState(MainBoss.State.SecondPhase);
                MainBoss.setPhase(2);
                MainBoss.doAgain();
                PlayLastLevel.nulifyEnemyCount();
                Destroy(gameObject);
            }
            else if (MainBoss.getPhase() == 2 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                MainBoss.setState(MainBoss.State.SecondPhase);
                MainBoss.setPhase(3);
                Destroy(gameObject);
            }
            else if (MainBoss.getPhase() == 3 && !MainBoss.cantBeKilled())
            {
                Instantiate(explosionPrefab, transform.position, transform.rotation);
                PlayLastLevel.changeDecision();
                PlayLastLevel.setState(PlayLastLevel.State.CutScene);
                Destroy(GameObject.Find("Main Boss"));
                Destroy(GameObject.Find("Fight"));
                Destroy(gameObject);
            }
        }


    }

}
