using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Brick : MonoBehaviour {
    public float health;

    public Sprite[] spriteStates;
    public GameObject explosion;
    [FMODUnity.EventRef]
    public string explosionSound;
    public bool isDestructable;


    float healthPerc, startHealth;
    SpriteRenderer sP;

	// Use this for initialization
	void Start () {
        sP = GetComponent<SpriteRenderer>();
        startHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSrpites();
        UpdateHealth();
	}



    public void DoDamage(float damage)
    {
        if (isDestructable)
        {
            health -= Time.deltaTime * damage;
        }

    }

    void UpdateHealth() {
        healthPerc = health / startHealth;
        if (health <= 0) {
            GameObject explosionInst = Instantiate(explosion, transform.position, Quaternion.identity);
            FMODUnity.RuntimeManager.PlayOneShot(explosionSound, transform.position);
            Destroy(explosionInst, 2);
            Destroy(gameObject);
        }

    }


    void UpdateSrpites() {
        if (healthPerc > 0.75f)
        {
            sP.sprite = spriteStates[0];
        }
        else if (healthPerc > 0.25f && healthPerc < 0.75f)
        {
            sP.sprite = spriteStates[1];
        }
        else if(healthPerc < 0.25f)
        {

            sP.sprite = spriteStates[2];

        }


    }
}
