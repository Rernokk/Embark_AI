using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour {
	[SerializeField]
	int size = 32;

	[SerializeField]
	int chunkSize = 3;

	[SerializeField]
	Material mat;

	[SerializeField]
	float magnitude = 10f;

	[SerializeField]
	Texture2D perlin, simplex, whorrley, voronoi;

	MeshFilter filter;
	Mesh myMesh;
	int squareCount = 0;

	Vector4[] tangents;
	Vector3[] vertices;
	Vector2[] uvs;
	int[] triangles;

	public TextureIndex[] fieldArray;

	void Start()
	{
		filter = GetComponent<MeshFilter>();
		myMesh = new Mesh();
		//GenerateGrid();
		for (int i = 0; i < chunkSize; i++){
			for (int j = 0; j < chunkSize; j++){
				GameObject chunk = new GameObject("Chunk");
				chunk.transform.parent = transform;
				chunk.transform.position = new Vector3(i * size, 0, j * size);
				chunk.AddComponent<MeshFilter>();
				chunk.AddComponent<MeshRenderer>();
				ChunkManager chunkManager = chunk.AddComponent<ChunkManager>();
				GenerateChildGrid(chunkManager, new Vector3(i * (1.0f / (float)chunkSize), 0, j * (1.0f / (float)chunkSize)));
			}
		}

		//myMesh.vertices = vertices;
		//myMesh.triangles = triangles;
		//myMesh.uv = uvs;
		//myMesh.tangents = tangents;
		//myMesh.RecalculateNormals();
		//myMesh.RecalculateBounds();
		//filter.sharedMesh = myMesh;
		//GetComponent<MeshRenderer>().material.SetTexture("_MainTex", perlin);
	}

	void GenerateGrid()
	{
		vertices = new Vector3[(size + 1) * (size + 1)];
		uvs = new Vector2[vertices.Length];
		tangents = new Vector4[vertices.Length];
		Vector4 tan = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= size; y++)
		{
			for (int x = 0; x <= size; x++, i++)
			{
				vertices[i] = new Vector3(x, perlin.GetPixel(Mathf.RoundToInt(perlin.width * (x / (float)size)), Mathf.RoundToInt(perlin.height * (y / (float)size))).r * magnitude, y);
				uvs[i] = new Vector2(x / (float)size, y / (float)size);
				tangents[i] = tan;
			}
		}

		triangles = new int[size * size * 6];
		for (int ti = 0, vi = 0, y = 0; y < size; y++, vi++)
		{
			for (int x = 0; x < size; x++, ti += 6, vi++)
			{
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + size + 1;
				triangles[ti + 5] = vi + size + 2;
			}
		}
	}

	void GenerateChildGrid(ChunkManager chunk, Vector3 offset){
		chunk.vertices = new Vector3[(size + 1) * (size + 1)];
		chunk.uvs = new Vector2[chunk.vertices.Length];
		chunk.tangents = new Vector4[chunk.vertices.Length];

		Vector4 tan = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= size; y++)
		{
			for (int x = 0; x <= size; x++, i++)
			{
				//chunk.vertices[i] = new Vector3(x, perlin.GetPixel(Mathf.RoundToInt(perlin.width * (x / (float)(size))), Mathf.RoundToInt(perlin.height * (y / (float)(size)))).r * magnitude, y);
				//= perlin.GetPixel(Mathf.RoundToInt(perlin.width * (((x / (float)size) / chunkSize)) + offset.x), Mathf.RoundToInt(perlin.height * (((y / (float)size) / chunkSize) + offset.z))).r;
				float tarHeight = 0;
				for (int a = -1; a <= 1; a++)
				{
					for (int b = -1; b <= 1; b++)
					{
						tarHeight += perlin.GetPixel(a + Mathf.RoundToInt(perlin.width * (((x / (float)size) / chunkSize) + offset.x)), b + Mathf.RoundToInt(perlin.height * (((y / (float)size) / chunkSize) + offset.z))).r;
					}
				}
				tarHeight /= 8.0f;
				tarHeight = Mathf.Round(tarHeight * magnitude);
				//tarHeight = perlin.GetPixel(Mathf.RoundToInt(perlin.width * (((x / (float)size) / chunkSize) + offset.x)), Mathf.RoundToInt(perlin.height * (((y / (float)size) / chunkSize) + offset.z))).r;

				//This shit works
				//tarHeight = ((x / (float) size)/chunkSize) + offset.x;
				//tarHeight = ((y / (float) size)/chunkSize) + offset.z;
				chunk.vertices[i] = new Vector3(x, tarHeight, y);
				chunk.uvs[i] = new Vector2(((x / (float)size) / chunkSize) + offset.x, ((y  / (float)size) / chunkSize) + offset.z);
				chunk.tangents[i] = tan;
			}
		}

		chunk.triangles = new int[size * size * 6];
		for (int ti = 0, vi = 0, y = 0; y < size; y++, vi++)
		{
			for (int x = 0; x < size; x++, ti += 6, vi++)
			{
				chunk.triangles[ti] = vi;
				chunk.triangles[ti + 3] = chunk.triangles[ti + 2] = vi + 1;
				chunk.triangles[ti + 4] = chunk.triangles[ti + 1] = vi + size + 1;
				chunk.triangles[ti + 5] = vi + size + 2;
			}
		}

		chunk.myMesh = new Mesh();
		chunk.myMesh.vertices = chunk.vertices;
		chunk.myMesh.triangles = chunk.triangles;
		chunk.myMesh.tangents = chunk.tangents;
		chunk.myMesh.uv = chunk.uvs;
		chunk.myMesh.RecalculateNormals();
		chunk.GetComponent<MeshRenderer>().material = mat;
		mat.mainTexture = perlin;
		chunk.GetComponent<MeshFilter>().sharedMesh = chunk.myMesh;
	}
}
