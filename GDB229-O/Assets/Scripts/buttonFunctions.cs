using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
   public void resume()
    {
        gameManager.instance.unPauseState();
    }
    public void restart()
    {
        gameManager.instance.unPauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void quit()
    {
        Application.Quit();
    }
    public void spawnPlayer()
    {
        gameManager.instance.unPauseState();
        gameManager.instance.playerScript.spawnPlayer();
    }
    public void playerHeal(int amount)
    {
        gameManager.instance.playerScript.playerHeal(amount);
    }
}
