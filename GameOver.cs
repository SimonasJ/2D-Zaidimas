using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public void LoadScene(string name)
    {
        Application.LoadLevel(1);
    }
    public void Exit()
    {
        Application.LoadLevel(0);
    }
}
