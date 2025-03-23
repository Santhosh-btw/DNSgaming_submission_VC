using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestartScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject tryAgain;
    [SerializeField] private GameObject returnMainMenu;

    private bool playerIsDead;

    private void Awake() {
        Application.targetFrameRate = 30;
    }

    void Update(){
        playerIsDead = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().isDead;
        if (playerIsDead){
                background.SetActive(true);
                GameOverText.SetActive(true);
                tryAgain.SetActive(true);
                returnMainMenu.SetActive(true);
        }
    }

    public void RestartGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

}
