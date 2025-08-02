using System;
using Framework;
using Framework.Patterns;
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

        public void Initialize()
        {
            cutBtn = GameObject.Find(Consts.SceneGameObjectName.CutBtn).GetComponent<Button>();
            Assert.IsNotNull(cutBtn);
            EnableCutBtnInteractable(false);
            
            nextCustomerBtn = GameObject.Find(Consts.SceneGameObjectName.NextCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(nextCustomerBtn);
            resetBtn = GameObject.Find(Consts.SceneGameObjectName.RestCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(resetBtn);

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

            RegisterEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterEvents();
        }

        public void SetScoreText(string value)
        {
            scoreText.text = value;
        }

        //開關GAME OVER CANVAS
        public void ShowGameOverPanel(bool isShow)
        {
            gameOverPanel.SetActive(isShow);
            if (isShow)
            {
                //播放game over BGM
                audioController.PlayGameOverBGM();
            }
        }

        public void EnableCutBtnInteractable(bool isEnabled)
        {
            cutBtn.interactable = isEnabled;
        }

        //開關主頁面 canvas
        public void ShowMainuMenu(bool isShow)
        {
            mainMenu.SetActive(isShow);
            if (isShow)
            {
                //播放開頭畫面 BGM
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
            
            {
                //播放主遊戲 BGM
                audioController.PlayMainGameBGM();
            }
        }
    }
}