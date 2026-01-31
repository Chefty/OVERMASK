using UnityEngine;

public class CardView : MonoBehaviour
{
    private CardMeshGenService cardMeshGenService;
    [SerializeField] private CardDataMockupDataConfig mockData;
    [SerializeField] private GameObject meshGenRoot;

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
}