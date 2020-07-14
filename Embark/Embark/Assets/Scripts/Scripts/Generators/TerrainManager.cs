using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
	[SerializeField]
	Texture2D noiseTexture;

	[SerializeField]
	float magnitude = 10f;

	int squareCount = 0;
	List<int> triangles;
	List<Vector3> vertices;
	List<Vector2> uvs;

	MeshFilter meshFilter;
	Mesh terrainMesh;

	int[,] noiseMap;

	void Start()
	{
		triangles = new List<int>();
		vertices = new List<Vector3>();
		uvs = new List<Vector2>();
		terrainMesh = new Mesh();
		meshFilter = GetComponent<MeshFilter>();

		noiseMap = new int[noiseTexture.width, noiseTexture.height];
		for (int i = 0; i < noiseTexture.width; i++)
		{
			for (int j = 0; j < noiseTexture.height; j++)
			{
				noiseMap[i, j] = (int)(noiseTexture.GetPixel(i, j).r * magnitude);
			}
		}

		for (int i = 0; i < noiseTexture.width; i++)
		{
			for (int j = 0; j < noiseTexture.height; j++)
			{
				Vector3 pos = new Vector3(i, noiseMap[i, j], j);
				GenerateCube(pos, i == 0 || j == 0 || j == noiseTexture.height - 1 || i == noiseTexture.width - 1);

			}
		}

		terrainMesh.vertices = vertices.ToArray();
		terrainMesh.triangles = triangles.ToArray();
		terrainMesh.uv = uvs.ToArray();
		meshFilter.sharedMesh = terrainMesh;
	}

	void GenerateTopSquare(Vector3 position)
	{
		vertices.Add(position + new Vector3(-.5f, .5f, -.5f));
		vertices.Add(position + new Vector3(-.5f, .5f, .5f));
		vertices.Add(position + new Vector3(.5f, .5f, .5f));
		vertices.Add(position + new Vector3(.5f, .5f, -.5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}
	void GenerateBottomSquare(Vector3 position)
	{
		vertices.Add(position + new Vector3(.5f, -.5f, .5f));
		vertices.Add(position + new Vector3(-.5f, -.5f, .5f));
		vertices.Add(position + new Vector3(-.5f, -.5f, -.5f));
		vertices.Add(position + new Vector3(.5f, -.5f, -.5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}
	void GenerateLeftSquare(Vector3 position)
	{

		vertices.Add(position + new Vector3(-.5f, -.5f, .5f));
		vertices.Add(position + new Vector3(-.5f, .5f, .5f));
		vertices.Add(position + new Vector3(-.5f, .5f, -.5f));
		vertices.Add(position + new Vector3(-.5f, -.5f, -.5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}
	void GenerateRightSquare(Vector3 position)
	{
		vertices.Add(position + new Vector3(.5f, .5f, -.5f));
		vertices.Add(position + new Vector3(.5f, .5f, .5f));
		vertices.Add(position + new Vector3(.5f, -.5f, .5f));
		vertices.Add(position + new Vector3(.5f, -.5f, -.5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}
	void GenerateBackSquare(Vector3 position)
	{
		vertices.Add(position + new Vector3(-.5f, .5f, -.5f));
		vertices.Add(position + new Vector3(.5f, .5f, -.5f));
		vertices.Add(position + new Vector3(.5f, -.5f, -.5f));
		vertices.Add(position + new Vector3(-.5f, -.5f, -.5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}
	void GenerateForwardSquare(Vector3 position)
	{
		vertices.Add(position + new Vector3(.5f, -.5f, .5f));
		vertices.Add(position + new Vector3(.5f, .5f, .5f));
		vertices.Add(position + new Vector3(-.5f, .5f, .5f));
		vertices.Add(position + new Vector3(-.5f, -.5f, .5f));

		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 1);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4);
		triangles.Add(squareCount * 4 + 2);
		triangles.Add(squareCount * 4 + 3);

		uvs.Add(new Vector2(0, 0));
		uvs.Add(new Vector2(0, 1));
		uvs.Add(new Vector2(1, 1));
		uvs.Add(new Vector2(1, 0));

		squareCount++;
	}

	void GenerateCube(Vector3 position, bool edge = false)
	{
		GenerateTopSquare(position);
		GenerateBottomSquare(position);
		GenerateLeftSquare(position);
		GenerateRightSquare(position);
		GenerateForwardSquare(position);
		GenerateBackSquare(position);
		if (!edge)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if ((i == 0 || j == 0) && (i + position.x >= 0 && j + position.z >= 0 && i + position.x < noiseTexture.width && j + position.z < noiseTexture.height))
					{
						if (noiseMap[(int)(i + position.x), (int)(j + position.z)] - position.y < -1)
						{
							GenerateCube(position - new Vector3(0, 1, 0));
						}
					}
				}
			}
		} else if (position.y > 0){
			GenerateCube(position - new Vector3(0, 1, 0), edge);
		}
	}
}
