using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Collectible : MonoBehaviour
{
    GameFlowController m_myGameFlowController;


    private void Awake()
    {
        m_myGameFlowController = FindObjectOfType<GameFlowController>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (this.gameObject.tag.Equals("Letter"))
            {
                m_myGameFlowController.WordCollected(this.GetComponentInChildren<TextMeshPro>().text[0]);
                Destroy(gameObject);
            } else
            {
                m_myGameFlowController.BonusCollected();
                Destroy(gameObject);
            }
        }
    }
}