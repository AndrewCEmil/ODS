using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class RenderTextureReader : MonoBehaviour {

	int outImageWidth;
	int outImageHeight;
	int sliceWidth;
	double rotationValue;
	GameObject vrHolder;
	GameObject testQuad;
	RenderTexture rt;
	Texture2D outTexture;
	bool recorded;
	// Use this for initialization
	void Start () {
		outImageWidth = 4096;
		outImageHeight = 2048;
		sliceWidth = 10;
		rotationValue = Math.PI * 2 * ((double)sliceWidth) / ((double)outImageWidth);
		vrHolder = GameObject.Find ("VR");
		testQuad = GameObject.Find ("2dQuad");
		rt = GetComponent<Renderer> ().material.mainTexture as RenderTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if (!recorded && Time.frameCount > 5) {
			Render ();
			recorded = true;
		}
	}
	//Questions:
	//	How to get full verticle slit?

	void Render() {
		outTexture = new Texture2D (outImageWidth, outImageHeight);

		//DEBUG
		for (int i = 0; i < outImageWidth; i++) {
			for (int j = 0; j < outImageHeight; j++) {
				outTexture.SetPixel (i, j, Color.white);
			}
		}
		outTexture.Apply ();
		testQuad.GetComponent<Renderer> ().material.SetTexture ("_MainTex", outTexture);
		//END DEBUG

		StartCoroutine ("RenderLoop");
	}

	IEnumerator RenderLoop() {
		for(double theta = -1 * Math.PI; theta < Math.PI; theta += rotationValue) {
			GrabSlit(theta);
			yield return null;
			RotateVRHolder ();
			yield return null;
		}

		WriteImage ();
	}

	void WriteImage() {
		Byte[] bytes = outTexture.EncodeToPNG ();
		FileStream file = File.Open(Application.dataPath + "/texture.png",FileMode.Create);
		BinaryWriter writer = new BinaryWriter (file);
		writer.Write (bytes);
		file.Close ();
	}

	void GrabSlit(double theta) {
		RenderTexture currentActiveRT = RenderTexture.active;
		// Set the supplied RenderTexture as the active one
		RenderTexture.active = rt;

		// Create a new Texture2D and read the RenderTexture image into it
		int widthPosition = thetaToPosition(theta);
		Rect rect = new Rect (rt.width / 2 - (sliceWidth / 2), 0, sliceWidth, rt.height);
		outTexture.ReadPixels(rect, widthPosition, 0);
		outTexture.Apply ();

		// Restorie previously active render texture
		RenderTexture.active = currentActiveRT;
	}

	int thetaToPosition(double theta) {
		return (int)(((theta + Math.PI) / (2f * Math.PI)) * outImageWidth);
	}

	void RotateVRHolder() {
		vrHolder.transform.Rotate (0, ((float)rotationValue) * 360f / ((float)Math.PI * 2f), 0);
	}
}
