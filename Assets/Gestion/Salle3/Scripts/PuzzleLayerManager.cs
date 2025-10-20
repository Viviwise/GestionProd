using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PuzzleLayerReward
{
    public string puzzleName;
    public LayerChanger layerChanger;
    public bool isCompleted = false;
}

public class PuzzleLayerManager : MonoBehaviour
{
    [Header("Configuration des Puzzles")]
    [SerializeField] private List<PuzzleLayerReward> puzzleRewards = new List<PuzzleLayerReward>();
    
    [Header("Progression")]
    [SerializeField] private int completedPuzzles = 0;
    [SerializeField] private bool allPuzzlesCompleted = false;
    
    [Header("Options")]
    [SerializeField] private bool activateLayersInOrder = false; // Si true, les puzzles doivent être complétés dans l'ordre
    [SerializeField] private bool showDebugLogs = true;
    
    [Header("Events")]
    public UnityEvent<int> onPuzzleCompleted; // Nombre de puzzles complétés
    public UnityEvent onAllPuzzlesCompleted;

    void Start()
    {
        ValidateSetup();
    }

    private void ValidateSetup()
    {
        if (puzzleRewards.Count == 0)
        {
            Debug.LogWarning("⚠️ Aucun puzzle configuré dans le PuzzleLayerManager!");
            return;
        }

        foreach (var reward in puzzleRewards)
        {
            if (reward.layerChanger == null)
            {
                Debug.LogError($"❌ LayerChanger manquant pour le puzzle: {reward.puzzleName}");
            }
        }

        if (showDebugLogs)
        {
            Debug.Log($"🎮 PuzzleLayerManager initialisé avec {puzzleRewards.Count} puzzles");
        }
    }

    // Appelé par les PuzzleTrigger individuels
    public void OnPuzzleCompleted(string puzzleName)
    {
        PuzzleLayerReward puzzle = puzzleRewards.Find(p => p.puzzleName == puzzleName);

        if (puzzle == null)
        {
            Debug.LogError($"❌ Puzzle '{puzzleName}' non trouvé dans la liste!");
            return;
        }

        if (puzzle.isCompleted)
        {
            if (showDebugLogs)
                Debug.Log($"ℹ️ Puzzle '{puzzleName}' déjà complété");
            return;
        }

        // Vérifier si on doit respecter l'ordre
        if (activateLayersInOrder)
        {
            int puzzleIndex = puzzleRewards.IndexOf(puzzle);
            if (puzzleIndex != completedPuzzles)
            {
                Debug.LogWarning($"⚠️ Vous devez compléter les puzzles dans l'ordre! Complétez d'abord: {puzzleRewards[completedPuzzles].puzzleName}");
                return;
            }
        }

        // Marquer comme complété
        puzzle.isCompleted = true;
        completedPuzzles++;

        if (showDebugLogs)
            Debug.Log($"🎉 Puzzle '{puzzleName}' complété! ({completedPuzzles}/{puzzleRewards.Count})");

        // Activer le LayerChanger associé
        if (puzzle.layerChanger != null)
        {
            puzzle.layerChanger.ChangeLayer();
        }

        // Invoquer l'événement
        onPuzzleCompleted?.Invoke(completedPuzzles);

        // Vérifier si tous les puzzles sont complétés
        CheckAllPuzzlesCompleted();
    }

    // Surcharge pour compléter par index
    public void OnPuzzleCompleted(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= puzzleRewards.Count)
        {
            Debug.LogError($"❌ Index de puzzle invalide: {puzzleIndex}");
            return;
        }

        OnPuzzleCompleted(puzzleRewards[puzzleIndex].puzzleName);
    }

    private void CheckAllPuzzlesCompleted()
    {
        if (completedPuzzles >= puzzleRewards.Count && !allPuzzlesCompleted)
        {
            allPuzzlesCompleted = true;
            
            if (showDebugLogs)
                Debug.Log("🏆 TOUS LES PUZZLES COMPLÉTÉS!");

            onAllPuzzlesCompleted?.Invoke();
        }
    }

    // Méthodes utilitaires
    public void ResetAllPuzzles()
    {
        foreach (var puzzle in puzzleRewards)
        {
            puzzle.isCompleted = false;
        }

        completedPuzzles = 0;
        allPuzzlesCompleted = false;

        if (showDebugLogs)
            Debug.Log("🔄 Tous les puzzles réinitialisés");
    }

    public void ResetPuzzle(string puzzleName)
    {
        PuzzleLayerReward puzzle = puzzleRewards.Find(p => p.puzzleName == puzzleName);
        
        if (puzzle != null && puzzle.isCompleted)
        {
            puzzle.isCompleted = false;
            completedPuzzles--;
            allPuzzlesCompleted = false;

            if (showDebugLogs)
                Debug.Log($"🔄 Puzzle '{puzzleName}' réinitialisé");
        }
    }

    public int GetCompletedPuzzleCount()
    {
        return completedPuzzles;
    }

    public int GetTotalPuzzleCount()
    {
        return puzzleRewards.Count;
    }

    public float GetProgressPercentage()
    {
        if (puzzleRewards.Count == 0) return 0f;
        return (float)completedPuzzles / puzzleRewards.Count * 100f;
    }

    public bool IsPuzzleCompleted(string puzzleName)
    {
        PuzzleLayerReward puzzle = puzzleRewards.Find(p => p.puzzleName == puzzleName);
        return puzzle != null && puzzle.isCompleted;
    }

    public List<string> GetCompletedPuzzleNames()
    {
        List<string> completed = new List<string>();
        foreach (var puzzle in puzzleRewards)
        {
            if (puzzle.isCompleted)
            {
                completed.Add(puzzle.puzzleName);
            }
        }
        return completed;
    }
}
