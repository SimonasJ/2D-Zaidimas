using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public Transform mainMenu, aboutMenu, playerControlMenu;

    public void LoadScene(string name)
    {
        Application.LoadLevel(1);
    }
    public void QuitGame(){
		Application.Quit ();
    }

	public void AboutMenu(bool clicked)
    {
        if (clicked == true)
        {
			aboutMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(false);
        }
        else
        {
			aboutMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(true);
        }
    }
	public void PlayerControlMenu(bool clicked)
	{
		if (clicked == true)
		{
			playerControlMenu.gameObject.SetActive(clicked);
			mainMenu.gameObject.SetActive(false);
		}
		else
		{
			playerControlMenu.gameObject.SetActive(clicked);
			mainMenu.gameObject.SetActive(true);
		}
	}
}
