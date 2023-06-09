using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    [Header("Singleton")]
    public static menuManager instance;

    [Header("User Interface")]
    public GameObject activeMenu;
    public GameObject mainMenu;
    public GameObject gamemodesMenu;
    public GameObject eliminateLvlsMenu;
    public GameObject surviveLvlsMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject pauseMenu;
    public GameObject pauseDifficulty;
    public GameObject pauseOptionsMenu;
    public GameObject loseMenu;
    public GameObject loseRespawn;
    public GameObject winMenu;
    public GameObject difficultyMenu;
    public GameObject difficultyEasy;
    public GameObject difficultyNormal;
    public GameObject difficultyHard;
    public GameObject pauseDifficultyEasy;
    public GameObject pauseDifficultyNormal;
    public GameObject pauseDifficultyHard;

    [Header("Levels")]
    public GameObject eliminateLvlOne;
    public GameObject eliminateLvlTwo;
    public GameObject eliminateLvlThree;
    public GameObject surviveLvlOne;

    [Header("Screen Effects")]
    public GameObject fadeBlackObj;
    public GameObject damageFlash;
    public GameObject lowHealth;

    [Header("First Selection Options")]
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject gamemodesFirst;
    [SerializeField] private GameObject eliminateLvlsFirst;
    [SerializeField] private GameObject survivalLvlsFirst;
    [SerializeField] private GameObject optionsFirst;
    [SerializeField] private GameObject creditsFirst;
    [SerializeField] private GameObject pauseFirst;
    [SerializeField] private GameObject pauseOptionsFirst;
    [SerializeField] private GameObject winFirst;
    [SerializeField] private GameObject loseFirst;
    [SerializeField] private GameObject difficultyFirst;
    [SerializeField] private GameObject pauseDifficultyFirst;


    /**
    * Awake
    */
    void Awake() {
        instance = this; //Only one instance of singleton
        fadeBlackObj.SetActive(true);
    }

    /**
    * Start
    */
    void Start() {
        DeactiveAllMenus();
        if (SceneManager.GetActiveScene().name == "LandingScene") OpenMain();
    }

    /**
    * Update
    */
    void Update() {
        if (inputManager.instance.MenuOpenInput && !fadeBlackObj.activeSelf && activeMenu != loseMenu && activeMenu != winMenu && SceneManager.GetActiveScene().name != "LandingScene") {
            if (!gameManager.instance.isPaused) {
                gameManager.instance.PauseState();
                OpenPause();
            } else gameManager.instance.UnpauseState();
        }
    }

    /*
     * Menu Handling
     */

    public void DeactiveAllMenus() {
        // Set All Menus to False
        if(activeMenu) activeMenu.SetActive(false);
        activeMenu = null;
        EventSystem.current.SetSelectedGameObject(null);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gamemodesMenu.SetActive(false);
        surviveLvlsMenu.SetActive(false);
        eliminateLvlsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        pauseOptionsMenu.SetActive(false);
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        // Screen Effects
        damageFlash.SetActive(false);
        lowHealth.SetActive(false);
    }

    private void OpenMenu(GameObject menu, GameObject firstSelected) {
        DeactiveAllMenus();
        EventSystem.current.SetSelectedGameObject(firstSelected);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    public void OpenPause() {
        OpenMenu(pauseMenu, pauseFirst);
    }

    public void OpenPauseOptions() {
        OpenMenu(pauseOptionsMenu, pauseOptionsFirst);
    }

    public void OpenPauseDifficulty() {
        HandleDifficultyBorders();
        OpenMenu(pauseDifficulty, pauseDifficultyFirst);
    }

    public void OpenLose() {
        // Disable Respawn if on survival
        if(SceneManager.GetActiveScene().name == "Survive") {
            loseRespawn.GetComponent<Button>().interactable = false;
        }
        OpenMenu(loseMenu, loseFirst);
    }

    public void OpenWin() {
        OpenMenu(winMenu, winFirst);
    }

    public void OpenMain() {
        OpenMenu(mainMenu, mainMenuFirst);
    }

    public void OpenGamemodes() {
        OpenMenu(gamemodesMenu, gamemodesFirst);
    }

    public void OpenEliminateLvls() {
        // Handle Displayed Progress
        int level = playerPrefsManager.instance.GetLevel();

        // Load Level 1 Highscore
        int lvlOneHS = playerPrefsManager.instance.GetHighscore("SampleScene");
        eliminateLvlOne.transform.Find("High Score").GetComponent<TextMeshProUGUI>().text = "Highscore: " + lvlOneHS.ToString();

        // Unlock Levels
        if (level > 1) UnlockLevel(eliminateLvlTwo, "exterminate");

        // Load the Menu
        gameManager.instance.LoadHighScore();
        OpenMenu(eliminateLvlsMenu, eliminateLvlsFirst);
    }

    public void OpenSurvivalLvls() {
        // Load Level 1 Highscore
        int lvlOneHS = playerPrefsManager.instance.GetHighscore("Survive");
        surviveLvlOne.transform.Find("High Score").GetComponent<TextMeshProUGUI>().text = "Highscore: " + lvlOneHS.ToString();

        OpenMenu(surviveLvlsMenu, survivalLvlsFirst);
    }

    public void OpenOptions() {
        OpenMenu(optionsMenu, optionsFirst);
    }

    public void OpenCredits() {
        OpenMenu(creditsMenu, creditsFirst);
    }
    public void OpenDifficulty()
    {
        HandleDifficultyBorders();
        OpenMenu(difficultyMenu, difficultyFirst);
    }

    /**
     * Difficulty Border Feedback
     */
    public void HandleDifficultyBorders() {
        // Disable all borders
        difficultyEasy.GetComponent<Image>().enabled = false;
        difficultyNormal.GetComponent<Image>().enabled = false;
        difficultyHard.GetComponent<Image>().enabled = false;
        pauseDifficultyEasy.GetComponent<Image>().enabled = false;
        pauseDifficultyNormal.GetComponent<Image>().enabled = false;
        pauseDifficultyHard.GetComponent<Image>().enabled = false;

        // Set Border
        int currDifficulty = playerPrefsManager.instance.GetDifficulty();
        if (currDifficulty == 2) {
            difficultyNormal.GetComponent<Image>().enabled = true;
            pauseDifficultyNormal.GetComponent<Image>().enabled = true;
        } else if (currDifficulty == 3) {
            difficultyHard.GetComponent<Image>().enabled = true;
            pauseDifficultyHard.GetComponent<Image>().enabled = true;
        } else {
            difficultyEasy.GetComponent<Image>().enabled = true;
            pauseDifficultyEasy.GetComponent<Image>().enabled = true;

        }
    }

    /**
     * Unlock Level
     */
    public void UnlockLevel(GameObject button, string scene) {
        // Load Highscore
        int highscore = playerPrefsManager.instance.GetHighscore(scene);
        button.transform.Find("High Score").GetComponent<TextMeshProUGUI>().text = "Highscore: " + highscore.ToString();

        // Enable Button
        button.GetComponent<Button>().interactable = true;
        button.transform.Find("Locked").GetComponent<Image>().enabled = false;
        button.transform.Find("Icon").GetComponent<Image>().enabled = true;

        // Reset Color Multiplier
        ColorBlock c = button.GetComponent<Button>().colors;
        c.colorMultiplier = 1;
        button.GetComponent<Button>().colors = c;
    }

    /*
     * Screen Transitions
     */

    // Fade To Black Screen
    public IEnumerator FadeBlack(bool fadeBlack = true) {
        Color fadeColor = fadeBlackObj.GetComponent<Image>().color;
        float colorAmt;

        if (fadeBlack) {
            if (!fadeBlackObj.activeSelf) fadeBlackObj.SetActive(true);
            // Fade Black
            while (fadeColor.a < 1) {
                colorAmt = fadeColor.a + (5 * Time.deltaTime);
                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, colorAmt);
                fadeBlackObj.GetComponent<Image>().color = fadeColor;
                yield return null;
            }

        } else {
            // Fade Transparent
            while (fadeColor.a > 0) {
                colorAmt = fadeColor.a - (5 * Time.deltaTime);
                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, colorAmt);
                fadeBlackObj.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
            if (fadeBlackObj.activeSelf) fadeBlackObj.SetActive(false);
        }

        yield return new WaitForEndOfFrame();
    }


    // Wait to Unfade Black Screen
    public IEnumerator WaitToUnfade() {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeBlack(false));
    }
}
