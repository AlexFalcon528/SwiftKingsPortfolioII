using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [Header("~~~~~~~Singleton~~~~~~~")]
    public static gameManager instance;
    [Header("\n~~~~~~~Player~~~~~~~")]
    public GameObject player;
    public playerController pScript;
    public GameObject spawnPoint;
    [Header("\n~~~~~~~User Interface~~~~~~~")]
    public GameObject activeMenu;
    public GameObject pause;
    public GameObject lose;
    public bool isPaused;
    float originalTimeScale;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this; //Only one instance of singleton
        player = GameObject.FindWithTag("Player"); //Find player
        spawnPoint = GameObject.FindWithTag("Spawnpoint"); //Find spawnpoint
        originalTimeScale = Time.timeScale; //Save original time scale for later use
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")&&activeMenu == null) //When pressing cancel button and not in a menu already
        {
            PauseState();//Pause game
            activeMenu = pause; //Store pause menu as active menu
            activeMenu.SetActive(isPaused); //Activate active menu
            
        }
    }
    void PauseState()
    {
        isPaused = true;//Pause
        Time.timeScale = 0; //Stop physics and time
        Cursor.visible = true; //Make cursor visible but confined within the screen of the game
        Cursor.lockState = CursorLockMode.Confined;
    }
    void UnpauseState()
    {
        Time.timeScale = originalTimeScale; //Reset time
        Cursor.visible = false; //Relock mouse and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false; //Unpause
        activeMenu.SetActive(false); //Deactivate current menu
        activeMenu = null;//Unstore current menu
    }
    void YouLose()
    {
        PauseState(); //Pause
        activeMenu = lose; //Set current menu to the lose menu
        activeMenu.SetActive(true); //Show lose menu
    }
}
