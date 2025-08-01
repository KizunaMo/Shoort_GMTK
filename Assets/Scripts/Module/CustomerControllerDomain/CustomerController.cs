using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework;
using Module.RecycleListViewDomain;
using Module.RecycleListViewDomain.Adapters;
using Module.RecycleListViewDomain.Items;
using UnityEngine;
using UnityEngine.Assertions;
using static Module.RecycleListViewDomain.Adapters.CustomerListAdapter;

namespace Module.CustomerControllerDomain
{
    public class CustomerController
    {
        public List<Func<UniTask>> OnBeforeNextAsyncHandlers;
        public List<Func<UniTask>> OnAfterNextAsyncHandlers;
        
        public int CurrentCustomerId => currentCustomerIndex;
        
        private RecycledListViewModule cutomerListViewModule;
        private CustomerListAdapter listViewAdapter;
        private List<CustomerValueObject> customers;
        private int defaultStarIndex = 0;

        private int currentCustomerIndex;

        public async UniTask InitializeAsync()
        {
            cutomerListViewModule = GameObject.Find(Consts.SceneGameObjectName.CutomerList).GetComponentInChildren<RecycledListViewModule>();
            Assert.IsNotNull(cutomerListViewModule);
            
            OnBeforeNextAsyncHandlers = new List<Func<UniTask>>();
            
            await SetupDefaultCustomersAsync();
        }

        public async UniTask MoveToNextCustomerAsync()
        {
            var tempIndex = currentCustomerIndex;
            if (currentCustomerIndex == 0)
            {
                currentCustomerIndex += 2;
            }
            else
            {
                currentCustomerIndex += 1;
            }


            if (OnBeforeNextAsyncHandlers != null)
            {
                Amo.Instance.Todo($"play animation? or something?");
                var allTask = OnBeforeNextAsyncHandlers.Select(handler => handler.Invoke());
                await UniTask.WhenAll(allTask);
                Amo.Instance.Todo($"I am waitting for you~");
            }

            //Game JAM ~ 先硬來
            var current = cutomerListViewModule.GetVisibleItemAtIndex(tempIndex);
            if (current != null && current is CustomerListItem customer)
            {
                await customer.PlayAnimationAsync(Consts.AnimationName.Jump,1);
                Amo.Instance.Log($"Play animation: jump",Color.cyan);
            }
            
            cutomerListViewModule.ScrollToIndex(currentCustomerIndex);
            
            Amo.Instance.Log($"Next customer: {currentCustomerIndex}");

            if (OnAfterNextAsyncHandlers != null)
            {
                Amo.Instance.Log($"after next: {currentCustomerIndex}");
                await OnAfterNextAsyncHandlers.Select(handler => handler.Invoke());
                Amo.Instance.Log($" all task completed");
            }
        }

        public void Reset()
        {
            currentCustomerIndex = defaultStarIndex;
            cutomerListViewModule.ScrollToIndex(currentCustomerIndex);
        }


        private async UniTask SetupDefaultCustomersAsync()
        {
            await cutomerListViewModule.InitializeAsync();
            customers = new List<CustomerValueObject>();
            for (int i = 0; i < 1000; i++)
            {
                if (i <= 3)
                {
                    customers.Add(new CustomerValueObject()
                    {
                        Uid = i,
                        Message = $"Empty {i}",
                        Color = new Color(1f, 1f, 1f, 0f)
                    });
                }
                else
                {
                    customers.Add(new CustomerValueObject()
                    {
                        Uid = i,
                        Message = $"Customer index {i}",
                        Color = Color.white
                    });
                }
            }

            listViewAdapter = new CustomerListAdapter(customers);
            listViewAdapter.OnItemClicked += OnItemClicked;
            cutomerListViewModule.SetAdapter(listViewAdapter);

            await UniTask.DelayFrame(1);
            cutomerListViewModule.UpdateList();
            Reset();
        }

        private void OnItemClicked(ListItem item)
        {
            Amo.Instance.Log($"Clicked item at position {item.Position}: {customers[item.Position]}");
        }

        public void Refresh()
        {
            cutomerListViewModule.SetAdapter(listViewAdapter);
            cutomerListViewModule.UpdateList();
            Reset();
        }
    }
}