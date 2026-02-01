using UnityEngine;

public class CardView : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_BaseColor");
    private CardMeshGenService cardMeshGenService;
    [SerializeField] private CardDataMockupDataConfig mockData;
    [SerializeField] private GameObject meshGenRoot;
    [SerializeField] private Renderer FactionMeshRenderer;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    public bool IsPlaced { get; private set; }
    public int id;
    [SerializeField] private PlayerFaction  playerFaction;
    [SerializeField] Color PlayerColor;
    [SerializeField] Color OpponentColor;
    [SerializeField] Color HouseColor;

    private void Start()
    {
        ResolveDependency();
    }

    public void SetupCard(CardGenContext context)
    {
        transform.parent = context.Parent;
        transform.localPosition = context.Position;
        transform.rotation = context.Rotation;
        transform.localScale = context.Scale;
        playerFaction  = context.Faction;
        ApplyFactionColor();

        CardMeshGenService.Instance.GenerateMesh(context.Data, meshGenRoot);
    }

    private void ApplyFactionColor()
    {
        var mpb = new MaterialPropertyBlock();
        var color = new Color();
        switch (playerFaction)
        {
            case PlayerFaction.House:
                color = HouseColor;
                break;
            case PlayerFaction.Opponent:
                color = OpponentColor;
                break;
            case PlayerFaction.Player:
                color = PlayerColor;
                break;
        }
        mpb.SetColor(ColorId, color);
        FactionMeshRenderer.SetPropertyBlock(mpb, 0);
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
        ApplyFactionColor();
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

    public void OnPlaced()
    {
        IsPlaced = true;
    }

    public Vector3 OriginalPosition => originalPosition;
    public Quaternion OriginalRotation => originalRotation;
    public Vector3 OriginalScale => originalScale;
}