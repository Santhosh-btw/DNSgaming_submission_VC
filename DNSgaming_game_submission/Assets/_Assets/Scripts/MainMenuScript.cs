using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    public void QuitGame(){
        Application.Quit();
    }

    public void StartGame(){
        SceneManager.LoadScene("GameScene");
    }

}
