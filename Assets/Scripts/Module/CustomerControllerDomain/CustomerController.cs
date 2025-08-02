using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Framework;
using ManagerDomain;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Module.CustomerControllerDomain
{
    public class CustomerController : IDisposable
    {
        public List<Func<UniTask>> OnCheckCutResultHandlers;

        private int customerCount => customers.Count;
        private List<Customer> customers;
        private HairCutChecker hairCutChecker;

        public async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
            customers = new List<Customer>();
            hairCutChecker = new HairCutChecker();
            hairCutChecker.OnCutFailed += () =>
            {
                RemoveAllCustomer();
                GameManager.Instance.GameOver();
            };
            OnCheckCutResultHandlers = new List<Func<UniTask>>();
            RegisterEvents();
        }


        public void Dispose()
        {
            Amo.Instance.Log($"CustomerControllerDomain.Dispose()");
            UnregisterEvents();
        }

        public Customer CreateCustomer()
        {
            Amo.Instance.Log($"CreateCustomer()");
            var newCustomer = Object.Instantiate(Resources.Load<Customer>(Consts.PrefabsPath.CustomerItemPrefab));
            newCustomer.Initialize();
            return newCustomer;
        }


        private void MoveToNextCustomer()
        {
            Amo.Instance.Log($"MoveToNextCustomer()");
            UniTask.Create(async () =>
            {
                UIManager.Instance.EnableCutBtnInteractable(false);
                UIManager.Instance.ShowTimerUI(false);
                UIManager.Instance.StopTimerUI();
                if (customerCount > 0)
                {
                    var previousCustomer = customers[customerCount - 1];
                    var isSuccess = hairCutChecker.IsCutSuccess();
                    previousCustomer.UnregisterFlaverEvents();
                    if (OnCheckCutResultHandlers != null)
                    {
                        var allTask = OnCheckCutResultHandlers.Select(t => t.Invoke());
                        await UniTask.WhenAll(allTask);
                    }

                    Amo.Instance.Log($"CHECK success {isSuccess}");
                    if (isSuccess)
                    {
                        GameManager.Instance.AddScore();
                        previousCustomer.PlayAnimationAsync(Consts.AnimationName.Exit, Consts.AnimationName.EnterDuration).Forget();
                    }
                    else
                    {
                        await previousCustomer.PlayAnimationAsync(Consts.AnimationName.Angry, Consts.AnimationName.AngryDuration);
                        RemoveAllCustomer();
                        GameManager.Instance.GameOver();
                        return;
                    }
                }

                var newCustomer = CreateCustomer();
                customers.Add(newCustomer);
                newCustomer.gameObject.name += customerCount.ToString();

                hairCutChecker.UpdateNewCheckCount(newCustomer.HairCount);
                await newCustomer.PlayAnimationAsync(Consts.AnimationName.Enter, Consts.AnimationName.EnterDuration);
                UIManager.Instance.ShowTimerUI(true);
                UIManager.Instance.ResumeTimerUI();
                UIManager.Instance.StartTimeer();
                //在中間 剪頭髮的過程
                //await newCustomer.PlayAnimationAsync(Consts.AnimationName.DuringCuttingDuration, Consts.AnimationName.EnterDuration);
                UIManager.Instance.EnableCutBtnInteractable(true);
            });
        }

        public void RemoveCustomer(Customer customer)
        {
            Object.Destroy(customer.gameObject);
            if (customers.Contains(customer))
            {
                customers.Remove(customer);
            }
        }

        public void RemoveAllCustomer()
        {
            Amo.Instance.Log("RemoveAllCustomer()", Color.red);

            var customersToRemove = customers.ToList();
            customers.Clear();

            foreach (var customer in customersToRemove)
            {
                if (customer != null && customer.gameObject != null)
                {
                    Amo.Instance.Log($"RemoveCustomer {customer.name}", Color.red);
                    Object.Destroy(customer.gameObject);
                }
            }
        }


        private void RegisterEvents()
        {
            UIManager.Instance.OnNextCustomer += MoveToNextCustomer;
        }


        private void UnregisterEvents()
        {
            UIManager.Instance.OnNextCustomer -= MoveToNextCustomer;
        }
    }

    public class HairCutChecker : IDisposable
    {
        public  event Action OnCutFailed;
        public int totalHairCount;

        public HairCutChecker()
        {
            UIManager.Instance.OnCut += CutHair;
        }

        public void UpdateNewCheckCount(int newCheckCount)
        {
            totalHairCount = newCheckCount;
        }

        public void Dispose()
        {
            UIManager.Instance.OnCut -= CutHair;
        }

        private int successIndex = 4;

        private void CutHair()
        {
            totalHairCount--;
            if (totalHairCount < 0)
            {
                OnCutFailed?.Invoke();
            }

            if (totalHairCount == successIndex)
            {
                Amo.Instance.Log($"Cut hair success!! {totalHairCount}", Color.green);
            }
            else
            {
                Amo.Instance.Log($"Cut hair {totalHairCount}", Color.cyan);
            }
        }

        public bool IsCutSuccess()
        {
            var isSuccess = totalHairCount == successIndex;
            Amo.Instance.Log($" total hair count is {totalHairCount} ");
            return isSuccess;
        }
    }
}