using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster2 : MonoBehaviour {

    public int points;
	[SerializeField]
	public int health;

    public Text pointsText;
	public Text healthText;

    void Update()
    {
		pointsText.text = ("Points: " + points);
		healthText.text = ("Health: " + health);
    }
}
