using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnFunctions : MonoBehaviour {
 
    /**
     * Pause Menu
     */

    public void Resume() {
        gameManager.instance.UnpauseState(); // Unpause the Game
    }

    public void Restart() {
        StartCoroutine(RestartTransition());
        gameManager.instance.UnpauseState();
        gameManager.instance.wave = 1;
    }

    public void OpenPauseOptions() {
        menuManager.instance.OpenPauseOptions();
    }

    public void PauseBack() {
        menuManager.instance.OpenPause();
    }

    public void LoseBack() {
        menuManager.instance.OpenLose();
    }

    public void ReturnMenu() {
        StartCoroutine(SceneTransition("LandingScene")); // Load the Main Menu
        gameManager.instance.HandleReturnMenu();
    }

    public void Respawn() {
        gameManager.instance.UnpauseState(); // Unpause the game
        gameManager.instance.currentScore = 0;
        gameManager.instance.pScript.SpawnPlayer(); // Spawn the player through the player controller
    }

    public void PauseDifficulty() {
        menuManager.instance.OpenPauseDifficulty();
    }

    public void Continue() {
        // Get Active Scene
        string activeScene = SceneManager.GetActiveScene().name;

        // Eliminate
        if (activeScene == "SampleScene") {
            StartCoroutine(SceneTransition("Exterminate"));
            gameManager.instance.UnpauseState();
        } else if (activeScene == "Exterminate") { // Theres no level past this one at the moment
            StartCoroutine(SceneTransition("Exterminate"));
            gameManager.instance.UnpauseState();
        }
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

    public void PlayEliminate() {
        menuManager.instance.OpenEliminateLvls();
    }

    public void EliminateLvl1() {
        StartCoroutine(SceneTransition("SampleScene")); // Load the Sample Scene (Eliminate All Enemies Gamemode)
    }

    public void EliminateLvl2() {
        StartCoroutine(SceneTransition("Exterminate")); // Load the Sample Scene (Eliminate All Enemies Gamemode)
    }


    public void EliminateLvl3() {
        StartCoroutine(SceneTransition("SampleScene")); // Load the Sample Scene (Eliminate All Enemies Gamemode)
    }


    public void PlaySurvive() {
        menuManager.instance.OpenSurvivalLvls();
    }

    public void SurviveForestMaze() {
        StartCoroutine(SceneTransition("Survive")); // Load the Sample Scene (Survive Waves of Enemies Gamemode)
    }

    public void Options() {
        menuManager.instance.OpenOptions();
    }

    public void Credits() {
        menuManager.instance.OpenCredits();
    }

    public void MainMenuBack() {
        menuManager.instance.OpenMain();
    }

    public void Difficulty() {
        menuManager.instance.OpenDifficulty();
    }

    public void Easy() {
        playerPrefsManager.instance.SetDifficulty(1);
        gameManager.instance.difficulty = 1;
        menuManager.instance.HandleDifficultyBorders();
    }

    public void Medium()
    {
        playerPrefsManager.instance.SetDifficulty(2);
        gameManager.instance.difficulty = 2;
        menuManager.instance.HandleDifficultyBorders();
    }

    public void Hard()
    {
        playerPrefsManager.instance.SetDifficulty(3);
        gameManager.instance.difficulty = 3;
        menuManager.instance.HandleDifficultyBorders();
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
    }
}
