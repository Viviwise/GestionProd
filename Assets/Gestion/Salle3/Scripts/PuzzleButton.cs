using UnityEngine;
using UnityEngine.Events;

public class PuzzleButton : MonoBehaviour
{
    public UnityEvent onActivated;

    private bool isActivated = false;

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log($"🔘 {gameObject.name} : Bouton activé");
        onActivated?.Invoke();

        // Effet visuel
        GetComponent<Renderer>().material.color = Color.green;
    }

    public bool IsActivated() => isActivated;

    public void ResetButton()
    {
        isActivated = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    void OnMouseDown()
    {
        Activate();
    }
}