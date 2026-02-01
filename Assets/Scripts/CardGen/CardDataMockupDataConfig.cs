using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataMockupData", menuName = "CardDataMockupData", order = 1)]
public class CardDataMockupDataConfig : ScriptableObject
{
    [SerializeField] private int redCount;
    [SerializeField] private int blueCount;
    [SerializeField] private int grayCount;

    [SerializeField] private CardDataMockupData cardDataMockupData;
    public CardDataMockupData CardDataMockupData => cardDataMockupData;

    [ContextMenu("GenerateMockData")]
    public void GenerateMockData()
    {
        if (cardDataMockupData == null)
        {
            Debug.LogWarning("CardDataMockupDataConfig: cardDataMockupData is not assigned.", this);
            return;
        }

        CardCellDefinition[][] current = cardDataMockupData.ArrayData;
        if (current == null)
        {
            Debug.LogWarning("CardDataMockupDataConfig: data grid is null.", this);
            return;
        }

        int rows = current.Length;
        int columns = 0;
        for (int r = 0; r < rows; r++)
        {
            if (current[r] != null && current[r].Length > columns)
            {
                columns = current[r].Length;
            }
        }

        int capacity = rows * columns;
        int totalRequested = Mathf.Max(0, redCount) + Mathf.Max(0, blueCount) + Mathf.Max(0, grayCount);
        if (capacity == 0)
        {
            Debug.LogWarning("CardDataMockupDataConfig: grid size is zero.", this);
            return;
        }

        if (totalRequested > capacity)
        {
            Debug.LogWarning("CardDataMockupDataConfig: counts exceed grid capacity, clamping.", this);
        }

        int remaining = capacity;
        int redsToPlace = Mathf.Clamp(redCount, 0, remaining);
        remaining -= redsToPlace;
        int bluesToPlace = Mathf.Clamp(blueCount, 0, remaining);
        remaining -= bluesToPlace;
        int graysToPlace = Mathf.Clamp(grayCount, 0, remaining);

        List<CardCellDefinition> pool = new List<CardCellDefinition>(capacity);
        for (int i = 0; i < redsToPlace; i++)
        {
            pool.Add(CardCellDefinition.Red);
        }
        for (int i = 0; i < bluesToPlace; i++)
        {
            pool.Add(CardCellDefinition.Blue);
        }
        for (int i = 0; i < graysToPlace; i++)
        {
            pool.Add(CardCellDefinition.Gray);
        }
        while (pool.Count < capacity)
        {
            pool.Add(CardCellDefinition.Empty);
        }

        for (int i = pool.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            (pool[i], pool[swapIndex]) = (pool[swapIndex], pool[i]);
        }

        CardCellDefinition[][] data = new CardCellDefinition[rows][];
        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            data[r] = new CardCellDefinition[columns];
            for (int c = 0; c < columns; c++)
            {
                data[r][c] = pool[index];
                index++;
            }
        }

        cardDataMockupData.ArrayData = data;
    }
}


[System.Serializable]
public class CardDataMockupData : ICardData
{
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 3;
    [SerializeField] private List<CardCellDefinitionRow> rowsData = new();
    [SerializeField] private int mockId = 0;

    public int CardId => mockId;

    public CardCellDefinition[][] ArrayData
    {
        get
        {
            var result = new CardCellDefinition[rows][];
            for (int r = 0; r < rows; r++)
            {
                result[r] = new CardCellDefinition[columns];
                if (r >= rowsData.Count)
                {
                    continue;
                }

                var cells = rowsData[r].Cells;
                for (int c = 0; c < columns; c++)
                {
                    if (c < cells.Count)
                    {
                        result[r][c] = cells[c];
                    }
                }
            }
            return result;
        }
        set
        {
            if (value == null)
            {
                rows = 0;
                columns = 0;
                rowsData.Clear();
                return;
            }

            rows = value.Length;
            columns = 0;
            for (int r = 0; r < rows; r++)
            {
                int rowLength = value[r]?.Length ?? 0;
                if (rowLength > columns)
                {
                    columns = rowLength;
                }
            }

            rowsData = new List<CardCellDefinitionRow>(rows);
            for (int r = 0; r < rows; r++)
            {
                var row = new CardCellDefinitionRow();
                for (int c = 0; c < columns; c++)
                {
                    if (value[r] != null && c < value[r].Length)
                    {
                        row.Cells.Add(value[r][c]);
                    }
                    else
                    {
                        row.Cells.Add(CardCellDefinition.Empty);
                    }
                }
                rowsData.Add(row);
            }
        }
    }
}

[System.Serializable]
public class CardCellDefinitionRow
{
    [SerializeField] private List<CardCellDefinition> cells = new();

    public List<CardCellDefinition> Cells => cells;
}

public enum CardCellDefinition
{
    Empty = 0,
    Red = 1,
    Blue = 2,
    Gray = 3,
}

public interface ICardData
{
    public int CardId { get; }
    public CardCellDefinition[][] ArrayData { get;}
}