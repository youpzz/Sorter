using UnityEngine;

public class ConveyorButton : MonoBehaviour, IInteractable
{

    [SerializeField] private bool isApprove;



    void Use()
    {
        ConveyorManager.Instance.ApproveItem(isApprove);


    }




    public void Interact()
    {
        Use();
    }

    public string GetInteractionText()
    {
        return "Нажать [E]";
    }
    
    public string GetNameText()
    {
        return isApprove ? "Принять" : "Отклонить";
    }

}
