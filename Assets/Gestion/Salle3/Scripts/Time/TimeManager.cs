using System;
using UnityEngine;

namespace Gestion.Salle3.Scripts
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [Header("Time Settings")]
        [SerializeField] private float dayDurationInSeconds = 120f; // Durée d'une journée complète
        [SerializeField] private float startHour = 8f; // Heure de départ (8h du matin)
        
        [Header("Rewind Settings")]
        [SerializeField] private KeyCode rewindKey = KeyCode.R;
        [SerializeField] private KeyCode forwardKey = KeyCode.F;
        [SerializeField] private float maxRewindDuration = 10f;
        [SerializeField] private float rewindSpeed = 2f; // Vitesse du retour en arrière

        // Propriétés publiques
        public static bool IsRewinding { get; private set; }
        public float CurrentHour { get; private set; }
        public float CurrentMinute => (CurrentHour % 1) * 60f;
        public int CurrentDay { get; private set; }

        // Events
        public event Action<float> OnTimeChanged;
        public event Action<bool> OnRewindStateChanged;

        private float currentRewindTime = 0f;
        private float timeSpeed = 1f;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            CurrentHour = startHour;
            CurrentDay = 0;
        }

        void Update()
        {
            HandleInput();
            UpdateTime();
        }

        private void HandleInput()
        {
            // Rewind
            if (Input.GetKeyDown(rewindKey))
            {
                StartRewind();
            }
            else if (Input.GetKeyUp(rewindKey))
            {
                StopRewind();
            }

            // Fast forward
            if (Input.GetKey(forwardKey))
            {
                timeSpeed = 3f;
            }
            else if (!IsRewinding)
            {
                timeSpeed = 1f;
            }
        }

        private void UpdateTime()
        {
            if (IsRewinding)
            {
                currentRewindTime += Time.deltaTime;
                
                // Reculer le temps
                CurrentHour -= (Time.deltaTime * rewindSpeed * (24f / dayDurationInSeconds));
                
                if (currentRewindTime >= maxRewindDuration || CurrentHour < 0)
                {
                    StopRewind();
                }
            }
            else
            {
                // Avancer le temps normalement
                CurrentHour += (Time.deltaTime * timeSpeed * (24f / dayDurationInSeconds));
            }

            // Gérer le passage au jour suivant/précédent
            if (CurrentHour >= 24f)
            {
                CurrentHour -= 24f;
                CurrentDay++;
            }
            else if (CurrentHour < 0f)
            {
                CurrentHour += 24f;
                CurrentDay--;
            }

            OnTimeChanged?.Invoke(CurrentHour);
        }

        private void StartRewind()
        {
            IsRewinding = true;
            currentRewindTime = 0f;
            timeSpeed = -rewindSpeed;
            OnRewindStateChanged?.Invoke(true);
            Debug.Log("Rewind started");
        }

        private void StopRewind()
        {
            IsRewinding = false;
            currentRewindTime = 0f;
            timeSpeed = 1f;
            OnRewindStateChanged?.Invoke(false);
            Debug.Log("Rewind stopped");
        }

        public float GetRewindProgress()
        {
            return currentRewindTime / maxRewindDuration;
        }

        public string GetFormattedTime()
        {
            int hours = Mathf.FloorToInt(CurrentHour);
            int minutes = Mathf.FloorToInt(CurrentMinute);
            return $"{hours:D2}:{minutes:D2}";
        }

        public void SetTime(float hour)
        {
            CurrentHour = Mathf.Clamp(hour, 0f, 24f);
            OnTimeChanged?.Invoke(CurrentHour);
        }
    }
}
