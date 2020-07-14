using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TextureIndex
{
	public Vector3 point;
	public Vector2 uv;
}

public class ComputeShaderScript : MonoBehaviour {
	[SerializeField]
	ComputeShader myShader;

	[SerializeField]
	Texture2D tex;

	MeshRenderer rend;

	TextureIndex[] data;

	void Start () {
		int lim = 32;
		data = new TextureIndex[lim * lim];
		int kernel = myShader.FindKernel("CSMain");
		ComputeBuffer buff = new ComputeBuffer(lim * lim, 4);
		for (int i = 0; i < lim; i++){
			for (int j = 0; j < lim; j++)
			{
				data[j + lim * i].point = new Vector3(j, 0.0f, i);
				data[j + lim * i].uv = new Vector2(j / (float) lim, i /(float) lim);
			}
		}

		buff.SetData(data);
		myShader.SetTexture(kernel, "Tex", tex);
		myShader.SetBuffer(kernel, "Points", buff);
		myShader.Dispatch(kernel, (lim * lim) / 32, (lim * lim) / 32, 1);
		buff.GetData(data);

		GameObject.Find("BiomeManager").GetComponent<BiomeManager>().fieldArray = data;
		//GameObject.Find("BiomeManager").GetComponent<BiomeManager>().MapStart();
	}

	void Update () {
		
	}
}
