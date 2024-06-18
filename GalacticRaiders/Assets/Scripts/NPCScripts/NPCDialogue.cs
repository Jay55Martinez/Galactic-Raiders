using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    int textIndex;
    public Text textBox;
    public float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        textIndex = 0;
        textBox.gameObject.SetActive(false);
    }

    public void setDialogue(Dialogue dia) {
        dialogue = dia;
    }

    public void StartTalking() {
        InvokeRepeating("NextVoiceLine", duration, duration);
        textBox.gameObject.SetActive(true);
        textBox.text = dialogue.voicelines[textIndex];
    }

    public void StopTalking() {
        textBox.gameObject.SetActive(false);
        CancelInvoke("NextVoiceLine");
        textIndex = 0;
    }

    void NextVoiceLine() {
        if (textIndex < dialogue.voicelines.Length - 1) {
            textIndex++;
        } else {
            Invoke("StopTalking", duration);
        }
        textBox.text = dialogue.voicelines[textIndex];
    }

    public void ResetDialogue() {
        textIndex = 0;
    }
}
