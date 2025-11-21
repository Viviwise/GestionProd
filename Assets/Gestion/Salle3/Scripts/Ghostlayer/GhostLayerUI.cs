using UnityEngine;
using TMPro;

public class GhostLayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PressButtonGhostLayer;
    [SerializeField] private LayerMask crossLayerMask; // Assignez "Cross" dans l'Inspecteur

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entré avec : {other.name} | Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        if (((1 << other.gameObject.layer) & crossLayerMask) != 0)
        {
            Debug.Log("Layer correspondant détecté ! Affichage du texte.");
            PressButtonGhostLayer.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & crossLayerMask) != 0)
        {
            Debug.Log("Layer correspondant quitté ! Masquage du texte.");
            PressButtonGhostLayer.gameObject.SetActive(false);
        }
    }
}