using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CardDataMockupData))]
public class CardDataMockupDataDrawer : PropertyDrawer
{
    private const float RowLabelWidth = 26f;
    private const float MinCellWidth = 60f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty rowsProp = property.FindPropertyRelative("rows");
        SerializedProperty columnsProp = property.FindPropertyRelative("columns");
        SerializedProperty rowsDataProp = property.FindPropertyRelative("rowsData");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float y = position.y;

        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, y, position.width, lineHeight),
            property.isExpanded,
            label,
            true);

        y += lineHeight + spacing;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            Rect rowsRect = new Rect(position.x, y, position.width * 0.5f - 2f, lineHeight);
            Rect colsRect = new Rect(position.x + position.width * 0.5f + 2f, y, position.width * 0.5f - 2f, lineHeight);

            int newRows = Mathf.Max(0, EditorGUI.IntField(rowsRect, "Rows", rowsProp.intValue));
            int newColumns = Mathf.Max(0, EditorGUI.IntField(colsRect, "Cols", columnsProp.intValue));

            if (newRows != rowsProp.intValue)
            {
                rowsProp.intValue = newRows;
            }

            if (newColumns != columnsProp.intValue)
            {
                columnsProp.intValue = newColumns;
            }

            y += lineHeight + spacing;

            EnsureListSizes(rowsProp, columnsProp, rowsDataProp);

            int rows = rowsProp.intValue;
            int columns = columnsProp.intValue;
            float gridWidth = Mathf.Max(0f, position.width - RowLabelWidth - 4f);
            float cellWidth = Mathf.Max(MinCellWidth, columns > 0 ? gridWidth / columns : gridWidth);

            for (int r = 0; r < rows; r++)
            {
                SerializedProperty rowProp = rowsDataProp.GetArrayElementAtIndex(r);
                SerializedProperty cellsProp = rowProp.FindPropertyRelative("cells");

                Rect labelRect = new Rect(position.x, y, RowLabelWidth, lineHeight);
                EditorGUI.LabelField(labelRect, $"R{r}");

                float cellX = position.x + RowLabelWidth + 4f;
                for (int c = 0; c < columns; c++)
                {
                    Rect cellRect = new Rect(cellX + c * cellWidth, y, cellWidth - 2f, lineHeight);
                    SerializedProperty cellProp = cellsProp.GetArrayElementAtIndex(c);
                    EditorGUI.PropertyField(cellRect, cellProp, GUIContent.none);
                }

                y += lineHeight + spacing;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        if (!property.isExpanded)
        {
            return lineHeight;
        }

        SerializedProperty rowsProp = property.FindPropertyRelative("rows");
        int rows = Mathf.Max(0, rowsProp.intValue);
        float rowsHeight = rows * (lineHeight + spacing);

        return lineHeight + spacing + lineHeight + spacing + rowsHeight;
    }

    private static void EnsureListSizes(
        SerializedProperty rowsProp,
        SerializedProperty columnsProp,
        SerializedProperty rowsDataProp)
    {
        int rows = Mathf.Max(0, rowsProp.intValue);
        int columns = Mathf.Max(0, columnsProp.intValue);

        while (rowsDataProp.arraySize < rows)
        {
            rowsDataProp.InsertArrayElementAtIndex(rowsDataProp.arraySize);
        }

        while (rowsDataProp.arraySize > rows)
        {
            rowsDataProp.DeleteArrayElementAtIndex(rowsDataProp.arraySize - 1);
        }

        for (int r = 0; r < rowsDataProp.arraySize; r++)
        {
            SerializedProperty rowProp = rowsDataProp.GetArrayElementAtIndex(r);
            SerializedProperty cellsProp = rowProp.FindPropertyRelative("cells");

            while (cellsProp.arraySize < columns)
            {
                cellsProp.InsertArrayElementAtIndex(cellsProp.arraySize);
            }

            while (cellsProp.arraySize > columns)
            {
                cellsProp.DeleteArrayElementAtIndex(cellsProp.arraySize - 1);
            }
        }
    }
}
