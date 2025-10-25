using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isWorking = true;
    [SerializeField] private Vector3 direction = Vector3.forward;




    void OnCollisionStay(Collision collision)
    {
        if (!isWorking) return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = direction.normalized * speed;
    }


    public void SwitchPower(bool state) => isWorking = state;
}
