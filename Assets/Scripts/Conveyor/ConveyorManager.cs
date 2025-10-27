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
    [SerializeField] private ItemPickable currentPickable;

    [Space(10)]
    [SerializeField] private Transform spawnPoint;

    private ItemScriptable[] itemScriptables;


    void Awake()
    {
        Instance = this;
        LoadItems();
    }

    void Start()
    {
        StartCoroutine(WaitForCooldown());
    }

    void LoadItems()
    {
        itemScriptables = Resources.LoadAll<ItemScriptable>("Items");
        Debug.Log($"Подгружено {itemScriptables.Length} предметов");
    }

    IEnumerator WaitForCooldown()
    {
        SpawnItem();
        
        float timer = stopTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        conveyorBelt.SwitchPower(false);
    }


    public void SpawnNewItem() => StartCoroutine(WaitForCooldown());

    void SpawnItem()
    {
        ItemScriptable randomItem = itemScriptables[Random.Range(0, itemScriptables.Length)];
        currentItem = randomItem;
        currentPickable = Instantiate(randomItem.itemPrefab, spawnPoint.position, Quaternion.identity).GetComponent<ItemPickable>();
    }

    void MakeItemDecicion(bool state)
    {
        conveyorDestroyer.ChangeDestroyState(state);
        StopAllCoroutines();
        currentPickable.SetViewAbility(false);
        conveyorBelt.SwitchPower(true);
        Debug.Log("Нажата кнопка");
    }


    public ItemScriptable GetCurrentItem() => currentItem;


    
    public void ApproveItem(bool state)
    {
        MakeItemDecicion(state);
    }


    
}
