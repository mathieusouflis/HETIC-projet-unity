using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameMenu : MonoBehaviour
{
    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("scene switched");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
    
    public void Multi()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        Debug.Log("Scene switched to multiplayer");
    }
}
