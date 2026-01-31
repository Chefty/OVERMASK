using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardGenService : MonoBehaviour
{
    [SerializeField] private Mesh cellMesh;
    [SerializeField] private Mesh emptyCellMesh;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material grayMaterial;
    [SerializeField] private Material maskMaterial;
    [SerializeField] private Material emptyCellMaterial;
    [SerializeField] private Vector2 targetSize = new Vector2(3f, 3f);

    public void GenerateMesh(ICardData data, GameObject target)
    {
        if (cellMesh == null)
        {
            Debug.LogWarning("CardGenService: cellMesh is not assigned.", this);
            return;
        }

        if (data == null || data.Data == null)
        {
            Debug.LogWarning("CardGenService: data is null or empty.", this);
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("CardGenService: target GameObject is null.", this);
            return;
        }

        List<CombineInstance> redCombines = new();
        List<CombineInstance> blueCombines = new();
        List<CombineInstance> grayCombines = new();
        List<CombineInstance> emptyCombines = new();
        List<CombineInstance> emptyCellCombines = new();

        CardCellDefinition[][] grid = data.Data;
        int rowCount = grid.Length;
        int columnCount = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            if (grid[r] != null && grid[r].Length > columnCount)
            {
                columnCount = grid[r].Length;
            }
        }

        float xScale = columnCount > 0 ? targetSize.x / columnCount : 1f;
        float zScale = rowCount > 0 ? targetSize.y / rowCount : 1f;
        Vector3 cellScale = new Vector3(xScale, 1f, zScale);
        for (int r = 0; r < grid.Length; r++)
        {
            CardCellDefinition[] row = grid[r];
            if (row == null)
            {
                continue;
            }

            for (int c = 0; c < row.Length; c++)
            {
                CardCellDefinition cell = row[c];
                Vector3 cellPosition = new Vector3(
                    (c + 0.5f) * xScale,
                    0f,
                    -(r + 0.5f) * zScale);

                CombineInstance instance = new CombineInstance
                {
                    mesh = cellMesh,
                    transform = Matrix4x4.TRS(cellPosition, Quaternion.identity, cellScale)
                };

                if (cell == CardCellDefinition.Empty)
                {
                    emptyCombines.Add(instance);
                    if (emptyCellMesh != null)
                    {
                        CombineInstance emptyCellInstance = new CombineInstance
                        {
                            mesh = emptyCellMesh,
                            transform = Matrix4x4.TRS(cellPosition, Quaternion.identity, cellScale)
                        };
                        emptyCellCombines.Add(emptyCellInstance);
                    }
                    continue;
                }

                if (cell == CardCellDefinition.Red)
                {
                    redCombines.Add(instance);
                }
                else if (cell == CardCellDefinition.Blue)
                {
                    blueCombines.Add(instance);
                }
                else if (cell == CardCellDefinition.Gray)
                {
                    grayCombines.Add(instance);
                }
            }
        }

        Mesh redMesh = BuildSubmesh(redCombines);
        Mesh blueMesh = BuildSubmesh(blueCombines);
        Mesh grayMesh = BuildSubmesh(grayCombines);
        Mesh maskMesh = BuildSubmesh(emptyCombines);
        Mesh emptyCellMeshCombined = BuildSubmesh(emptyCellCombines);

        Mesh combined = new Mesh();
        int totalVertexCount = redMesh.vertexCount + blueMesh.vertexCount + grayMesh.vertexCount;
        if (totalVertexCount > 65535)
        {
            combined.indexFormat = IndexFormat.UInt32;
        }

        CombineInstance[] submeshes = new CombineInstance[3];
        submeshes[0] = new CombineInstance { mesh = redMesh, transform = Matrix4x4.identity };
        submeshes[1] = new CombineInstance { mesh = blueMesh, transform = Matrix4x4.identity };
        submeshes[2] = new CombineInstance { mesh = grayMesh, transform = Matrix4x4.identity };
        combined.CombineMeshes(submeshes, false, false);
        combined.RecalculateBounds();

        MeshFilter filter = target.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = target.AddComponent<MeshFilter>();
        }

        MeshRenderer renderer = target.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = target.AddComponent<MeshRenderer>();
        }

        filter.sharedMesh = combined;
        renderer.sharedMaterials = new[] { redMaterial, blueMaterial, grayMaterial };

        SetupMaskMesh(target.transform, maskMesh);
        SetupEmptyCellMesh(target.transform, emptyCellMeshCombined);
    }

    private static Mesh BuildSubmesh(List<CombineInstance> combines)
    {
        Mesh mesh = new Mesh();
        if (combines.Count == 0)
        {
            return mesh;
        }

        int vertexCount = 0;
        for (int i = 0; i < combines.Count; i++)
        {
            if (combines[i].mesh != null)
            {
                vertexCount += combines[i].mesh.vertexCount;
            }
        }

        if (vertexCount > 65535)
        {
            mesh.indexFormat = IndexFormat.UInt32;
        }

        mesh.CombineMeshes(combines.ToArray(), true, true);
        mesh.RecalculateBounds();
        return mesh;
    }

    private void SetupMaskMesh(Transform parent, Mesh maskMesh)
    {
        if (maskMaterial == null)
        {
            return;
        }

        Transform maskTransform = parent.Find("MaskMesh");
        if (maskTransform == null)
        {
            GameObject maskObject = new GameObject("MaskMesh");
            maskObject.layer = LayerMask.NameToLayer("HoleMask");
            maskTransform = maskObject.transform;
            maskTransform.SetParent(parent, false);
        }

        MeshFilter filter = maskTransform.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = maskTransform.gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer renderer = maskTransform.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = maskTransform.gameObject.AddComponent<MeshRenderer>();
        }

        filter.sharedMesh = maskMesh;
        renderer.sharedMaterial = maskMaterial;
    }

    private void SetupEmptyCellMesh(Transform parent, Mesh emptyMesh)
    {
        if (emptyCellMaterial == null)
        {
            return;
        }

        Transform emptyTransform = parent.Find("EmptyCells");
        if (emptyTransform == null)
        {
            GameObject emptyObject = new GameObject("EmptyCells");
            emptyTransform = emptyObject.transform;
            emptyTransform.SetParent(parent, false);
        }

        MeshFilter filter = emptyTransform.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = emptyTransform.gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer renderer = emptyTransform.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = emptyTransform.gameObject.AddComponent<MeshRenderer>();
        }

        filter.sharedMesh = emptyMesh;
        renderer.sharedMaterial = emptyCellMaterial;
    }
}



