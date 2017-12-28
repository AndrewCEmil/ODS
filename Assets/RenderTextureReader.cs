using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderTextureReader : MonoBehaviour {

	RenderTexture rt;
	bool recorded;
	// Use this for initialization
	void Start () {
		Texture text = GetComponent<Renderer> ().material.mainTexture;
		rt = text as RenderTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if (!recorded) {
			Record ();
			recorded = true;
		}
	}

	void Record() {
		Texture2D tex2d = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);

		RenderTexture.active = rt;
		tex2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		tex2d.Apply();

		Color[] colors = tex2d.GetPixels ();
		Debug.Log (colors.ToString ());
	}
}
