using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class RenderTextureReader : MonoBehaviour {

	public RenderTexture leftRenderTexture;
	public RenderTexture rightRenderTexture;
	public int sliceWidth;
	int outImageWidth;
	int outImageHeight;
	double rotationValue;
	Texture2D outTexture;
	bool recorded;
	// Use this for initialization
	void Start () {
		outImageWidth = 4096;
		outImageHeight = 4096;
		rotationValue = Math.PI * 2 * ((double)sliceWidth) / ((double)outImageWidth);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("c")) {
			Capture ();
		}
		if (Input.GetKeyDown ("w")) {
			WriteImage ();
		}
	}
	//Questions:
	//	How to get full verticle slit?

	void Capture() {
		outTexture = new Texture2D (outImageWidth, outImageHeight);
		StartCoroutine ("CaptureRotateLoop");
	}

	IEnumerator CaptureRotateLoop() {
		for(double theta = -1 * Math.PI; theta < Math.PI; theta += rotationValue) {
			GrabSlit(theta, leftRenderTexture, false);
			GrabSlit(theta, rightRenderTexture, true);
			yield return null;
			RotateVRHolder ();
			yield return null;
		}

		WriteImage ();
	}

	void WriteImage() {
		outTexture.Apply ();
		Byte[] bytes = outTexture.EncodeToPNG ();
		FileStream file = File.Open(Application.dataPath + "/texture.png",FileMode.Create);
		BinaryWriter writer = new BinaryWriter (file);
		writer.Write (bytes);
		file.Close ();
	}

	void GrabSlit(double theta, RenderTexture rt, bool bottom) {
		RenderTexture currentActiveRT = RenderTexture.active;
		// Set the supplied RenderTexture as the active one
		RenderTexture.active = leftRenderTexture;

		int widthPosition = thetaToPosition(theta);
		Rect rect = new Rect (rt.width / 2 - (sliceWidth / 2), 0, sliceWidth, rt.height);
		if (bottom) {
			outTexture.ReadPixels (rect, widthPosition, rt.height);
		} else {
			outTexture.ReadPixels (rect, widthPosition, 0);
		}

		// Restorie previously active render texture
		RenderTexture.active = currentActiveRT;
	}

	int thetaToPosition(double theta) {
		return (int)(((theta + Math.PI) / (2f * Math.PI)) * outImageWidth);
	}

	void RotateVRHolder() {
		transform.Rotate (0, ((float)rotationValue) * 360f / ((float)Math.PI * 2f), 0);
	}
}
