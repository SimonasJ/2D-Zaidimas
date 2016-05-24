using UnityEngine;
using System.Collections;

public class Winner : MonoBehaviour
{
    public void LoadScene(string name)
    {
        Application.LoadLevel("1");
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}