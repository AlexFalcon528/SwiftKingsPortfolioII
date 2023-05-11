using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnFunctions : MonoBehaviour
{
    public void Resume() {
        gameManager.instance.UnpauseState();
        gameManager.instance.isPaused = false;
    }

    public void Restart() {
        //
    }

    public void Exit() {
        Application.Quit();
    }

    public void Respawn() {
        //
    }
}
