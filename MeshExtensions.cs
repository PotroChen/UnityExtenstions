using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshExtensions
{
    private class VertexInfo
    {
        List<Vector3> vertices = null;
        List<Vector2> uv = null;
        List<Vector2> uv2 = null;
        List<Vector2> uv3 = null;
        List<Vector2> uv4 = null;
        List<Vector3> normals = null;
        List<Vector4> tangents = null;
        List<Color32> colors = null;
        List<BoneWeight> boneWeights = null;

        public VertexInfo()
        {
            vertices = new List<Vector3>();
        }
        public VertexInfo(Mesh mesh)
        {
            vertices = CreateList(mesh.vertices);
            uv = CreateList(mesh.uv);
            uv2 = CreateList(mesh.uv2);
            uv3 = CreateList(mesh.uv3);
            uv4 = CreateList(mesh.uv4);
            normals = CreateList(mesh.normals);
            tangents = CreateList(mesh.tangents);
            colors = CreateList(mesh.colors32);
            boneWeights = CreateList(mesh.boneWeights);
        }

        private List<T> CreateList<T>(T[] source)
        {
            if (source == null || source.Length == 0)
                return null;
            return new List<T>(source);
        }
        private void Copy<T>(ref List<T> dest, List<T> source, int index)
        {
            if (source == null)
                return;
            if (dest == null)
                dest = new List<T>();
            dest.Add(source[index]);
        }
        public int Add(VertexInfo aOther, int aIndex)
        {
            int i = vertices.Count;
            Copy(ref vertices, aOther.vertices, aIndex);
            Copy(ref uv, aOther.uv, aIndex);
            Copy(ref uv2, aOther.uv2, aIndex);
            Copy(ref uv3, aOther.uv3, aIndex);
            Copy(ref uv4, aOther.uv4, aIndex);
            Copy(ref normals, aOther.normals, aIndex);
            Copy(ref tangents, aOther.tangents, aIndex);
            Copy(ref colors, aOther.colors, aIndex);
            Copy(ref boneWeights, aOther.boneWeights, aIndex);
            return i;
        }
        public void AssignTo(Mesh target)
        {
            if (vertices.Count > 65535)
                target.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            target.SetVertices(vertices);
            if (uv != null) target.SetUVs(0, uv);
            if (uv2 != null) target.SetUVs(1, uv2);
            if (uv3 != null) target.SetUVs(2, uv3);
            if (uv4 != null) target.SetUVs(3, uv4);
            if (normals != null) target.SetNormals(normals);
            if (tangents != null) target.SetTangents(tangents);
            if (colors != null) target.SetColors(colors);
            if (boneWeights != null) target.boneWeights = boneWeights.ToArray();
        }
    }

    public static Mesh CreateFromSubMesh(this Mesh mesh, int subMeshIndex)
    {
        if (subMeshIndex < 0 || subMeshIndex >= mesh.subMeshCount)
            return null;
        int[] indices = mesh.GetTriangles(subMeshIndex);
        VertexInfo source = new VertexInfo(mesh);
        VertexInfo dest = new VertexInfo();
        Dictionary<int, int> map = new Dictionary<int, int>();
        int[] newIndices = new int[indices.Length];
        for (int i = 0; i < indices.Length; i++)
        {
            int o = indices[i];
            int n;
            if (!map.TryGetValue(o, out n))
            {
                n = dest.Add(source, o);
                map.Add(o, n);
            }
            newIndices[i] = n;
        }
        Mesh m = new Mesh();
        dest.AssignTo(m);
        m.triangles = newIndices;
        return m;
    }

}
