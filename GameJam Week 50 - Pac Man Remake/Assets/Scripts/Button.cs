using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using FMODUnity;

public class Button : MonoBehaviour {
    public bool opensDoor, addsLaser;
    public PostProcessingProfile camEffect;
    public WallColorChange outlineColorChange;
    public Transform door, buttonTop;
    public bool doesChangeCamEffects, doesChangeOutlineColor, doesChangeToOneColor, doesExplode;
    public Color outlineColor;
	public GameObject explosion;

    public GameObject[] thingsToDisable;
    public GameObject[] thingsToEnable;

	[FMODUnity.EventRef]
	public string pressButtonSound, explosionSound;


    PacMan_Controller pacMan;

    bool hasBeenPressed;

    Camera cam;
    PostProcessingBehaviour pPB;

    private void Start()
    {
        cam = Camera.main;
        pPB = cam.GetComponent<PostProcessingBehaviour>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            EnableAndDisableThings();
            pacMan = collision.gameObject.GetComponent<PacMan_Controller>();
            
            if (!hasBeenPressed) {
                FMODUnity.RuntimeManager.PlayOneShot(pressButtonSound, transform.position);
                if (opensDoor) {
                    if (door != null)
                    {
                        
                        Destroy(door.gameObject);
                    }

                }
                 if (addsLaser)
                {
                    pacMan.laserEnabled = true;
                    

                }


                LowerButton();


            }

        }
        
    }




    void LowerButton() {
		hasBeenPressed = true;
		if (buttonTop != null)
		{
			ColorChangeSpriteRend topButtonColorChange = buttonTop.GetComponent<ColorChangeSpriteRend>();

			if (topButtonColorChange != null)
			{
				topButtonColorChange.enabled = false;

			}
		}


        if (doesChangeCamEffects) {
            pPB.profile = camEffect;

        }

        if (doesChangeOutlineColor) {
            outlineColorChange.enabled = true;
        }

        if (doesChangeToOneColor) {
            outlineColorChange.enabled = true;
            outlineColorChange.SetToOneColor(outlineColor);

        }

		if (buttonTop != null)
		{
			Vector3 newTrans = new Vector3(0, -1.2f, 0);
			buttonTop.localPosition = newTrans;

		}

		if(doesExplode){
			GameObject buttonExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
			FMODUnity.RuntimeManager.PlayOneShot(explosionSound, transform.position);

			Destroy(gameObject);
			Destroy(buttonExplosion, 2f);
            


		}

    }



    void EnableAndDisableThings() {
        if (thingsToDisable != null) {
            foreach (GameObject g in thingsToDisable) {
                if (g != null)
                {
                    g.SetActive(false);
                }
            }
        }

        if (thingsToEnable != null) {
            foreach (GameObject g in thingsToEnable) {
                g.SetActive(true);
            }
        }



    }

}
