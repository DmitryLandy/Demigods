using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        //Gets the next scene (game) in the scene queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
