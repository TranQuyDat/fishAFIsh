using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI txtbox;
    [SerializeField] private string[] dialogue;
    [SerializeField] private float speedtxt;

    private int cur_dialogueindex = 0;
    private bool isEmptytxtbox = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(cur_dialogueindex < dialogue.Length)
        {
            Startdialogue();
        }
        else
        {
        }
    }

    public void Startdialogue()
    {
        Emptydialogue();
        if(isEmptytxtbox)
            StartCoroutine(readtxt());
    }

    IEnumerator readtxt()
    {
        isEmptytxtbox = false;
        char c = ' ';
        for (int i = 0; i< dialogue[cur_dialogueindex].ToCharArray().Length;i++ )
        {
            c = (char)dialogue[cur_dialogueindex].ToCharArray().GetValue(i);
            txtbox.text += c;
            yield return new WaitForSeconds(speedtxt);
           
        }
        txtbox.text += " ";
    }

    public void nextLine()
    {
        if (cur_dialogueindex < dialogue.Length-1)
        {
            cur_dialogueindex++;
            StartCoroutine(readtxt());
        }
        else return;
    }

    public void Emptydialogue()
    {
        cur_dialogueindex = 0;
        txtbox.text = "";
        isEmptytxtbox = true;
    }
}
