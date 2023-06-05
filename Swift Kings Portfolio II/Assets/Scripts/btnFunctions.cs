using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class btnFunctions : MonoBehaviour {
 
    /**
     * Pause Menu
     */

    public void Resume() {
        gameManager.instance.UnpauseState(); // Unpause the Game
        gameManager.instance.isPaused = false; // Set isPaused to False
    }

    public void Restart() {
        StartCoroutine(RestartTransition());
        Resume(); // Run the Resume Function above
        gameManager.instance.wave = 1;
    }

    public void ReturnMenu() {
        StartCoroutine(SceneTransition("LandingScene")); // Load the Main Menu
        gameManager.instance.HandleReturnMenu();
    }

    public void Respawn() {
        gameManager.instance.UnpauseState(); // Unpause the game
        gameManager.instance.pScript.SpawnPlayer(); // Spawn the player through the player controller
    }

    public void Exit() {
        Application.Quit(); // Quit the application
    }

    /**
     * Main Menu
     * */
    public void Play() {
        menuManager.instance.OpenGamemodes();
    }

    public void PlayElminate() {
        StartCoroutine(SceneTransition("SampleScene")); // Load the Sample Scene (Elminate All Enmies Gamemode)
    }

    public void PlaySurvive() {
        StartCoroutine(SceneTransition("Survive")); // Load the Sample Scene (Survive Waves of Enemies Gamemode)
        gameManager.instance.nextWave = true;
    }

    public void Options() {
        // TODO: Expand for Game Settings (i.e. reticle color)
    }

    public void Credits() {
        // TODO: Expand for Game Credits
    }

    public void MainMenuBack() {
        menuManager.instance.OpenMain();
    }


    /**
     * Handle Scene Transitions
     */
    public IEnumerator SceneTransition(string SceneName) {
        StartCoroutine(menuManager.instance.FadeBlack(true));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator RestartTransition() {
        StartCoroutine(menuManager.instance.FadeBlack(true));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        StartCoroutine(menuManager.instance.FadeBlack(false));
    }
}
