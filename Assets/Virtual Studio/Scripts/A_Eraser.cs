using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class A_Eraser : MonoBehaviour
{
    A_PaintBrush pb;
    GameObject objectHit;
    Mesh meshHit;
    bool found;
    bool eraserActive;

    //icon interaction
    public Button icon;
    Color originalColor;
    bool reverted;

    private void Start()
    {
        pb = this.transform.parent.GetComponent<A_PaintBrush>();
    }

    private void Update()
    {
        //eraser
        if (pb.controller != null)
        {
            if (pb.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && pb.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y <= -0.5f)
            {
                //Vector2 touchpad = (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
                //print("Psressing Touchpad" + touchpad.y);

                if (!eraserActive)
                {
                    this.GetComponent<Renderer>().enabled = true;
                    this.GetComponent<Collider>().enabled = true;
                    eraserActive = true;
                    pb.eraserActive = true;
                    pb.paintBrushActive = false;
                    if (pb.displayBrush != null)
                    {
                        pb.displayBrush.SetActive(false);
                    }
                }
                else
                {
                    this.GetComponent<Renderer>().enabled = false;
                    this.GetComponent<Collider>().enabled = false;
                    eraserActive = false;
                    pb.eraserActive = false;
                    pb.paintBrushActive = true;
                    if (pb.displayBrush != null)
                    {
                        pb.displayBrush.SetActive(true);
                    }
                }
            }
        }

    }

    public void DeActivateEraser()
    {
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        eraserActive = false;
        pb.paintBrushActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "paint")
        {
            //print("collided");
            if (other.transform.parent.gameObject != null)
            {
                StartCoroutine(lerp(other.transform.gameObject));
            }
        }
    }

    IEnumerator lerp(GameObject paint)
    {
        while (paint != null && paint.transform.localScale.magnitude > 0.01f)
            {
                if (paint.transform.parent != null)
                {
                    paint.transform.parent.position = Vector3.Lerp(paint.transform.parent.position, this.transform.position, Time.deltaTime * 3);
                    paint.transform.parent.localScale = Vector3.Lerp(paint.transform.parent.localScale, Vector3.zero, Time.deltaTime * 5);
                    if (paint.transform.parent.localScale.magnitude <= 0.1f)
                    {
                        paint.transform.parent.gameObject.transform.parent = GameObject.Find("RecycleBin").transform;
                        paint.transform.parent.gameObject.SetActive(false);
                        break;
                    }
                    yield return null;
                }
            }
        }    
    
}


    //script that can delete a triangle hit by a raycast
//    private void Update()
//    {
//        RaycastHit hit;

//        Vector3 fwd = transform.TransformDirection(Vector3.forward);

//        float distance = 0.04f;

//        Debug.DrawRay(transform.position, fwd * distance, Color.red);

//        //
//        if (Physics.Raycast(transform.position, fwd, out hit, distance))
//        {
//            if (hit.collider.gameObject.tag == "paint")
//            {
//                objectHit = hit.collider.gameObject;
//                meshHit = objectHit.GetComponent<MeshFilter>().mesh;
//                deleteTri(hit.triangleIndex);
//            }
//        }
      
//    }

//    void deleteTri(int index)
//    {
//        Destroy(objectHit.GetComponent<MeshCollider>());
//        int[] oldTriangles = meshHit.triangles;
//        int[] newTriangles = new int[meshHit.triangles.Length - 3];

//        int i = 0;
//        int j = 0;

//        while (j < meshHit.triangles.Length)
//        {
//            if (j != index * 3)
//            {
//                newTriangles[i++] = oldTriangles[j++];
//                newTriangles[i++] = oldTriangles[j++];
//                newTriangles[i++] = oldTriangles[j++];
//            }
//            else
//            {
//                j += 3;
//            }
//        }
//        meshHit.triangles = newTriangles;
//        objectHit.AddComponent<MeshCollider>();
//    }
//}

    //// old script that can delete a square by figuring out the triangle hit and the adjacent one
    //void Update()
    //{
    //    RaycastHit hit1;
    //    RaycastHit hit2;
    //    Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    Vector3 back = transform.TransformDirection(Vector3.down);

    //    float distance = 0.04f;

    //    Debug.DrawRay(transform.position, fwd * distance, Color.red);
    //    Debug.DrawRay(transform.position, back * distance, Color.red);

    //    //
    //    if (Physics.Raycast(transform.position, fwd, out hit1, distance))
    //    {
    //        if (hit1.collider.gameObject.tag == "paint")
    //        {
    //            print(hit1.collider.gameObject.name);
    //            objectHit = hit1.collider.gameObject;

    //            int hitTri = hit1.triangleIndex;

    //            Mesh mesh = objectHit.GetComponent<MeshFilter>().mesh;

    //            //get neighbour
    //            int[] triangles = mesh.triangles;
    //            Vector3[] vertices = mesh.vertices;
    //            Vector3 p0 = vertices[triangles[hitTri * 3 + 0]];
    //            Vector3 p1 = vertices[triangles[hitTri * 3 + 1]];
    //            Vector3 p2 = vertices[triangles[hitTri * 3 + 2]];

    //            float edge1 = Vector3.Distance(p0, p1);
    //            float edge2 = Vector3.Distance(p0, p2);
    //            float edge3 = Vector3.Distance(p1, p2);

    //            Vector3 shared1;
    //            Vector3 shared2;

    //            if (edge1 > edge2 && edge1 > edge3)
    //            {
    //                shared1 = p0;
    //                shared2 = p1;
    //            }
    //            else if (edge2 > edge1 && edge2 > edge3)
    //            {
    //                shared1 = p0;
    //                shared2 = p2;
    //            }
    //            else
    //            {
    //                shared1 = p1;
    //                shared2 = p2;
    //            }

    //            int v1 = findVertex(shared1);
    //            int v2 = findVertex(shared2);

    //            deleteSquare(hitTri, findTriangle(vertices[v1], vertices[v2], hitTri));
    //        }
    //    }
    //}

    ////raycasting
    //void Erase()
    //{

    //}

    ////vertex
    //int findVertex(Vector3 v)
    //{
    //    Vector3[] vertices = objectHit.GetComponent<MeshFilter>().mesh.vertices;
    //    for(int i = 0; i < vertices.Length; i++)
    //    {
    //        if (vertices[i] == v)
    //            return i;
    //    }
    //    return -1;
    //}

    ////triangle
    //int findTriangle (Vector3 v1, Vector3 v2, int notTriIndex)
    //{
    //    int [] triangles = objectHit.GetComponent<MeshFilter>().mesh.triangles;
    //    Vector3[] vertices = objectHit.GetComponent<MeshFilter>().mesh.vertices;
    //    int i = 0;
    //    int j = 0;
    //    int found = 0;
    //    while (j < triangles.Length)
    //    {
    //        if(j/3 != notTriIndex)
    //        {
    //            if (vertices[triangles[j]] == v1 && (vertices[triangles[j+1]] == v2 || vertices[triangles[j+2]] == v2))
    //                return j / 3;
    //            else if (vertices[triangles[j]] == v2 && (vertices[triangles[j + 1]] == v1 || vertices[triangles[j + 2]] == v1))
    //                return j / 3;
    //            else if (vertices[triangles[j+1]] == v2 && (vertices[triangles[j]] == v1 || vertices[triangles[j + 2]] == v1))
    //                return j / 3;
    //            else if (vertices[triangles[j+1]] == v1 && (vertices[triangles[j]] == v2 || vertices[triangles[j + 2]] == v2))
    //                return j / 3;
    //        }

    //        j += 3;
    //    }
    //    return -1;
    //}

    ////square calculations
    //void deleteSquare(int index1, int index2)
    //{
    //    Destroy(objectHit.GetComponent<MeshCollider>());
    //    Mesh mesh = objectHit.GetComponent<MeshFilter>().mesh;
    //    int[] oldTriangles = mesh.triangles;
    //    int[] newTriangles = new int[mesh.triangles.Length - 6];

    //    int i = 0;
    //    int j = 0;
    //    while (j < mesh.triangles.Length)
    //    {
    //        if(j != index1*3 && j != index2*3)
    //        {
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //        }
    //        else
    //        {
    //            j += 3;
    //        }
    //    }
    //    objectHit.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
    //    objectHit.AddComponent<MeshCollider>();
    //}


