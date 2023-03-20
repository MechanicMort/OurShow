using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyItem : MonoBehaviour
{
    public bool endOfRound = false;
    public GameObject myPos;
    public GameObject goToPos;
    public GameObject myCanvas;
    public string myItem;
    public float timeIncreaseOnIncorrect;
    public bool correctAnswer;
    public LineRenderer lineRenderer;
    public Vector3 IDUBBOl;
    public bool letterDone = false;
    
    public TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(myPos.transform.position.x, myPos.transform.position.y, -0.5f), 2.5f * Time.deltaTime);
        transform.LookAt(myPos.transform.position, IDUBBOl);
        if (Vector3.Distance(transform.position, new Vector3(myPos.transform.position.x, myPos.transform.position.y, -0.5f)) > 0.1f)
        {
            myCanvas.SetActive(false);
        }
        else
        {
            //tell game controller at location
            if (!letterDone)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<FillTheGapManager>().ThisLetterDone(endOfRound);
               
                letterDone = true;
                myCanvas.SetActive(true);
            }
       
        }
  

        textMeshProUGUI.text = myItem;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(0, -7, -0.4f));
    }
    public void Start()
    {
        goToPos = GameObject.FindGameObjectWithTag("GOTO");
    }


    public void RoundDone()
    {
        myPos = goToPos;
    }

    public void ClickedOn()
    {
        if (correctAnswer)
        {
            myPos = goToPos;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<FillTheGapManager>().CorrectAnswer(myItem);
           // gameObject.SetActive(false);
        }
        else
        {

            myPos = goToPos;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<FillTheGapManager>().time += timeIncreaseOnIncorrect;
            //gameObject.SetActive(false);
        }
    }
}
