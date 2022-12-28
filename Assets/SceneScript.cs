using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public void SelectScene()
    {
        switch(gameObject.name)
        {
            case "start":
                SceneManager.LoadScene("Game");
                break;
            case "Main":
                SceneManager.LoadScene("Main Menu");
                break;
            case "NEXT BTN":
                SceneManager.LoadScene("Level2");
                break;
            case "StartLab":
                SceneManager.LoadScene("Room1");
                break;
            case "room2":
                SceneManager.LoadScene("Room2");
                break;
        }
        
    }
}
