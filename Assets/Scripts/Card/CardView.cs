using UnityEngine;

public class CardView : MonoBehaviour
{
    private CardMeshGenService cardMeshGenService;
    [SerializeField] private CardDataMockupDataConfig mockData;
    [SerializeField] private GameObject meshGenRoot;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private void Start()
    {
        ResolveDependency();
    }

    private void ResolveDependency()
    {
        cardMeshGenService = CardMeshGenService.Instance;
        if (cardMeshGenService == null)
        {
            cardMeshGenService = FindFirstObjectByType<CardMeshGenService>();
        }
    }

    [ContextMenu(nameof(GenerateMockData))]
    public void GenerateMockData()
    {
        ResolveDependency();
        var data = mockData.CardDataMockupData as ICardData;
        cardMeshGenService.GenerateMesh(data, meshGenRoot);
    }

    public void CacheOriginalTransform()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    public void RestoreOriginalTransform()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
    }

    public Vector3 OriginalPosition => originalPosition;
    public Quaternion OriginalRotation => originalRotation;
    public Vector3 OriginalScale => originalScale;
}