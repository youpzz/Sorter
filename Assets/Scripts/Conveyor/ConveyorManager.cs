using System.Collections;
using System.Threading;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    public static ConveyorManager Instance;

    [Header("Референсы")]
    [SerializeField] private ConveyorBelt conveyorBelt;
    [SerializeField] private ConveyorSpawner conveyorSpawner;
    [SerializeField] private ConveyorDestroyer conveyorDestroyer;

    [Space(10)]
    [Header("Настройки")]
    [SerializeField] private float spawnTime = 60;

    [SerializeField] private float stopTime = 3;


    [Space(10)]

    [SerializeField] private ItemScriptable currentItem;

    private ItemScriptable[] itemScriptables;


    void Awake()
    {
        Instance = this;
        LoadItems();
    }

    void Start()
    {
        StartCoroutine(waitForCooldown());
    }

    void LoadItems()
    {
        itemScriptables = Resources.LoadAll<ItemScriptable>("Items");
        Debug.Log($"Подгружено {itemScriptables.Length} предметов");
    }

    IEnumerator waitForCooldown()
    {
        SpawnItem();
        
        float timer = spawnTime;
        while (timer > 0)
        {
            if (timer <= spawnTime - stopTime) conveyorBelt.SwitchPower(false);
            timer -= Time.deltaTime;
            yield return null;
        }

        conveyorBelt.SwitchPower(true);


    }


    void SpawnItem()
    {
        ItemScriptable randomItem = itemScriptables[Random.Range(0, itemScriptables.Length)];
        currentItem = randomItem;
        conveyorSpawner.SpawnItem(randomItem.itemPrefab);
    }

    
}
