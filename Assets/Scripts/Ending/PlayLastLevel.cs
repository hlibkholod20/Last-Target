using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayLastLevel : MonoBehaviour
{
    [Header("Movement Variables:")]
    [Range(0, 20)]
    public float playerSpeed = 10.0f;
    [Header("Rotation Variables:")]
    public float maxRotationX = 10.0f;
    public float maxRotationY = 10.0f;
    public float rotationSpeed = 10.0f;
    [Header("Boost:")]
    public float playerBoost = 5.0f;
    [Header("Fire Button:")]
    [SerializeField]
    private KeyCode fireButton = KeyCode.Space;
    [Header("Projectile:")]
    [SerializeField]
    private float projectileOffset = 0.25f;
    public GameObject firstProjectilePrefab;
    public GameObject secondProfectilePrefab;
    public GameObject thirdProjectilePrefab;
    private GameObject tempProjectile;
    [Header("Player Explosion: ")]
    [SerializeField]
    private GameObject playerExplosion;
    [Header("Goal of this level:")]
    public static GameObject finalMusic;
    public Transform endPoint;
    public int toGoal = 10000;
    public static int scoreToGoal;
    public static int score = 0;
    public static int highscore = 0;
    //Gameplay parts 
    public static int missed = 0;
    public static int rockets = 3;
    public static float lives = 3.0f;
    public static Text playerStats;
    //for submethods
    private float shipInvisibleTime = 1.5f;
    private static bool decided = false;
    public enum State
    {
        Playing,
        Explosion,
        Invincible,
        Shield,
        CutScene,
        Choice
    };
    private static State state = State.CutScene;
    //blinking 
    private float shipMoveOnToScreenSpeed = 5f;
    private float blinkRate = .1f;
    private int numberOfTimesToBlink = 10;
    private int blinkCount;
    //can be useful
    private bool[] gotShot = new bool[10];
    private int littleTroubles = 0;
    private static int enemyCount = 0;
    private bool fightStarted = false;
    private int clickCount = 0;
    [Header("Ending settings: ")]
    public GameObject posEnding;
    public GameObject negEnding;
    public GameObject start;


    // Start is called before the first frame update
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level3" && Player.getLevel() == 0)
        {
            playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
            scoreToGoal = toGoal;
            UpdateStats();
            tempProjectile = firstProjectilePrefab;
            finalMusic = GameObject.Find("Final Music");
            gotShot[9] = true;
        }
    }

    //updates states on the left-top side if the screen
    public static void UpdateStats()
    {
        if (SceneManager.GetActiveScene().name == "Level3" && Player.getLevel() == 0)
        {
            playerStats.text = "Lives: " + lives.ToString()
                                + "\nMissed: " + missed.ToString()
                                + "\nRockets: " + rockets.ToString()
                                + "\nShield: " + LastLevelHIeld.shieldPoints.ToString() + "%";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayLastLevel.getState() != State.Explosion && PlayLastLevel.getState() != State.CutScene)
        {
            //set speed characteristics
            float speed = playerSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed += playerBoost;
            }

            //change weapon system
            if (Input.GetKey("1"))
            {
                firstProjectilePrefab = thirdProjectilePrefab;
                clickCount = 1;
                highscore = score;
            }
            if (Input.GetKey("2"))
            {
                firstProjectilePrefab = tempProjectile;
                clickCount = 0;
                highscore = score;
            }

            //move Player depending on input left or right 
            float amtToMove = speed * Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.Translate(Vector3.right * amtToMove, Space.World);

            //calculate rotation depending on moving inputs
            Quaternion targetRotation = Quaternion.Euler(Input.GetAxisRaw("Vertical") * maxRotationX, Input.GetAxisRaw("Horizontal") * maxRotationY, 0);

            //set rotation smoothly  
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //move Player depending on input up or down 
            float amtToMove1 = speed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.Translate(Vector3.up * amtToMove1, Space.World);

            //get choice 
            if (PlayLastLevel.getState() == State.Choice && transform.position.y >= endPoint.position.y && !fightStarted)
            {
                Instantiate(playerExplosion, transform.position, Quaternion.identity);
                Destroy(posEnding);
                PlayLastLevel.setState(State.CutScene);
                decided = true;
                GameObject.Find("Plane").GetComponent<AudioSource>().enabled = false;
                finalMusic.GetComponent<AudioSource>().enabled = true;
                Destroy(this.gameObject);
            }

            //check if shield destroyed
            if (LastLevelHIeld.isDestroyed()) PlayLastLevel.setState(State.Playing);

            //screen wrap 
            if (transform.position.x < -12.7f) transform.position = new Vector3(12.7f, transform.position.y, transform.position.z);
            if (transform.position.x > 12.7f) transform.position = new Vector3(-12.7f, transform.position.y, transform.position.z);
            if (transform.position.y < -8.0f) transform.position = new Vector3(transform.position.x, 7.8f, transform.position.z);
            if (transform.position.y > 7.8f) transform.position = new Vector3(transform.position.x, -8.0f, transform.position.z);

            //fire and if the case another choise
            if (Input.GetKeyDown(fireButton)) // what's the difference between GetKey, GetKeyDown, GetKeyUp?
            {
                //making of choise
                if (PlayLastLevel.getState() == State.Choice)
                {
                    fightStarted = true;
                    Destroy(negEnding);
                    MainBoss.setState(MainBoss.State.FirstPhase);
                    MainBoss.setPhase(1);
                    GameObject.Find("Plane").GetComponent<AudioSource>().enabled = false;
                    GameObject.Find("Fight").GetComponent<AudioSource>().enabled = true;
                    PlayLastLevel.setState(State.Shield);
                }

                //behaviour if weapon-systems
                if (score >= highscore + 1800 && clickCount == 0)
                {
                    Vector3 position = transform.position + new Vector3(0, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Instantiate(firstProjectilePrefab, position, Quaternion.identity);// Quaternion.identity = Quaternion.Euler(0,0,0); that means that no rotations will have place by instantiating
                    Vector3 position2 = transform.position + new Vector3(0.75f, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Vector3 position3 = transform.position + new Vector3(-0.75f, 0.6f * transform.localScale.y + projectileOffset, 0);
                    Instantiate(secondProfectilePrefab, position2, Quaternion.Euler(0, 0, -30.0f));
                    Instantiate(secondProfectilePrefab, position3, Quaternion.Euler(0, 0, 30.0f));
                }
                else
                {
                    if (clickCount == 0 || rockets > 0)
                    {
                        Vector3 position = transform.position + new Vector3(0, 0.6f * transform.localScale.y + projectileOffset, 0);
                        Instantiate(firstProjectilePrefab, position, Quaternion.identity);
                        if (clickCount == 1) rockets--;
                    }
                }
            }
        }

    }

    //detects entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (PlayLastLevel.getState() == State.Playing)
        {
            if (other.tag == "ProjectileEnemy")
            {
                if (gotShot[littleTroubles])
                {
                    lives--;
                    UpdateStats();
                    Destroy(other.gameObject);
                    StartCoroutine(DestroyShip());
                    littleTroubles = 0;
                }
                else
                {
                    Destroy(other.gameObject);
                    littleTroubles++;
                }
            }

            else if (other.tag == "Advanced Enemy")
            {
                lives--;
                UpdateStats();
                StartCoroutine(DestroyShip());

                // update enemy
                AdvancedEnemy enemy = other.gameObject.GetComponent<AdvancedEnemy>();
                enemy.SetPositionAndSpeed();

            }

            else if (other.tag == "Main Boss")
            {
                lives--;
                UpdateStats();
                StartCoroutine(DestroyShip());
            }
        }
    }

    private IEnumerator DestroyShip()
    {
        blinkCount = 0;
        // changing of state 
        PlayLastLevel.setState(State.Explosion);
        // show explosion
        Instantiate(playerExplosion, transform.position, Quaternion.identity);

        transform.position = new Vector3(0, -5.5f, transform.position.z);

        yield return new WaitForSeconds(shipInvisibleTime);

        //do something after 1.5 sec
        if (lives > 0)
        {
            highscore = score;
            while (transform.position.y < -2.2f)
            {
                // move the ship up
                float amtToMove = shipMoveOnToScreenSpeed * Time.deltaTime;
                transform.position = new Vector3(0, transform.position.y + amtToMove, transform.position.z);
                yield return 0;
            }
            PlayLastLevel.setState(State.Invincible);
            while (blinkCount < numberOfTimesToBlink)
            {
                gameObject.GetComponentInChildren<Renderer>().enabled = !gameObject.GetComponentInChildren<Renderer>().enabled;
                if (gameObject.GetComponentInChildren<Renderer>().enabled)
                {
                    blinkCount++;
                }
                yield return new WaitForSeconds(blinkRate);
            }
            PlayLastLevel.setState(State.Playing);
        }
        else
        {
            MainBoss.setPhase(0);
            MainBoss.setState(MainBoss.State.CutScene);
            AdvancedEnemy.setState(AdvancedEnemy.State.Sleeping);
            PlayLastLevel.ResetStats();
            SceneManager.LoadScene("Lose");
        }
    }

    public static void ResetStats()
    {
        PlayLastLevel.setState(State.CutScene);
        PlayLastLevel.enemyCount = 0;
        PlayLastLevel.score = 0;
        PlayLastLevel.lives = 3;
        PlayLastLevel.missed = 0;
        PlayLastLevel.highscore = 0;
        PlayLastLevel.rockets = 3;
        LastLevelHIeld.shieldPoints = 100;
    }

    public static bool getDecision()
    {
        return decided;
    }

    public static void changeDecision()
    {
        decided = !decided;
    }

    public static int getEnemiesKilled()
    {
        return enemyCount;
    }

    public static void increaseEnemyCount()
    {
        enemyCount++;
    }

    public static void nulifyEnemyCount()
    {
        enemyCount = 0;
    }

    public static void setState(State stateX)
    {
        state = stateX;
    }

    public static State getState()
    {
        return state;
    }
}
