using System;
using UnityEngine;

public class CardGenService : MonoBehaviour, ICardGenService
{
    [SerializeField] private CardView CardPrefab;
    public  static CardGenService Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public CardView GenCard(CardGenContext context)
    {
        var go = Instantiate(CardPrefab, context.Parent);
        var cardView = go.GetComponent<CardView>();
        cardView.SetupCard(context);
        return cardView;
    }
}


public interface ICardGenService
{
    public CardView GenCard(CardGenContext context);
}


public class CardGenContext
{
    public int CardId;
    public ICardData Data;
    public PlayerFaction Faction;
    public Transform Parent = null;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}


public enum PlayerFaction
{
    House,
    Player,
    Opponent
}