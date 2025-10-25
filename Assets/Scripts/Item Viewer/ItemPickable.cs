using System.Collections;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemScriptable scriptable;
    private GameObject itemPrefab;
    private bool isViewed = false;


    bool isInteractable = false;

    void Start()
    {
        Init();
    }

    void Init()
    {
        itemPrefab = scriptable.inspectionPrefab;
        if (GetComponent<Outline>()) GetComponent<Outline>().enabled = false;
    }

    void ShowItem()
    {
        if (!isViewed && !ItemViewer.Instance.isViewing() && gameObject.activeSelf)
        {
            Debug.Log("Осмотр предмета");
            ItemViewer.Instance.ShowItem(this);
            isViewed = true;
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        StartCoroutine(InteractionCooldown());
    }

    void OnDisable()
    {
        isInteractable = false;
    }

    public void CanView(bool state) => isViewed = !state;
    public GameObject GetPrefab() => itemPrefab;


    IEnumerator InteractionCooldown()
    {
        float timer = 0.5f;

        while (timer > 0) { timer -= Time.deltaTime; yield return null; }

        isInteractable = true;
    }
    



    // == Interactable ==

    public void Interact()
    {
        if (isInteractable)
        {
            ShowItem();
        }
    }
    public string GetInteractionText()
    {
        return "Осмотреть [E]";
    }
    public string GetNameText()
    {
        return scriptable.itemName;
    }
}
