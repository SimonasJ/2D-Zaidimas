﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Character : MonoBehaviour 
{
    public Animator myAnimator { get; private set; }
	[SerializeField]
	protected Transform knifePos;
	[SerializeField]                   
	protected float movementSpeed; 
	protected bool facingRight;
	[SerializeField]
	private GameObject knifePrefab;
	public bool Attack { get; set;}
	[SerializeField]
	protected int health;
	[SerializeField]
	private EdgeCollider2D MeleeCollider;
	[SerializeField]
	public bool boss = false;
	public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }

 //-------------------------------------------------------------------------------------------------
	public virtual void Start () 
	{
		facingRight = true;
		myAnimator = GetComponent<Animator>();
	}
//-------------------------------------------------------------------------------------------------	
	void Update () 
	{
		
	}
//-------------------------------------------------------------------------------------------------
     public abstract IEnumerator TakeDamage();
//-------------------------------------------------------------------------------------------------
	public void ChangeDirection()
	{
		facingRight = !facingRight;
		if(boss)
			transform.localScale = new Vector3 (transform.localScale.x * -1, 2, 1);
		else
			transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);

	}
//-------------------------------------------------------------------------------------------------
    public void ThrowKnife(int value)
	{
			if (facingRight) {
				GameObject tmp = (GameObject)Instantiate (knifePrefab, knifePos.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
				tmp.GetComponent<Knife> ().Initialize (Vector2.right);
			} else {
				GameObject tmp = (GameObject)Instantiate (knifePrefab, knifePos.position, Quaternion.Euler (new Vector3 (0, 0, 180)));
				tmp.GetComponent<Knife> ().Initialize (Vector2.left);

			}
	}
//-------------------------------------------------------------------------------------------------
	public void MeleeAttack()
	{
		MeleeCollider.enabled = !MeleeCollider.enabled;
	}
//-------------------------------------------------------------------------------------------------
	public void DieCollider()
	{
		MeleeCollider.enabled = false;
	}
//-------------------------------------------------------------------------------------------------
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerKnife")
        {
			Destroy(other.gameObject);
			StartCoroutine(TakeDamage());
        }
    }

}
