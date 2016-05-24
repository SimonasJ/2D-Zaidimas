using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour 
{
	[SerializeField]
	private float speed;
	private Rigidbody2D myRigidbody;
	private Vector2 direction;
//-------------------------------------------------------------------------------------------------
	void Start () 
    {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
//-------------------------------------------------------------------------------------------------
	void FixedUpdate()
	{
		myRigidbody.velocity = direction * speed;
	}
//-------------------------------------------------------------------------------------------------
	public void Initialize(Vector2 direction)
	{
        this.direction = direction;
	}
//-------------------------------------------------------------------------------------------------
	void OnBecameInvisible()
	{
		if(gameObject.CompareTag("PlayerKnife") || gameObject.CompareTag("EnemyKnife") || gameObject.CompareTag("BossEnemyKnife"))
		Destroy (gameObject);
	}
	
//-------------------------------------------------------------------------------------------------
}
