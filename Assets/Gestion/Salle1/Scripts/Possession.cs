using System;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Possession : MonoBehaviour
{
    
    public GameObject ghost;
   // public Camera camGhost;
  //  public PlayerMovement playerMovement;
    
    public GameObject hamster;
   // public Camera camHamster;
 //   public HamsterController hamsterController;
    
    private KeyCode toggleKey = KeyCode.H;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Switch();
        }
    }

    void Switch()
    {
        if (ghost == null || hamster == null)
        {
            return;
        }
        bool aActive = ghost.activeSelf;
        
        ghost.SetActive(!aActive);
       // playerMovement.enabled = !aActive;
       // camGhost.enabled = !aActive;

        hamster.SetActive(aActive);
       // hamsterController.enabled = aActive;
       // camHamster.enabled = aActive;
    }
}

