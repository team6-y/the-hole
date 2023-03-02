using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitScript : MonoBehaviour
{
    public Boolean isLoaded = false;
    void OnTriggerEnter(Collider other)
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync("Environment_Underwater", LoadSceneMode.Additive);
            isLoaded = true;
        }
    }
}
