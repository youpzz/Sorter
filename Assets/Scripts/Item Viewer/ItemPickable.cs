using UnityEngine;

public class ItemPickable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject itemPrefab;
    private bool isViewed = false;



    void ShowItem()
    {
        if (!isViewed && !ItemViewer.Instance.isViewing())
        {
            ItemViewer.Instance.ShowItem(this);
            isViewed = true;
        }
    }

    public void CanView(bool state) => isViewed = !state;
    public GameObject GetPrefab() => itemPrefab;



    // == Interactable ==

    public void Interact()
    {
        ShowItem();
    }
    public string GetInteractionText()
    {
        return "Нажмите [E]";
    }
    public string GetNameText()
    {
        return "Объект";
    }
}
