using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
	public MeshFilter myFilter;
	public Mesh myMesh;
	public MeshRenderer myRenderer;

	public Vector4[] tangents;
	public Vector3[] vertices;
	public Vector2[] uvs;
	public int[] triangles;
}
