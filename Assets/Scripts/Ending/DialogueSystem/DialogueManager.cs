using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text text;
    public GameObject backGroundMusic;
    public GameObject dialogueWindow;
    private Queue<string> sentences = new Queue<string>();

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueWindow.transform.position = new Vector3(-6.0f, -2.0f, dialogueWindow.transform.position.z);
        nameText.text = dialogue.Name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentences();
    }

    public void DisplayNextSentences()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            string sentence = sentences.Dequeue();
            text.text = sentence;
        }
            
    }

    public void EndDialogue() {
        dialogueWindow.transform.position = new Vector3(-600.0f, -175.0f, transform.position.z);
        if (this.tag == "Final Dialogue" && PlayLastLevel.getState() == PlayLastLevel.State.CutScene)
        {
            MainBoss.setPhase(0);
            MainBoss.setState(MainBoss.State.CutScene);
            PlayLastLevel.ResetStats();
            SceneManager.LoadScene("Final Credits");
        }
        else PlayLastLevel.setState(PlayLastLevel.State.Choice);
    }
}
