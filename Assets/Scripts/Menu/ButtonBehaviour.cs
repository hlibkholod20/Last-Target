using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonBehaviour : MonoBehaviour
{
  public void LoadLevelByIndex (int levelIndex)
    {
        Player.ResetStats();
        SceneManager.LoadScene(levelIndex);
    }

  public void LoadLevelByName (string levelName)
    {
        Player.ResetStats();
        SceneManager.LoadScene(levelName); 
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name == "Main Menu")
            {
                if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
                else
                {
                    Player.ResetStats();
                    LoadLevelByName("Level1");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Win")
            {
                if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
                else
                {
                    if (Player.getLevel() == 1)
                    {
                        Player.ResetStats();
                        Player.setLevel(2);
                        SceneManager.LoadScene("Level2");
                    }
                    else if (Player.getLevel() == 2)
                    {
                        Player.ResetStats();
                        Player.setLevel(0);
                        SceneManager.LoadScene("Level3");
                    }
                }

            }
            else if (SceneManager.GetActiveScene().name == "Lose")
            {
                Player.ResetStats();
                PlayLastLevel.ResetStats();
                Application.Quit();
            }
        } 
    }

}
