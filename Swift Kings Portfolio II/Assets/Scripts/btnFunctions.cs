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
        gameManager.instance.HandleReturnMenu();
        StartCoroutine(SceneTransition("LandingScene")); // Load the Main Menu

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
        StartCoroutine(SceneTransition("SampleScene")); // Load the Sample Scene (Elminate All Enmies Gamemode)
    }

    public void PlaySurvive() {
        StartCoroutine(SceneTransition("Survive")); // Load the Sample Scene (Survive Waves of Enemies Gamemode)
        gameManager.instance.nextWave = true;
    }

    public void Options() {
        // TODO: Expand for Game Settings (i.e. reticle color)
    }

    public void Back() {
        gameManager.instance.gamemodes.SetActive(false);
        gameManager.instance.mainMenu.SetActive(true);
    }


    /**
     * Handle Scene Transitions
     */
    public IEnumerator SceneTransition(string SceneName) {
        StartCoroutine(gameManager.instance.FadeBlack(true));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneName);
    }

    public IEnumerator RestartTransition() {
        StartCoroutine(gameManager.instance.FadeBlack(true));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        StartCoroutine(gameManager.instance.FadeBlack(false));
    }
}
