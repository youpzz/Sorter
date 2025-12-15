using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    [SerializeField] private ItemScriptable scriptable;

    [SerializeField] private List<Damage> damageTypes = new List<Damage>();

    private GameObject itemPrefab;

    private bool isViewed = false;
    private bool canBeViewed = true;
    private bool isInteractable = false;

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
    public void SetViewAbility(bool state) => canBeViewed = state;
    public GameObject GetPrefab() => itemPrefab;
    public bool isDamaged() => damageTypes.Count > 0;


    IEnumerator InteractionCooldown()
    {
        float timer = 0.5f;

        while (timer > 0) { timer -= Time.deltaTime; yield return null; }

        isInteractable = true;
    }




    // == Interactable ==

    public void Interact()
    {
        if (isInteractable && canBeViewed)
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



[Serializable]
public class Damage
{
    public DamageType markType;
    public GameObject markObject;
    
}

public enum DamageType
{
    NoQR, Spit
}
