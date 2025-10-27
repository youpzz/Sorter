using Unity.VisualScripting;
using UnityEngine;

public class ItemViewer : MonoBehaviour
{
    public static ItemViewer Instance;


    [SerializeField] private Transform viewPoint; // Позиция перед игроком

    [SerializeField] private float inspectionMultiplier = 1;

    [Space(10)]
    [Header("Визуал")]

    [SerializeField] private GameObject violetLight;
    [SerializeField] private GameObject qrChecker;



    private GameObject currentItem;
    private ItemPickable currentPickable;
    private Vector3 rotationVelocity = new Vector3(0, 200f, 0);


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HandleItemInspection();
    }

    public void ShowItem(ItemPickable itemPickable)
    {
        if (currentItem != null) Destroy(currentItem);

        currentPickable = itemPickable;
        currentItem = Instantiate(itemPickable.GetPrefab(), viewPoint.position, Quaternion.identity);
        currentItem.transform.SetParent(viewPoint);
        Debug.Log(currentItem);
        SetCursorVisibility(true);
        
    }

    void SetCursorVisibility(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    void HandleItemInspection()
    {
        if (currentItem == null) return;


        HandleItemDeEquip();
        HandleItemRotation();
    }


    void HandleItemDeEquip()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
        {
            currentPickable.gameObject.SetActive(true);
            currentPickable.CanView(true);
            Destroy(currentItem);
            currentItem = null;
            currentPickable = null;
            SetCursorVisibility(false);

        }
    }
    
    void HandleItemRotation()
    {
        if (Input.GetMouseButton(0))
        {
            if (currentItem == null) return;

            float rotateX = Input.GetAxis("Mouse X") * rotationVelocity.y * inspectionMultiplier * Time.deltaTime;
            float rotateY = -Input.GetAxis("Mouse Y") * rotationVelocity.y * inspectionMultiplier * Time.deltaTime;

            currentItem.transform.Rotate(Camera.main.transform.up, rotateX, Space.World);
            currentItem.transform.Rotate(Camera.main.transform.right, rotateY, Space.World);
        }
    }

    

    public bool isViewing() => currentItem != null;
}
