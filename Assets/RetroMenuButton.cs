using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RetroMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Referensi UI")]
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject starIndicator; // Opsional: ikon panah/bintang

    [Header("Efek Hover Retro")]
    [SerializeField] private float hoverOffsetX = 24f;      // Jarak geser ke kanan
    [SerializeField] private float hoverScale = 1.08f;      // Sedikit membesar saat disorot
    [SerializeField] private float animSpeed = 14f;         // Kecepatan transisi
    [SerializeField] private Color normalTint = new Color(0.82f, 0.82f, 0.82f, 1f); // Agak redup saat diam
    [SerializeField] private Color hoverTint = Color.white; // Menyala terang saat di-hover

    [Header("Audio (Opsional)")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hoverClip;

    private RectTransform textRect;
    private Vector2 originalPos;
    private Vector2 targetPos;
    private Vector3 targetScale;
    private Color targetTint;

    private void Start()
    {
        if (buttonText != null)
        {
            textRect = buttonText.GetComponent<RectTransform>();
            // Set pivot ke kiri-tengah agar saat membesar tidak menabrak kursor di kiri
            textRect.pivot = new Vector2(0f, 0.5f);
            
            originalPos = textRect.anchoredPosition;
            targetPos = originalPos;
            targetScale = Vector3.one;
            targetTint = normalTint;
            buttonText.color = normalTint;
        }

        if (starIndicator != null)
            starIndicator.SetActive(false);
    }

    private void Update()
    {
        if (textRect == null) return;

        float dt = Time.unscaledDeltaTime * animSpeed;
        textRect.anchoredPosition = Vector2.Lerp(textRect.anchoredPosition, targetPos, dt);
        textRect.localScale = Vector3.Lerp(textRect.localScale, targetScale, dt);
        buttonText.color = Color.Lerp(buttonText.color, targetTint, dt);
    }

    public void OnPointerEnter(PointerEventData eventData) => SetHoverState(true);
    public void OnPointerExit(PointerEventData eventData) => SetHoverState(false);
    public void OnSelect(BaseEventData eventData) => SetHoverState(true);
    public void OnDeselect(BaseEventData eventData) => SetHoverState(false);

    private void SetHoverState(bool isHovered)
    {
        if (isHovered)
        {
            targetPos = originalPos + new Vector2(hoverOffsetX, 0f);
            targetScale = Vector3.one * hoverScale;
            targetTint = hoverTint;

            if (starIndicator != null) starIndicator.SetActive(true);
            if (sfxSource != null && hoverClip != null) sfxSource.PlayOneShot(hoverClip);
        }
        else
        {
            targetPos = originalPos;
            targetScale = Vector3.one;
            targetTint = normalTint;

            if (starIndicator != null) starIndicator.SetActive(false);
        }
    }
}