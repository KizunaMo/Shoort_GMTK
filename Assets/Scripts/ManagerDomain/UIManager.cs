using System;
using Cysharp.Threading.Tasks;
using Framework;
using Framework.Patterns;
using Module.CustomerControllerDomain;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ManagerDomain
{
    public class UIManager : LazyMonoSingleton<UIManager>, IInitializable
    {
        public event Action OnNextCustomer;
        public event Action OnCut;
        public event Action OnReset;

        private TMP_Text scoreText;
        private Button cutBtn;
        private Button nextCustomerBtn; //Note: for testing
        private Button resetBtn; //Note: for testing
        private Button startGameBtn;

        private GameObject mainMenu;
        private GameObject gameOverPanel;

        private AudioController audioController;

        private CircularTimerUI timerUI;

        public void Initialize()
        {
            cutBtn = GameObject.Find(Consts.SceneGameObjectName.CutBtn).GetComponent<Button>();
            Assert.IsNotNull(cutBtn);
            EnableCutBtnInteractable(false);

            nextCustomerBtn = GameObject.Find(Consts.SceneGameObjectName.NextCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(nextCustomerBtn);
            nextCustomerBtn.gameObject.SetActive(false);
            
            resetBtn = GameObject.Find(Consts.SceneGameObjectName.RestCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(resetBtn);
            resetBtn.gameObject.SetActive(false);
            
            scoreText = GameObject.Find(Consts.SceneGameObjectName.ScoreText).GetComponent<TMP_Text>();
            Assert.IsNotNull(scoreText);
            scoreText.text = "0";

            gameOverPanel = GameObject.Find(Consts.SceneGameObjectName.GameOverPanel);
            Assert.IsNotNull(gameOverPanel);
            gameOverPanel.SetActive(false);

            startGameBtn = GameObject.Find(Consts.SceneGameObjectName.StarGameBtn).GetComponent<Button>();
            Assert.IsNotNull(startGameBtn);

            mainMenu = GameObject.Find(Consts.SceneGameObjectName.MainMenuPanel);
            Assert.IsNotNull(mainMenu);

            audioController = GameObject.Find(Consts.SceneGameObjectName.AudioController).GetComponent<AudioController>();
            Assert.IsNotNull(audioController);

            timerUI = GameObject.Find(Consts.SceneGameObjectName.TimerUI).GetComponent<CircularTimerUI>();
            ShowTimerUI(false);
            timerUI.OnTimerEnd += () =>
            {
                timerUI.ResumeTimer();
                NextCustomer();
            };
            Assert.IsNotNull(timerUI);

            RegisterEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterEvents();
        }

        public void ShowTimerUI(bool isShow)
        {
            timerUI.gameObject.SetActive(isShow);
        }

        public void StartTimeer()
        {
            timerUI.StartTimer();
        }

        public void StopTimerUI()
        {
            timerUI.PauseTimer();
        }

        public void ResumeTimerUI()
        {
            timerUI.ResumeTimer();
        }

        public void SetScoreText(string value)
        {
            scoreText.text = value;
        }

        //�}��GAME OVER CANVAS
        public void ShowGameOverPanel(bool isShow)
        {
            gameOverPanel.SetActive(isShow);
            if (isShow)
            {
                //����game over BGM
                audioController.PlayGameOverBGM();
            }
        }

        public void EnableCutBtnInteractable(bool isEnabled)
        {
            cutBtn.interactable = isEnabled;
        }

        //�}���D���� canvas
        public void ShowMainuMenu(bool isShow)
        {
            mainMenu.SetActive(isShow);
            if (isShow)
            {
                //����}�Y�e�� BGM
                audioController.PlayMenuBGM();
            }
        }

        private void RegisterEvents()
        {
            cutBtn.onClick.AddListener(CutHair);
            nextCustomerBtn.onClick.AddListener(NextCustomer);
            resetBtn.onClick.AddListener(ResetCustomer);
            startGameBtn.onClick.AddListener(StartGame);
        }


        private void UnregisterEvents()
        {
            cutBtn?.onClick.RemoveAllListeners();
            nextCustomerBtn?.onClick.RemoveAllListeners();
            resetBtn?.onClick.RemoveAllListeners();
            startGameBtn?.onClick.RemoveAllListeners();
        }


        private void CutHair()
        {
            Amo.Instance.Log($"Cut ! ", Color.green);
            OnCut?.Invoke();
        }

        private void NextCustomer()
        {
            Amo.Instance.Log($"Next Customer! ", Color.green);
            OnNextCustomer?.Invoke();
        }


        private void ResetCustomer()
        {
            Amo.Instance.Log($"Reset customer", Color.red);
            OnReset?.Invoke();
        }

        private void StartGame()
        {
            Amo.Instance.Log($"Start Game! ", Color.green);
            GameManager.Instance.ResetStatus();
            EnableCutBtnInteractable(false);
            ShowMainuMenu(false);
            ShowGameOverPanel(false);
            //����D�C�� BGM
            audioController.PlayMainGameBGM();
            NextCustomer();
        }
    }
}