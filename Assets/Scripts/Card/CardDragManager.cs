using DG.Tweening;
using UnityEngine;

public class CardDragManager : MonoBehaviour
{
    [SerializeField] private float dragHeightY = 0.5f;
    [SerializeField] private float maxRayDistance = 200f;
    [SerializeField] private float snapDuration = 0.5f;
    [SerializeField] private Ease snapEase = Ease.OutQuad;

    public static CardDragManager Instance { get; private set; }

    private Camera mainCamera;
    private CardView activeCard;
    private bool canDragCard = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Game.Instance.Round.OnCardRequested.AddListener(OnCardRequested);
        
        mainCamera = Camera.main;
    }

    private void OnCardRequested()
    {
        canDragCard = true;
    }

    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!canDragCard) return;
            TrySelectTarget();
        }

        if (Input.GetMouseButton(0))
        {
            DragTarget();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (activeCard != null)
            {
                if (Input.mousePosition.y <= Screen.height * 0.2f)
                {
                    Transform cardTransform = activeCard.transform;
                    DOTween.Kill(cardTransform);
                    cardTransform.DOMove(activeCard.OriginalPosition, snapDuration)
                        .SetEase(snapEase)
                        .SetTarget(cardTransform);
                    cardTransform.DORotateQuaternion(activeCard.OriginalRotation, snapDuration)
                        .SetEase(snapEase)
                        .SetTarget(cardTransform);
                    cardTransform.DOScale(activeCard.OriginalScale, snapDuration)
                        .SetEase(snapEase)
                        .SetTarget(cardTransform);
                }
                else
                {
                    var slot = PlaymatView.Instance.GetCardDisplayerForFaction(Game.Instance.Round.LocalPlayer.Faction);
                    if (slot != null)
                    {
                        var cardTransform = activeCard.transform;
                        DOTween.Kill(cardTransform);
                        cardTransform.SetParent(slot.transform, true);
                        cardTransform.DOMove(slot.transform.position, snapDuration)
                            .SetEase(snapEase)
                            .SetTarget(cardTransform);
                        cardTransform.DORotateQuaternion(Quaternion.identity, snapDuration)
                            .SetEase(snapEase)
                            .SetTarget(cardTransform);
                        cardTransform.DOScale(Vector3.one, snapDuration)
                            .SetEase(snapEase)
                            .SetTarget(cardTransform);
                        activeCard.OnPlaced();
                        canDragCard = false;
                    }
                }
            }
            activeCard = null;
        }
    }

    private void TrySelectTarget()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
        {
            var possibleCard = hit.transform.GetComponentInParent<CardView>();
            if (possibleCard == null || possibleCard.playerFaction != PlayerFaction.Red)
            {
                return;
            }

            activeCard = hit.transform.GetComponentInParent<CardView>();
            if (activeCard != null)
            {
                if (activeCard.IsPlaced)
                {
                    activeCard = null;
                    return;
                }

                activeCard.CacheOriginalTransform();
            }
        }
    }

    private void DragTarget()
    {
        if (activeCard == null)
        {
            return;
        }

        Plane dragPlane = new Plane(Vector3.up, new Vector3(0f, dragHeightY, 0f));
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPoint = ray.GetPoint(enter);
            Transform cardTransform = activeCard.transform;
            cardTransform.position = new Vector3(worldPoint.x, dragHeightY, worldPoint.z);
            cardTransform.rotation = Quaternion.identity;
        }
    }
}
