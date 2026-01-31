using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataMockupData", menuName = "CardDataMockupData", order = 1)]
public class CardDataMockupDataConfig : ScriptableObject
{
    [SerializeField] private CardDataMockupData cardDataMockupData;
}


[System.Serializable]
public class CardDataMockupData : ICardData
{
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 3;
    [SerializeField] private List<CardCellDefinitionRow> rowsData = new();

    public CardCellDefinition[][] Data
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
}

public interface ICardData
{
    public CardCellDefinition[][] Data { get; set; }
}