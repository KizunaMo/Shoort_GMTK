using System;
using UnityEngine;
using UnityEngine.UI;

namespace Module.CustomerControllerDomain
{
    public class CircularTimerUI : MonoBehaviour
    {
        public event Action OnTimerEnd;
        
        [SerializeField] private Image fillImage;
        [SerializeField] private float duration = 5f;
    
        private Timer timer;
    
        private void Awake()
        {
            timer = new Timer(duration);
            timer.OnTick += UpdateFill;
            timer.OnComplete += OnTimerComplete;
        
            if (fillImage == null)
                fillImage = GetComponent<Image>();
        }
    
        public void StartTimer() => timer.Start();
        public void PauseTimer() => timer.Pause();
        public void ResumeTimer() => timer.Resume();
    
        private void Update() => timer.Update(Time.deltaTime);
    
        private void UpdateFill(float progress)
        {
            fillImage.fillAmount = 1f - progress;
        }
    
        private void OnTimerComplete()
        {
            Amo.Instance.Log("Timer completed!");
            OnTimerEnd?.Invoke();
        }
    }
}