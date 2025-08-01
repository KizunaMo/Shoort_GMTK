using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework;
using Module.RecycleListViewDomain;
using Module.RecycleListViewDomain.Adapters;
using UnityEngine;
using UnityEngine.Assertions;
using static Module.RecycleListViewDomain.Adapters.CustomerListAdapter;

namespace Module.CustomerControllerDomain
{
    public class CustomerController
    {
        public int CurrentCustomerId => currentCustomerIndex;

        private RecycledListViewModule cutomeristViewModule;
        private CustomerListAdapter listViewAdapter;
        private List<CustomerValueObject> customers;
        private int defaultStarIndex = 0;

        private int currentCustomerIndex;

        public async UniTask InitializeAsync()
        {
            cutomeristViewModule = GameObject.Find(Consts.SceneGameObjectName.CutomerList).GetComponentInChildren<RecycledListViewModule>();
            Assert.IsNotNull(cutomeristViewModule);
            await SetupDefaultCustomersAsync();
        }

        public void NextCustomer()
        {
            if (currentCustomerIndex == 0)
            {
                currentCustomerIndex += 2;
            }
            else
            {
                currentCustomerIndex += 1;
            }
            cutomeristViewModule.ScrollToIndex(currentCustomerIndex);
            Amo.Instance.Log($"Next customer: {currentCustomerIndex}");
        }

        public void Reset()
        {
            currentCustomerIndex = defaultStarIndex;
            cutomeristViewModule.ScrollToIndex(currentCustomerIndex);
        }


        private async UniTask SetupDefaultCustomersAsync()
        {
            await cutomeristViewModule.InitializeAsync();
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
                        Color = Random.ColorHSV(),
                    });
                }
            }

            listViewAdapter = new CustomerListAdapter(customers);
            listViewAdapter.OnItemClicked += OnItemClicked;
            cutomeristViewModule.SetAdapter(listViewAdapter);

            await UniTask.DelayFrame(1);
            cutomeristViewModule.UpdateList();
            Reset();
        }

        private void OnItemClicked(ListItem item)
        {
            Amo.Instance.Log($"Clicked item at position {item.Position}: {customers[item.Position]}");
        }
    }
}