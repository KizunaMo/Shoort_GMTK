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


        private AudioSource BGMaudioSouece;
        
        

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

            BGMaudioSouece = GameObject.Find("BGM").GetComponent<AudioSource>();

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
            UIManager.Instance.Initialize();

#if UNITY_EDITOR
            Amo.Instance.Initialize();
#endif
        }

        public void AddScore()
        {
            score++;
            //�[�t
            Time.timeScale += 0.01f;

            //���ֳt�פ]�ܧ�
            BGMaudioSouece.pitch += 0.01f;

            UIManager.Instance.AudioController.PlaySoundEffect_Success();

            UIManager.Instance.SetScoreText(score.ToString());
        }

        //public void GameOver()
        //{

        //    //�C���^�_��t
        //    Time.timeScale = 1f;
        //    //���֦^�_��t
        //    BGMaudioSouece.pitch = 1f;
            
        //    // UniTask.Create(async () =>
        //    // {
        //    //     Amo.Instance.Log($"Game Over", Color.red);
        //    //     //UIManager.Instance.ShowGameOverPanel(true);
        //    //     //await UniTask.Delay(TimeSpan.FromSeconds(Consts.FinalResultShowTime));
        //    //     //UIManager.Instance.ShowMainuMenu(true);
        //    // });
        //}

        public void SetPitchAndTimeScale(float pitch,float timeScale)
        {
            Time.timeScale = timeScale;
            BGMaudioSouece.pitch = pitch;
        }

        public void ResetStatus()
        {
            score = 0;
            UIManager.Instance.SetScoreText(score.ToString());
        }
    }
}