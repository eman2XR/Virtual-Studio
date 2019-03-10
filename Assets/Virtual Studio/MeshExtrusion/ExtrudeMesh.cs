// Generates an extrusion trail from the attached mesh
// Uses the MeshExtrusion algorithm in MeshExtrusion.cs;
// original script from Unity essentials in Js translated to c# by Emanuel( @ https://VirtualStudio.insilico.uk)
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtrudeMesh : MonoBehaviour {

    public Transform target;
    public float time = 0.2f;
    public float minDistance = 0.1f;
    public bool buildCaps;
    private Mesh srcMesh;
    bool invertFaces = false;
    bool autoCalculateOrientation = false;

    private MeshExtrusion.Edge[] precomputedEdges;

    Vector3 direction;
    Quaternion rotation;
    Quaternion previousRotation;

    List<ExtrudedTrailSection> sections = new List<ExtrudedTrailSection>();

     class ExtrudedTrailSection
    {
        public Vector3 point;
        public Matrix4x4 matrix;
        public float time;
    }
    

    void Start()
    {
        srcMesh = GetComponent<MeshFilter>().sharedMesh;
        precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
    }

    void LateUpdate()
    {
            Vector3 position = target.transform.position;
            float now = Time.time;

           // Remove old sections
                    while (sections.Count > 0 && now > sections[sections.Count - 1].time + time)
            {
                sections.RemoveAt(sections.Count - 1);
            }

        // Add a new trail section to beginning of array 
        if (sections.Count == 0 || (sections[0].point - position).sqrMagnitude > minDistance * minDistance)
            {
                ExtrudedTrailSection section = new ExtrudedTrailSection();
                section.point = position;
                section.matrix = target.transform.localToWorldMatrix;
                section.time = now;
                sections.Insert(0, section);
               // print(sections.Count); 
            }

            // We need at least 2 sections to create the line
            if (sections.Count < 2)
                return;

            Matrix4x4 worldToLocal = transform.worldToLocalMatrix;
            Matrix4x4[] finalSections = new Matrix4x4[sections.Count];

        for (int i = 0; i < sections.Count; i++)
        {
                    finalSections[i] = worldToLocal * sections[i].matrix;
        }
            
        // Rebuild the extrusion mesh	
        MeshExtrusion.ExtrudeMesh(srcMesh, GetComponent<MeshFilter>().mesh, finalSections, precomputedEdges, invertFaces, buildCaps);
        GetComponent<MeshFilter>().mesh.RecalculateTangents();
    }

}