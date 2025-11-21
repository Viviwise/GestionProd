using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gestion.Salle3.Scripts
{
    public class TimeUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private Slider timeSlider;
        [SerializeField] private Image rewindIndicator;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color rewindColor = Color.red;

        void Start()
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnTimeChanged += UpdateUI;
                TimeManager.Instance.OnRewindStateChanged += UpdateRewindIndicator;
            }

            if (timeSlider != null)
            {
                timeSlider.minValue = 0f;
                timeSlider.maxValue = 24f;
                timeSlider.onValueChanged.AddListener(OnSliderChanged);
            }
        }

        void OnDestroy()
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.OnTimeChanged -= UpdateUI;
                TimeManager.Instance.OnRewindStateChanged -= UpdateRewindIndicator;
            }
        }

        private void UpdateUI(float currentHour)
        {
            if (timeText != null)
            {
                timeText.text = TimeManager.Instance.GetFormattedTime();
            }

            if (dayText != null)
            {
                dayText.text = $"Jour {TimeManager.Instance.CurrentDay + 1}";
            }

            if (timeSlider != null && !timeSlider.IsInteractable())
            {
                timeSlider.value = currentHour;
            }
        }

        private void UpdateRewindIndicator(bool isRewinding)
        {
            if (rewindIndicator != null)
            {
                rewindIndicator.color = isRewinding ? rewindColor : normalColor;
            }

            if (timeText != null)
            {
                timeText.color = isRewinding ? rewindColor : normalColor;
            }
        }

        private void OnSliderChanged(float value)
        {
            if (TimeManager.Instance != null && !TimeManager.IsRewinding)
            {
                TimeManager.Instance.SetTime(value);
            }
        }
    }
}
