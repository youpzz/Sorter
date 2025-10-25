using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    [Header("UI")]
    [SerializeField] private Image holdProgressBar;
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private TMP_Text itemNameText;

    [Header("Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask obstacleLayer;
    public float interactionDistance = 2.5f;
    [SerializeField] private float maxHold = 1f;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup interactableCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;
    private bool seeInteractable;
    private IInteractable currentInteractable;
    private Usable currentUsable;
    private GameObject currentGameobject;

    private float holdProgress;

    // --- Unity Methods ---

    void Awake()
    {
        Instance = this;
        itemNameText.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
        interactableCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        HandleUI();
        DetectInteractable();
        HandleInteractionInput();
    }

    private void DetectInteractable()
    {
        Ray ray = GetCenterRay();
        bool foundInteractable = false;

        int combinedMask = interactableLayer | obstacleLayer;

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, combinedMask))
        {
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
            {
                if (IsVisible(hit))
                {
                    IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

                    if (interactable != null)
                    {
                        foundInteractable = true;
                        currentGameobject = hit.collider.gameObject;
                        if (currentGameobject.GetComponent<Outline>()) currentGameobject.GetComponent<Outline>().enabled = true;

                        if (interactable != currentInteractable)
                        {
                            ResetCurrentInteractable();
                            currentInteractable = interactable;

                            string interactionString;
                            currentUsable = hit.collider.GetComponentInParent<Usable>();
                            if (currentUsable != null)
                            {
                                interactionString = interactable.GetInteractionText() + "\n" + currentUsable.GetUsableInteractionText();
                                ShowInteractionPrompt(interactable.GetNameText(), interactionString);
                                return;
                            }

                            ShowInteractionPrompt(interactable.GetNameText(), interactable.GetInteractionText());
                        }
                    }
                }
            }
            else
            {
                foundInteractable = false;
            }
        }

        if (foundInteractable != seeInteractable)
        {
            seeInteractable = foundInteractable;
            SetVisibility(seeInteractable);
        }

        if (!foundInteractable && currentInteractable != null)
        {
            ResetCurrentInteractable();
        }
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable == null) return;

        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact(currentInteractable);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            holdProgress = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentUsable != null)
            {
                currentUsable.PickUp();
            }
        }
    }

    // --- Interaction ---
    private void Interact(IInteractable interactable)
    {
        // Защита от повторного осмотра
        if (ItemViewer.Instance != null && ItemViewer.Instance.isViewing()) return;

        interactable.Interact();
        ResetCurrentInteractable();
    }



    public void ResetCurrentInteractable()
    {
        if (currentInteractable == null) return;

        if (currentGameobject.GetComponent<Outline>()) currentGameobject.GetComponent<Outline>().enabled = false;
        currentGameobject = null;

        holdProgress = 0;
        currentInteractable = null;
        currentUsable = null;

        

        HideInteractionPrompt();
        if (seeInteractable)
        {
            seeInteractable = false;
            SetVisibility(false);
        }
    }
    private void HandleUI()
    {
        if (!holdProgressBar) return;

        holdProgressBar.gameObject.SetActive(holdProgress > 0);
        holdProgressBar.fillAmount = holdProgress / maxHold;
    }

    private void ShowInteractionPrompt(string itemName, string interactName)
    {
        if (itemNameText != null) itemNameText.text = itemName;
        if (interactionText != null) interactionText.text = interactName;

        if (itemNameText != null) itemNameText.gameObject.SetActive(true);
        if (interactionText != null) interactionText.gameObject.SetActive(true);
    }

    private void HideInteractionPrompt()
    {
        if (itemNameText != null) itemNameText.gameObject.SetActive(false);
        if (interactionText != null) interactionText.gameObject.SetActive(false);
    }

    private Ray GetCenterRay() =>
        playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

    private bool IsVisible(RaycastHit hit)
    {
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = (hit.point - origin).normalized;
        float distance = Vector3.Distance(origin, hit.point);

        if (Physics.Raycast(origin, direction, out RaycastHit blockerHit, distance - 0.05f, obstacleLayer))
        {
            return false;
        }

        return true;
    }

    private void SetVisibility(bool visible)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvasGroupSmooth(visible));
    }

    private IEnumerator FadeCanvasGroupSmooth(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f;
        float startAlpha = interactableCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration);

            interactableCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0f, 1f, progress));

            yield return null;
        }

        interactableCanvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }

    public void Show() => SetVisibility(true);
    public void Hide() => SetVisibility(false);
    public void Toggle() => SetVisibility(!seeInteractable);
}

// --- Interface ---
public interface IInteractable
{
    void Interact();
    string GetInteractionText();
    string GetNameText();
}

public interface Usable
{

    string GetUsableInteractionText();
    void PickUp();
    string GetNameText();
}