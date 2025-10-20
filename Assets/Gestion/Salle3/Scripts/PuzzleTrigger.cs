using UnityEngine;
using UnityEngine.Events;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Identification")]
    [SerializeField] private string puzzleName = "Puzzle 1";
    
    [Header("Configuration")]
    [Tooltip("Nombre de triggers requis pour activer le LayerChanger")]
    [SerializeField] private int triggersRequired = 3;

    [Header("Manager")]
    [SerializeField] private PuzzleLayerManager puzzleManager;

    [Header("État")]
    [SerializeField] private int triggersActivated = 0;
    [SerializeField] private bool puzzleCompleted = false;

    [Header("Options")]
    [SerializeField] private bool showDebugLogs = true;

    [Header("Events")]
    public UnityEvent onTriggerActivated;
    public UnityEvent onPuzzleProgress;
    public UnityEvent onPuzzleCompleted;

    void Start()
    {
        ValidatePuzzle();
    }

    private void ValidatePuzzle()
    {
        if (puzzleManager == null)
        {
            puzzleManager = FindObjectOfType<PuzzleLayerManager>();
            
            if (puzzleManager == null)
            {
                Debug.LogError($"❌ [{puzzleName}] Aucun PuzzleLayerManager trouvé dans la scène!");
            }
        }

        if (string.IsNullOrEmpty(puzzleName))
        {
            puzzleName = gameObject.name;
            Debug.LogWarning($"⚠️ Puzzle sans nom, utilisation du nom du GameObject: {puzzleName}");
        }

        if (triggersRequired <= 0)
        {
            Debug.LogWarning($"⚠️ [{puzzleName}] Le nombre de triggers requis doit être supérieur à 0");
            triggersRequired = 1;
        }

        if (showDebugLogs)
        {
            Debug.Log($"🎮 [{puzzleName}] Puzzle initialisé: 0/{triggersRequired} triggers activés");
        }
    }

    public void OnTriggerActivated()
    {
        if (puzzleCompleted)
        {
            if (showDebugLogs)
                Debug.Log($"ℹ️ [{puzzleName}] Puzzle déjà complété");
            return;
        }

        triggersActivated++;

        if (showDebugLogs)
            Debug.Log($"🔔 [{puzzleName}] Trigger activé: {triggersActivated}/{triggersRequired}");

        onTriggerActivated?.Invoke();

        if (triggersActivated < triggersRequired)
        {
            onPuzzleProgress?.Invoke();
        }

        if (triggersActivated >= triggersRequired)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        if (puzzleCompleted) return;

        puzzleCompleted = true;

        if (showDebugLogs)
            Debug.Log($"🎉 [{puzzleName}] Puzzle complété!");

        // Notifier le manager
        if (puzzleManager != null)
        {
            puzzleManager.OnPuzzleCompleted(puzzleName);
        }
        else
        {
            Debug.LogError($"❌ [{puzzleName}] Impossible de notifier le manager: référence null!");
        }

        onPuzzleCompleted?.Invoke();
    }

    public void ResetPuzzle()
    {
        triggersActivated = 0;
        puzzleCompleted = false;

        if (showDebugLogs)
            Debug.Log($"🔄 [{puzzleName}] Puzzle réinitialisé");
    }

    public void ForceCompletePuzzle()
    {
        triggersActivated = triggersRequired;
        CompletePuzzle();
    }

    // Getters
    public string GetPuzzleName() => puzzleName;
    public int GetProgress() => triggersActivated;
    public float GetProgressPercentage() => (float)triggersActivated / triggersRequired * 100f;
    public bool IsCompleted() => puzzleCompleted;
}
