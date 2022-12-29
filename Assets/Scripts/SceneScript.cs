using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public void SelectScene()
    {
        switch(gameObject.name)
        {
            case "start":
                SceneManager.LoadScene("Level1 QuizGame");
                break;
            
            case "NEXT BTN":
                SceneManager.LoadScene("Level2 LabGame");
                break;
            
            case "StartLab":
                SceneManager.LoadScene("CleanSet");
                break;
            
            case "room2":
                SceneManager.LoadScene("Room2");
                break;

            case "room3":
                SceneManager.LoadScene("Room3");
                break;
            
            case "room4":
                SceneManager.LoadScene("Tourniquet-Step1");
                break;


            //Temp-scene water simulator
            case "tempScene":
                SceneManager.LoadScene("LiquidScene");
                break;
            
            //Back to main menu
            case "Main":
                SceneManager.LoadScene("Main Menu");
                break;
            
            default:
                SceneManager.LoadScene("Main Menu");
                break;
        }
        
    }
}
