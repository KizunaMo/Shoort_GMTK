using Module.RecycleListViewDomain;

public delegate void OnItemClickedHandler(ListItem item);

public interface IListViewAdapter
{
    OnItemClickedHandler OnItemClicked { get; set; }
    int Count { get; }
    float ItemCustomerHeight { get; }
    ListItem CreateItem();
    void SetItemContent(ListItem item);
    object GetItemData(int index);
}