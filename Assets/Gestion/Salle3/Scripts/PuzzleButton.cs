using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private PuzzleTrigger puzzleTrigger;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Visuel")]
    [SerializeField] private Color inactiveColor = Color.red;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    [Header("Feedback")]
    [SerializeField] private GameObject activationParticles;
    [SerializeField] private AudioClip activationSound;

    private bool isActivated = false;
    private Transform player;
    private Renderer buttonRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        buttonRenderer = GetComponent<Renderer>();
      
        if (buttonRenderer != null)
        {
            if (inactiveMaterial != null)
                buttonRenderer.material = inactiveMaterial;
            else
                buttonRenderer.material.color = inactiveColor;
        }

        if (puzzleTrigger == null)
        {
            Debug.LogError($"❌ {gameObject.name}: PuzzleTrigger non assigné!");
        }
    }

    void Update()
    {
        if (isActivated || player == null || puzzleTrigger == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                ActivateButton();
            }
        }
    }

    private void ActivateButton()
    {
        if (isActivated || puzzleTrigger == null) return;

        isActivated = true;
        
        Debug.Log($"✅ Bouton {gameObject.name} du puzzle [{puzzleTrigger.GetPuzzleName()}] activé!");


        if (buttonRenderer != null)
        {
            if (activeMaterial != null)
                buttonRenderer.material = activeMaterial;
            else
                buttonRenderer.material.color = activeColor;
        }
        
        puzzleTrigger.OnTriggerActivated();
    }

    public void ResetButton()
    {
        isActivated = false;

        if (buttonRenderer != null)
        {
            if (inactiveMaterial != null)
                buttonRenderer.material = inactiveMaterial;
            else
                buttonRenderer.material.color = inactiveColor;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isActivated ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
