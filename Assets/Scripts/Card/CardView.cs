using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

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
    public byte id;
    public PlayerFaction  playerFaction;
    [FormerlySerializedAs("PlayerColor")] [SerializeField] Color BlueColor;
    [FormerlySerializedAs("OpponentColor")] [SerializeField] Color RedColor;
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
        id = context.Data.CardId;
        ApplyFactionColor();

        CardMeshGenService.Instance.GenerateMesh(context.Data, meshGenRoot);
    }

    private void ApplyFactionColor()
    {
        var mpb = new MaterialPropertyBlock();
        var color = new Color();
        switch (playerFaction)
        {
            case PlayerFaction.Mask:
                color = HouseColor;
                break;
            case PlayerFaction.Blue:
                color = BlueColor;
                break;
            case PlayerFaction.Red:
                color = RedColor;
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
        PlaymatView.Instance.GetCardDisplayerForFaction(Game.Instance.Round.LocalPlayer.Faction).ForceCardView(this);
        PlayerHand.Instance.RemoveCard(this);
        Game.Instance.Round.ChooseCard(id);
        FactionMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    }

    public Vector3 OriginalPosition => originalPosition;
    public Quaternion OriginalRotation => originalRotation;
    public Vector3 OriginalScale => originalScale;
}