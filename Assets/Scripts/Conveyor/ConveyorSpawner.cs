using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{

    [SerializeField] private Transform spawnPoint;


    public void SpawnItem(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
