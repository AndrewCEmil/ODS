using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
			Record ();
			recorded = true;
		}
	}

	void Record() {
		RenderTexture currentActiveRT = RenderTexture.active;
		// Set the supplied RenderTexture as the active one
		RenderTexture.active = rt;

		// Create a new Texture2D and read the RenderTexture image into it
		Texture2D tex = new Texture2D(rt.width, rt.height);
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

		// Restorie previously active render texture
		RenderTexture.active = currentActiveRT;

		Color[] colors = tex.GetPixels ();
		for (int i = 0; i < colors.Length; i++) {
			Debug.Log ("i: " + i.ToString () + ", color: " + colors [i].ToString ());
		}

		for (int i = 0; i < rt.width; i++) {
			for (int j = 0; j < rt.height; j++) {
				//tex.SetPixel (i, j, Color.red);
			}
		}
		tex.Apply ();



		testQuad.GetComponent<Renderer> ().material.SetTexture ("_MainTex", tex);
		RenderTexture.active = currentActiveRT;
	}
}
