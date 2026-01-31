using UnityEngine;

public class CardDataService : MonoBehaviour, ICardDataService
{
    [SerializeField] TextAsset jsonAsset;
    public static CardDataService Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public ICardData GetCardData(int id)
    {
        //we read the json here with id, then we return it
        throw new System.NotImplementedException();
    }
}

public interface ICardDataService
{
    ICardData GetCardData(int id);
}