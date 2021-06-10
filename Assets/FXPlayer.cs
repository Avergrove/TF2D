using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPlayer : MonoBehaviour
{
    Animation animController;
    public String toPlay;
    public float selfDestructTime;

    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animation>();
        animController.wrapMode = WrapMode.Once;
        animController.Play(toPlay);
        GameObject.Destroy(this, selfDestructTime);
    }
}
