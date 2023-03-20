using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRandomizer : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
         int randy =  Random.RandomRange(1, 9);
        animator.SetInteger("Kite", randy);

    }

   
}
