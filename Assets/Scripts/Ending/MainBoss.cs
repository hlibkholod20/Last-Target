using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoss : MonoBehaviour
{
    public enum State
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        CutScene
    }
    private static State state = State.CutScene;
    [SerializeField]
    private float speed;
    private static int phase = 0;
    private static bool hasDone = false;
    private static bool unkillable = true;
    private float direction;
    private bool[] luck = new bool[30];
    private int score = 0;

    void Start()
    {
        luck[16] = true;
    }
    // Update is called once per frame
    void Update()
    {
        StartFight();
        if (state == State.ThirdPhase) transform.position = new Vector3(Random.Range(-5, 5), transform.position.y, 0);

            if (PlayLastLevel.getEnemiesKilled() == 10)
        {
            state = State.CutScene;
            if (transform.position.z < 0)
            {
                float amtToMove = speed * Time.deltaTime;
                if (phase == 1) transform.Translate(-amtToMove * 1.3f, -amtToMove * 1.4f, -amtToMove * 1.2f);
                else transform.Translate(amtToMove * 1.3f, -amtToMove * 1.4f, -amtToMove * 1.2f);
                unkillable = false;
            }
        }

        if (transform.position.x > 16) transform.position = new Vector3(-16, 0, 0);
        else if (transform.position.x < -16) transform.position = new Vector3(16, 0, 0);

        score = (score + 1) % 30;
    }

    public void StartFight()
    {
        if (state == State.FirstPhase)
        {
            unkillable = true;
            if (transform.position.y >= 5.5f)
            {
                float amtToMove = speed * Time.deltaTime;
                transform.Translate(amtToMove * 1.3f, amtToMove * 1.4f, amtToMove * 1.2f);
            }
            if (!hasDone)
            {
                AdvancedEnemy.setState(AdvancedEnemy.State.Active);
                GameObject.Find("Advanced Enemy").GetComponent<AdvancedEnemy>().SetPositionAndSpeed();
                hasDone = !hasDone;
            }
            
        }
        else if (state == State.SecondPhase)
        {
            unkillable = true;
            if (transform.position.y >= 5.5f)
            {
                float amtToMove = speed * Time.deltaTime;
                transform.Translate(-amtToMove * 1.3f, amtToMove * 1.4f, amtToMove * 1.2f);
            }
            if (!hasDone)
            {
                AdvancedEnemy.setState(AdvancedEnemy.State.Active);
                GameObject.Find("Advanced Enemy").GetComponent<AdvancedEnemy>().SetPositionAndSpeed();
                hasDone = !hasDone;
            }

        }
        else if (state == State.ThirdPhase) {
            unkillable = true;
            if (transform.position.y >= 5.5f)
            {
                float amtToMove = speed * Time.deltaTime;
                transform.Translate(0, -amtToMove, 0);
            }
            unkillable = false;
        }
    }

    public static void setState(State sta)
    {
        state = sta;
    }

    public static int getPhase()
    {
        return phase;
    } 

    public static void setPhase(int ph)
    {
        phase = ph;
    }

    public static void doAgain()
    {
        hasDone = !hasDone;
    } 

    public static void ifItBleedsItCanBeKilled()
    {
        unkillable = !unkillable;
    }

    public static bool cantBeKilled()
    {
        return unkillable;
    }

    private void setDirection()
    {
        direction = Random.Range(-5, 5);
    }
}
