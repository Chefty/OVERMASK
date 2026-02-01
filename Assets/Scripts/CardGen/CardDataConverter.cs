using UnityEngine;

public static class CardDataConverter
{
    /// <summary>
    /// Converts a square-length byte array into a CardCellDefinition grid.
    /// Length must be a perfect square and maps row-major (row * size + col).
    /// </summary>
    public static CardCellDefinition[][] ToArray(byte[] data)
    {
        if (data == null)
        {
            Debug.LogWarning("CardDataConverter: data is null.");
            return null;
        }

        int length = data.Length;
        if (length == 0)
        {
            return new CardCellDefinition[0][];
        }

        int size = Mathf.RoundToInt(Mathf.Sqrt(length));
        if (size * size != length)
        {
            Debug.LogWarning($"CardDataConverter: data length {length} is not a perfect square.");
            return null;
        }

        CardCellDefinition[][] result = new CardCellDefinition[size][];
        for (int r = 0; r < size; r++)
        {
            result[r] = new CardCellDefinition[size];
            for (int c = 0; c < size; c++)
            {
                int index = r * size + c;
                result[r][c] = (CardCellDefinition)data[index];
            }
        }

        return result;
    }
}
