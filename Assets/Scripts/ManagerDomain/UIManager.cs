using Cysharp.Threading.Tasks;
using Framework;
using Framework.Patterns;
using Module.CustomerControllerDomain;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ManagerDomain
{
    public class UIManager : LazyMonoSingleton<UIManager>, IInitializable
    {
        private Button cutBtn;
        private Button nextCustomerBtn; //Note: for testing
        private Button resetBtn; //Note: for testing

        private CustomerController customerController;


        public void Initialize()
        {
            cutBtn = GameObject.Find(Consts.SceneGameObjectName.CutBtn).GetComponent<Button>();
            Assert.IsNotNull(cutBtn);
            nextCustomerBtn = GameObject.Find(Consts.SceneGameObjectName.NextCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(nextCustomerBtn);
            resetBtn = GameObject.Find(Consts.SceneGameObjectName.RestCustomerBtn).GetComponent<Button>();
            Assert.IsNotNull(resetBtn);
            

            customerController = new CustomerController();
            customerController.InitializeAsync().Forget();
            RegisterEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            cutBtn.onClick.AddListener(CutHair);
            nextCustomerBtn.onClick.AddListener(NextCustomer);
            resetBtn.onClick.AddListener(ResetCustomer);
        }


        private void UnregisterEvents()
        {
            cutBtn.onClick.RemoveAllListeners();
            nextCustomerBtn.onClick.RemoveAllListeners();
        }


        private void CutHair()
        {
            Amo.Instance.Log($"Cut ! ", Color.green);
        }

        private void NextCustomer()
        {
            Amo.Instance.Log($"Next Customer! ", Color.green);
            customerController.NextCustomer();
        }


        private void ResetCustomer()
        {
            Amo.Instance.Log($"Reset customer", Color.red);
            customerController.Reset();
        }
    }
}