using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isWorking = true;
    [SerializeField] private Vector3 direction = Vector3.forward;

    private Rigidbody rb_;



    void Update()
    {
        if (!isWorking) return;

        if (rb_) rb_.linearVelocity = direction.normalized * speed;
    }

    void OnCollisionStay(Collision collision)
    {
        if (!isWorking) return;

        rb_ = collision.gameObject.GetComponent<Rigidbody>();
    }


    public void SwitchPower(bool state) => isWorking = state;
}
