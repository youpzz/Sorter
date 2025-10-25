using UnityEngine;

public class ItemViewer : MonoBehaviour
{
    public static ItemViewer Instance;


    [SerializeField] private Transform viewPoint; // Позиция перед игроком
    private GameObject currentItem;
    private ItemPickable currentPickable;
    private Vector3 rotationVelocity = new Vector3(0, 100f, 0);


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

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            currentPickable.CanView(true);
            Destroy(currentItem);
            currentItem = null;
            currentPickable = null;
            SetCursorVisibility(false);
        }

        if (Input.GetMouseButton(0))
        {
            float rotateX = Input.GetAxis("Mouse X") * rotationVelocity.y * Time.deltaTime;
            float rotateY = -Input.GetAxis("Mouse Y") * rotationVelocity.y * Time.deltaTime;

            currentItem.transform.Rotate(Camera.main.transform.up, rotateX, Space.World);
            currentItem.transform.Rotate(Camera.main.transform.right, rotateY, Space.World);
        }
    }

    

    public bool isViewing() => currentItem != null;
}
