using UnityEngine;

public class ConveyorDestroyer : MonoBehaviour
{
    public static ConveyorDestroyer Instance;


    bool isDestroying;

    void Awake()
    {
        Instance = this;
    }



    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<ItemPickable>()) return;

        ItemPickable itemPickable = other.GetComponent<ItemPickable>();

        DestroyItem(itemPickable);
        SpawnNewItem();
    }


    void DestroyItem(ItemPickable item)
    {
        bool isCorrect = false;
        isCorrect = isDestroying ? item.isDamaged() ? true : false : item.isDamaged() ? false : true;
        PlayerScore.Instance.AddScore(isCorrect ? 1 : -1);


        Destroy(item.gameObject);

    }

    public void ChangeDestroyState(bool state) => isDestroying = !state; 


    void SpawnNewItem()
    {
        ConveyorManager.Instance.SpawnNewItem();
    }
}
