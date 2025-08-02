using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Framework;
using ManagerDomain;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
                //RemoveAllCustomer();
                //FailedAsync().Forget();
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
                        previousCustomer.PlayAnimationAsync(Consts.AnimationName.Exit, Consts.AnimationName.EnterDuration, callBack: (trans) => { })
                            .Forget();
                    }
                    else
                    {
                        UIManager.Instance.AudioController.PlaySoundEffect_Fail();
                        await previousCustomer.PlayAnimationAsync(Consts.AnimationName.Angry, Consts.AnimationName.AngryDuration);

                        FailedAsync().Forget();
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

        private async UniTask FailedAsync()
        {
            RemoveAllCustomer();

            //todo: 波失敗扭來扭去

            await UniTask.Delay(TimeSpan.FromSeconds(Consts.AnimationName.FailedAnimationDuration));


            GameManager.Instance.GameOver();
        }

        public Vector3 CalculateGridPosition(int index, Vector3 startPosition = default)
        {
            if (startPosition == default)
                startPosition = new Vector3(-105, 100, 0);

            const int itemsPerRow = 10;

            int row = index / itemsPerRow; // 第幾排
            int col = index % itemsPerRow; // 該排第幾個

            Vector3 position = new Vector3(
                startPosition.x + col * Consts.CustomKeywords.Hidth, // X: 由左往右
                startPosition.y - row * Consts.CustomKeywords.Height, // Y: 往下排（3D空間）
                startPosition.z
            );

            return position;
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
            UniTask.Create(async () =>
            {
                var customersToRemove = customers.ToList();
                customers.Clear();
                UIManager.Instance.ShowFinalResultUI(true);
                var animationTasks = new List<UniTask>();

                for (int i = 0; i < customersToRemove.Count; i++)
                {
                    customersToRemove[i].EnableAnimator(false);
                    Amo.Instance.Log($"CustomerControllerDomain.RemoveAllCustomer() {customersToRemove[i].gameObject.name}");

                    if (i == customersToRemove.Count - 1)
                    {
                        customersToRemove[i].gameObject.transform.position = Vector3.one * 99999;
                        Amo.Instance.Log($"CustomerControllerDomain.RemoveAllCustomer() {customersToRemove[i].gameObject.name}", Color.red);
                        continue;
                    }

                    var pos = CalculateGridPosition(i);
                    customersToRemove[i].gameObject.transform.localScale = new Vector3(1, 1, 1);

                    var customerTask = AnimateCustomerToPositionWithBounce(customersToRemove[i], pos, i * 0.05f);
                    animationTasks.Add(customerTask);
                }

                var cameraTask = AnimateCameraSize(Consts.CamFarway, 1.0f);

                await UniTask.WhenAll(animationTasks.Concat(new[] { cameraTask }));

                await UniTask.Delay(TimeSpan.FromSeconds(Consts.FinalResultShowTime));

                foreach (var customer in customersToRemove)
                {
                    if (customer != null)
                        Object.Destroy(customer.gameObject);
                }
                AnimateCameraSize(Consts.CamNearby, 0.1f);
                UIManager.Instance.ShowFinalResultUI(false);
            });
        }

        private async UniTask AnimateCustomerToPositionWithBounce(Customer customer, Vector3 targetPos, float delay)
        {
            var transform = customer.gameObject.transform;

            await transform.DOLocalMove(targetPos, 0.8f)
                .SetDelay(delay)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion();

            await transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 3, 0.5f)
                .AsyncWaitForCompletion();

            StartIdleBouncing(transform);
        }

        private void StartIdleBouncing(Transform target)
        {
            if (target == null) return;

            var originalPos = target.localPosition;

            var sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Yoyo)
                .Append(target.DOLocalMoveY(originalPos.y + Random.Range(0.3f, 1f), Random.Range(0.1f, 0.5f)))
                .SetEase(Ease.InOutSine);

            sequence.OnStepComplete(() =>
            {
                if (target != null)
                {
                    float randomHeight = Random.Range(0.1f, 0.5f);
                    float randomDuration = Random.Range(0.1f, 0.3f);

                    target.DOLocalMoveY(originalPos.y + randomHeight, randomDuration)
                        .SetEase(Ease.InOutSine);
                }
            });
        }

        private async UniTask AnimateCameraSize(float targetSize, float duration)
        {
            if (Camera.main == null) return;

            await Camera.main.DOOrthoSize(targetSize, duration)
                .SetEase(Ease.OutQuart)
                .AsyncWaitForCompletion();
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
        public event Action OnCutFailed;
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