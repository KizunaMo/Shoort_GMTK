using System.Collections.Generic;
using Framework;
using Module.RecycleListViewDomain.Items;
using UnityEngine;

namespace Module.RecycleListViewDomain.Adapters
{
    public class CustomerListAdapter : IListViewAdapter
    {
        private List<CustomerValueObject> customerDatas;

        public CustomerListAdapter(List<CustomerValueObject> customerDatas)
        {
            this.customerDatas = customerDatas;
        }

        public OnItemClickedHandler OnItemClicked { get; set; }
        public int Count => customerDatas.Count;
        public float ItemHeight => 360f;

        public ListItem CreateItem()
        {
            var item =Object.Instantiate(Resources.Load<CustomerListItem>(Consts.PrefabsPath.CustomerItemPrefabPath));
            item.Initialize();
            return item;
        }

        public void SetItemContent(ListItem item)
        {
            if (item is CustomerListItem customer)
            {
                customer.UpdateContent(GetItemData(item.Position));
            }
        }

        public object GetItemData(int index)
        {
            return customerDatas[index];
        }

        public void UpdateData(List<CustomerValueObject> newData)
        {
            customerDatas = newData;
        }

        public class CustomerValueObject
        {
            public int Uid;
            public Color Color;//note: for testing
            public string Message;
        }
    }
}