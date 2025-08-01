using System;
using Cysharp.Threading.Tasks;
using Framework;
using Module.CustomerControllerDomain;
using UnityEngine;

namespace ManagerDomain
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private int score;
        
        private CustomerController customerController;


        private void Awake()
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


        private void Start()
        {
            score = 0;
            InitializeAllModule();
            customerController = new CustomerController();
            customerController.InitializeAsync().Forget();
        }

        private void OnDestroy()
        {
            customerController.Dispose();
        }


        private void InitializeAllModule()
        {
            var uiManager = UIManager.Instance;




#if UNITY_EDITOR
            var amo = Amo.Instance;
#endif
        }

        public void AddScore()
        {
            score++;
            UIManager.Instance.SetScoreText(score.ToString());
        }

        public void GameOver()
        {
            UniTask.Create(async () =>
            {
                Amo.Instance.Log($"Game Over", Color.red);
                UIManager.Instance.ShowGameOverPanel(true);
                await UniTask.Delay(TimeSpan.FromSeconds(Consts.GameOverShowTime));
                UIManager.Instance.ShowMainuMenu(true);
            });
        }

        public void ResetStatus()
        {
            score = 0;
            UIManager.Instance.SetScoreText(score.ToString());
        }
    }
}