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
        gameManager.instance.wave = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void ReturnMenu() {
        gameManager.instance.HandleReturnMenu();
        SceneManager.LoadScene("LandingScene"); // Load Menu Scene
    }

    public void Exit() {
        Application.Quit(); // Quit the application
    }

    public void Respawn() {
        gameManager.instance.UnpauseState(); // Unpause the game
        gameManager.instance.pScript.SpawnPlayer(); // Spawn the player through the player controller
    }


    /**
     * Main Menu
     * */
    public void Play() {
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.gamemodes.SetActive(true);
    }

    public void PlayElminate() {
        SceneManager.LoadScene("SampleScene"); // Load the Sample Scene (Elminate All Enmies Gamemode)
    }

    public void PlaySurvive() {
        SceneManager.LoadScene("Survive"); // Load the Survival Scene (Survival Wave Gamemode)
        gameManager.instance.nextWave = true;
    }

    public void Options() {
        // TODO: Expand for Game Settings (i.e. reticle color)
    }

    public void Back() {
        gameManager.instance.gamemodes.SetActive(false);
        gameManager.instance.mainMenu.SetActive(true);
    }
}
