using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class SpotLight : MonoBehaviour
	{
		[Range(3, 64)] public int geometrySides = 32;
		public int geometrySegment = 8;
		public bool geometryCap = true;
		public float startConeRadius = 0.1f;
		public float fadeRadius = 3f;
		[Range(0.1f, 180f)] public float spotAngle = 35f;
		public Material mat;
		public Texture3D noise;
		
		MeshFilter filter;
		
		void Start ()
		{
			// filter the 'mesh' using geometry
			filter = GetComponent<MeshFilter>();
			filter.mesh = MeshGenerator.GenerateCone(1f, 1f, 1f, geometrySides, geometrySegment, geometryCap);

			// render mesh shader to create the light effect
			MeshRenderer meshRender = GetComponent<MeshRenderer>();
			meshRender.material = mat;
		}
		
		void Update ()
		{
			// Cull the shader into cone shape Geometry will be changed in vertex shader

			float endConeRadius = fadeRadius * Mathf.Tan(spotAngle * Mathf.Deg2Rad * 0.5f);

			Bounds bound = new Bounds(new Vector3(0, 0, fadeRadius / 2), new Vector3(endConeRadius * 2, endConeRadius * 2, fadeRadius));
			filter.mesh.bounds = bound;
			
			// material parameters using Volumetric Shader
			float coneAngle = Mathf.Atan2(endConeRadius - startConeRadius, fadeRadius) * Mathf.Rad2Deg * 2f;
			float slopeRad = (coneAngle * Mathf.Deg2Rad) / 2f;
			mat.SetVector("_ConeSlopeCosSin", new Vector4(Mathf.Cos(slopeRad), Mathf.Sin(slopeRad), 0, 0));
			mat.SetVector("_ConeRadius", new Vector4(startConeRadius, endConeRadius, 0, 0));
			mat.SetTexture ("_NoiseTex", noise);
		}

	}

