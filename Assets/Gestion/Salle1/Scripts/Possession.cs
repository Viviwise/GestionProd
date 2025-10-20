using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Possession : MonoBehaviour
{
    
    public GameObject ghost;
    public Camera ghostCamera;
    public PlayerMovement playerMovement;
    
    public GameObject hamster;
    public Camera hamsterCamera;
    public HamsterController hamsterController;
    
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
        ghostCamera.enabled = !aActive;
        playerMovement.enabled = !aActive;
        
        hamster.SetActive(aActive);
        hamsterCamera.enabled = aActive;
        hamsterController.enabled = aActive;
    }
}

