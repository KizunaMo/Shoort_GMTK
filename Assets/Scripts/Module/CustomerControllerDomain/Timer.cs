using System;

namespace Module.CustomerControllerDomain
{
    public class Timer
    {
        private float duration;
        private float currentTime;
        private bool isRunning;
        private bool isPaused;
    
        public event Action<float> OnTick; // 進度 0-1
        public event Action OnComplete;
    
        public float Progress => 1f - (currentTime / duration);
        public float RemainingTime => currentTime;
        public bool IsRunning => isRunning && !isPaused;
    
        public Timer(float duration)
        {
            this.duration = duration;
            this.currentTime = duration;
        }
    
        public void Start()
        {
            currentTime = duration;
            isRunning = true;
            isPaused = false;
        }
    
        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;
        public void Stop() => isRunning = false;
    
        public void Update(float deltaTime)
        {
            if (!isRunning || isPaused) return;
        
            currentTime -= deltaTime;
            OnTick?.Invoke(Progress);
        
            if (currentTime <= 0f)
            {
                isRunning = false;
                OnComplete?.Invoke();
            }
        }
    }
}