using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ManagerDomain
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

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
            InitializeAllModule();
            UIManager.Instance.RegisterBeforeNextCustomerEvent(AddATask);
            UIManager.Instance.RegisterBeforeNextCustomerEvent(AnotherTask);
        }

        private async UniTask AnotherTask()
        {
            Amo.Instance.Log($"I want to do aaaaaaa",Color.red);
            await UniTask.Delay(1000);
            Amo.Instance.Log($"I am done , after 5 seconds",Color.red);
        }

        private async UniTask AddATask()
        {
            Amo.Instance.Log($"I want to do BBBBBBB");
            await UniTask.Delay(1000);
            Amo.Instance.Log($"I am done , and it cost 10 seconds");
        }

        private void InitializeAllModule()
        {
            var uiManager = UIManager.Instance;
            
            
            
            
#if UNITY_EDITOR
            var amo = Amo.Instance;
#endif
        }
    }
}