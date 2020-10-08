using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public static class MeshGenerator
	{
		public static Mesh GenerateCone(float lengthZ, float radiusStart, float radiusEnd, int numSides, int numSegments, bool cap)
		{
			Debug.Assert(lengthZ > 0f);
			Debug.Assert(radiusStart >= 0f);
			Debug.Assert(numSides >= 3);
			Debug.Assert(numSegments >= 0);

			Mesh mesh = new Mesh();
			mesh.name = "ProceduralCone";
			mesh.hideFlags = HideFlags.DontSave;

			bool genCap = cap && radiusStart > 0f;

			radiusStart = Mathf.Max(radiusStart, 0.001f);   // small start, non-zero

			int vertCountSides = numSides * (numSegments + 2);
			int vertCountTotal = vertCountSides;

			if (genCap)
				vertCountTotal += numSides + 1;


			{
				Vector3[] vertices = new Vector3[vertCountTotal];
				for (int i = 0; i < numSides; i++)
				{
					float angle = 2 * Mathf.PI * i / numSides;
					float angleCos = Mathf.Cos(angle);
					float angleSin = Mathf.Sin(angle);

					for (int seg = 0; seg < numSegments + 2; seg++)
					{
						float tseg = (float)seg / (numSegments + 1);
						Debug.Assert(tseg >= 0f && tseg <= 1f);
						float radius = Mathf.Lerp(radiusStart, radiusEnd, tseg);
						vertices[i + seg * numSides] = new Vector3(radius * angleCos, radius * angleSin, tseg * lengthZ);
					}
				}

				if (genCap)
				{
					int ind = vertCountSides;
					vertices[ind] = Vector3.zero;
					ind++;
					for (int i = 0; i < numSides; i++)
					{
						float angle = 2 * Mathf.PI * i / numSides;
						float angleCos = Mathf.Cos(angle);
						float angleSin = Mathf.Sin(angle);
						vertices[ind] = new Vector3(radiusStart * angleCos, radiusStart * angleSin, 0f);
						ind++;
					}
					Debug.Assert(ind == vertices.Length);
				}
				mesh.vertices = vertices;
			}


			{
				var uv = new Vector2[vertCountTotal];
				int ind = 0;
				for (int i = 0; i < vertCountSides; i++)
					uv[ind++] = Vector2.zero;

				if (genCap)
				{
					for (int i = 0; i < numSides + 1; i++)
						uv[ind++] = Vector2.one;
				}
				Debug.Assert(ind == uv.Length);
				mesh.uv = uv;
			}


			{
				int triCountSides = numSides * 2 * Mathf.Max(numSegments + 1, 1);
				int indCountTotal = triCountSides * 3;

				if (genCap)
					indCountTotal += numSides * 3;

				int[] indices = new int[indCountTotal];
				int ind = 0;
				for (int i = 0; i < numSides; i++)
				{
					int ip1 = i + 1;
					if (ip1 == numSides)
						ip1 = 0;

					for (int k = 0; k < numSegments + 1; ++k)
					{
						int offset = k * numSides;

						indices[ind++] = offset + i;
						indices[ind++] = offset + ip1;
						indices[ind++] = offset + i + numSides;

						indices[ind++] = offset + ip1 + numSides;
						indices[ind++] = offset + i + numSides;
						indices[ind++] = offset + ip1;
					}
				}

				if (genCap)
				{
					for (int i = 0; i < numSides - 1; i++)
					{
						indices[ind++] = vertCountSides;
						indices[ind++] = vertCountSides + i + 1;
						indices[ind++] = vertCountSides + i + 2;
					}
					indices[ind++] = vertCountSides;
					indices[ind++] = vertCountSides + numSides;
					indices[ind++] = vertCountSides + 1;
				}
				Debug.Assert(ind == indices.Length);
				mesh.triangles = indices;
			}
			return mesh;
		}
	}

