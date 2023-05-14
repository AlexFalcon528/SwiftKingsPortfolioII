using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnFunctions : MonoBehaviour
{
    public void Resume() {
        gameManager.instance.UnpauseState(); // Unpause the Game
        gameManager.instance.isPaused = false; // Set isPaused to False
    }

    public void Restart() {
        Resume(); // Run the Resume Function above
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void Exit() {
        Application.Quit(); // Quit the application
    }

    public void Respawn() {
        gameManager.instance.UnpauseState(); // Unpause the game
        gameManager.instance.pScript.SpawnPlayer(); // Spawn the player through the player controller
    }


    public void Play() {
        SceneManager.LoadScene("SampleScene");
    }

    public void Options() {
        //
    }
}
