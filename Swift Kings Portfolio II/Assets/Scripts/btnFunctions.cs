using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnFunctions : MonoBehaviour
{
    public void Resume() {
        gameManager.instance.UnpauseState();
        gameManager.instance.isPaused = false;
    }

    public void Restart() {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Respawn() {
        gameManager.instance.UnpauseState();
        gameManager.instance.pScript.SpawnPlayer();
    }
}
