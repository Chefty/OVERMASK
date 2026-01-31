using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private CardGenService cardGenService;
    [SerializeField] private CardDataMockupDataConfig mockData;
    [SerializeField] private GameObject meshGenRoot;

    [ContextMenu(nameof(GenerateMockData))]
    public void GenerateMockData()
    {
        var data = mockData.CardDataMockupData as ICardData;
        cardGenService.GenerateMesh(data, meshGenRoot);
    }
}
