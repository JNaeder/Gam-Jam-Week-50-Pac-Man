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
    public bool doesChangeCamEffects, doesChangeOutlineColor, doesChangeToOneColor;
    public Color outlineColor;

    public GameObject[] thingsToDisable;
    public GameObject[] thingsToEnable;

    [FMODUnity.EventRef]
    public string pressButtonSound;


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


        Vector3 newTrans = new Vector3(0, -1.2f, 0);
        buttonTop.localPosition = newTrans;
        hasBeenPressed = true;

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
