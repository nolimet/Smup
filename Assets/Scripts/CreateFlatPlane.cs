using UnityEngine;

public class CreateFlatPlane : MonoBehaviour
{
    [SerializeField] private float length = 20f, width = 20f;
    [SerializeField] private int resX = 50, resZ = 50; // 2 minimum

    private void Start()
    {
        var filter = gameObject.GetComponent<MeshFilter>();
        var mesh = filter.mesh;
        mesh.Clear();

        #region Vertices

        var vertices = new Vector3[resX * resZ];
        for (var z = 0; z < resZ; z++)
        {
            // [ -length / 2, length / 2 ]
            var zPos = ((float)z / (resZ - 1) - .5f) * length;
            for (var x = 0; x < resX; x++)
            {
                // [ -width / 2, width / 2 ]
                var xPos = ((float)x / (resX - 1) - .5f) * width;
                vertices[x + z * resX] = new Vector3(xPos, 0f, zPos);
            }
        }

        #endregion

        #region Normales

        var normales = new Vector3[vertices.Length];
        for (var n = 0; n < normales.Length; n++)
            normales[n] = Vector3.up;

        #endregion

        #region UVs

        var uvs = new Vector2[vertices.Length];
        for (var v = 0; v < resZ; v++)
        for (var u = 0; u < resX; u++)
            uvs[u + v * resX] = new Vector2((float)u / (resX - 1), (float)v / (resZ - 1));

        #endregion

        #region Triangles

        var nbFaces = (resX - 1) * (resZ - 1);
        var triangles = new int[nbFaces * 6];
        var t = 0;
        for (var face = 0; face < nbFaces; face++)
        {
            // Retrieve lower left corner from face ind
            var i = face % (resX - 1) + face / (resZ - 1) * resX;

            triangles[t++] = i + resX;
            triangles[t++] = i + 1;
            triangles[t++] = i;

            triangles[t++] = i + resX;
            triangles[t++] = i + resX + 1;
            triangles[t++] = i + 1;
        }

        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        ;
    }
}
