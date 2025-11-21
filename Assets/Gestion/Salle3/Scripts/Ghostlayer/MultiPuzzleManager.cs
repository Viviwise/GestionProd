using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiPuzzleManager : MonoBehaviour
{
    public enum PuzzleMode
    {
        PlacementOnly,
        ButtonsOnly,
        Both
    }

    [System.Serializable]
    public class PuzzleData
    {
        [Header("🎯 Configuration")]
        public string puzzleName = "Puzzle 1";
        public PuzzleMode mode = PuzzleMode.PlacementOnly;

        [Header("📍 Placement")]
        public PuzzlePlacementZone[] zones;

        [Header("🔘 Boutons")]
        public PuzzleButton[] buttons;

        [Header("🎁 Récompense")]
        public LayerChanger reward;

        [Header("🎉 Événement")]
        public UnityEvent onCompleted;

        [HideInInspector] public bool isCompleted = false;
        [HideInInspector] public bool placementDone = false;
        [HideInInspector] public bool buttonsDone = false;
    }

    public List<PuzzleData> puzzles = new List<PuzzleData>();
    public UnityEvent onAllCompleted;

   
    void Awake()
    {
       
        foreach (var puzzle in puzzles)
        {
            puzzle.isCompleted = false;
            puzzle.placementDone = false;
            puzzle.buttonsDone = false;
        }
    }

  
    void Start()
    {
        StartCoroutine(InitializeAfterFrame());
    }

   
    IEnumerator InitializeAfterFrame()
    {
        yield return null; 

        foreach (var puzzle in puzzles)
        {
            if (puzzle == null) continue;

           
            if (puzzle.mode == PuzzleMode.PlacementOnly || puzzle.mode == PuzzleMode.Both)
            {
                if (puzzle.zones != null)
                {
                    foreach (var zone in puzzle.zones)
                    {
                        if (zone != null)
                        {
                            var p = puzzle;
                            zone.onObjectPlaced.AddListener(() => CheckPlacement(p));
                        }
                    }
                }
            }

          
            if (puzzle.mode == PuzzleMode.ButtonsOnly || puzzle.mode == PuzzleMode.Both)
            {
                if (puzzle.buttons != null)
                {
                    foreach (var button in puzzle.buttons)
                    {
                        if (button != null)
                        {
                            var p = puzzle;
                            button.onActivated.AddListener(() => CheckButtons(p));
                        }
                    }
                }
            }

            Debug.Log($"✅ Puzzle '{puzzle.puzzleName}' initialisé");
        }
    }

    void CheckPlacement(PuzzleData puzzle)
    {
        if (puzzle.placementDone) return;

        bool allPlaced = true;
        foreach (var zone in puzzle.zones)
        {
            if (zone != null && !zone.IsOccupied())
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            puzzle.placementDone = true;
            Debug.Log($"📍 {puzzle.puzzleName} : Placement terminé");
            CheckPuzzleCompletion(puzzle);
        }
    }

    void CheckButtons(PuzzleData puzzle)
    {
        if (puzzle.buttonsDone) return;

        bool allActivated = true;
        foreach (var button in puzzle.buttons)
        {
            if (button != null && !button.IsActivated())
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated)
        {
            puzzle.buttonsDone = true;
            Debug.Log($"🔘 {puzzle.puzzleName} : Boutons activés");
            CheckPuzzleCompletion(puzzle);
        }
    }

    void CheckPuzzleCompletion(PuzzleData puzzle)
    {
        if (puzzle.isCompleted) return;

        bool completed = false;

        switch (puzzle.mode)
        {
            case PuzzleMode.PlacementOnly:
                completed = puzzle.placementDone;
                break;

            case PuzzleMode.ButtonsOnly:
                completed = puzzle.buttonsDone;
                break;

            case PuzzleMode.Both:
                completed = puzzle.placementDone && puzzle.buttonsDone;
                break;
        }

        if (completed)
        {
            puzzle.isCompleted = true;
            Debug.Log($"✅ {puzzle.puzzleName} RÉSOLU!");

            if (puzzle.reward != null)
                puzzle.reward.ChangeLayer();

            puzzle.onCompleted?.Invoke();
            CheckAllPuzzlesCompleted();
        }
    }

    void CheckAllPuzzlesCompleted()
    {
        foreach (var puzzle in puzzles)
        {
            if (!puzzle.isCompleted)
                return;
        }

        Debug.Log("🎉 TOUS LES PUZZLES RÉSOLUS!");
        onAllCompleted?.Invoke();
    }

    [ContextMenu("Reset Tout")]
    public void ResetAll()
    {
        foreach (var puzzle in puzzles)
        {
            puzzle.isCompleted = false;
            puzzle.placementDone = false;
            puzzle.buttonsDone = false;

            if (puzzle.zones != null)
            {
                foreach (var zone in puzzle.zones)
                {
                    if (zone != null) zone.ResetZone();
                }
            }

            if (puzzle.buttons != null)
            {
                foreach (var button in puzzle.buttons)
                {
                    if (button != null) button.ResetButton();
                }
            }
        }

        Debug.Log("🔄 Tous les puzzles réinitialisés");
    }
}
