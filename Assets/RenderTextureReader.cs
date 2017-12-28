using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class RenderTextureReader : MonoBehaviour {

	GameObject testQuad;
	RenderTexture rt;
	bool recorded;
	// Use this for initialization
	void Start () {
		testQuad = GameObject.Find ("2dQuad");
		Texture text = GetComponent<Renderer> ().material.mainTexture;
		rt = text as RenderTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if (!recorded && Time.frameCount > 5) {
			GetTexture ();
			recorded = true;
		}
	}
	//Questions:
	//	How to get full verticle slit?

	void Render() {
		//Rotate camera
		//	Grab slit
		//	Write slit
	}

	void GetTexture() {
		RenderTexture currentActiveRT = RenderTexture.active;
		// Set the supplied RenderTexture as the active one
		RenderTexture.active = rt;

		// Create a new Texture2D and read the RenderTexture image into it
		Texture2D tex = new Texture2D(rt.width, rt.height);
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		tex.Apply ();

		// Restorie previously active render texture
		RenderTexture.active = currentActiveRT;

		testQuad.GetComponent<Renderer> ().material.SetTexture ("_MainTex", tex);
		Byte[] bytes = tex.EncodeToPNG ();
		FileStream file = File.Open(Application.dataPath + "/texture.png",FileMode.Create);
		BinaryWriter writer = new BinaryWriter (file);
		writer.Write (bytes);
		file.Close ();
		RenderTexture.active = currentActiveRT;
	}
}
