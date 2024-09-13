using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ProjectToPlane : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;
    public Transform floorPlane; // Assuming the floor plane is a flat object with a normal pointing up

    private GameObject preMarker;
    private LineRenderer preTrace;
    private List<Vector3> preMarkerPositions = new List<Vector3>();
    private GameObject postMarker;
    private LineRenderer postTrace;
    private List<Vector3> postMarkerPositions = new List<Vector3>();
    public bool plotMarker = false;
    public Gui_Controller gui_controller;

    void Start()
    {
        // Create a simple marker (e.g., a small sphere) in code
        preMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        preMarker.transform.localScale = new Vector3(2f, 0.1f, 2f); // Adjust the size of the marker
        // Create a new material with an unlit shader
        Material unlitMaterial = new Material(Shader.Find("Unlit/Color"));
        unlitMaterial.color = Color.red; // Set the color of the marker

        // Apply the unlit material to the marker
        preMarker.GetComponent<Renderer>().material = unlitMaterial;

        // Create a new GameObject for preTrace
        GameObject preTraceObject = new GameObject("PreTrace");
        preTraceObject.transform.parent = this.transform;
        preTrace = preTraceObject.AddComponent<LineRenderer>();
        preTrace.startWidth = 0.5f;
        preTrace.endWidth = 0.5f;
        preTrace.material = new Material(Shader.Find("Sprites/Default"));
        preTrace.startColor = Color.blue;
        preTrace.endColor = Color.blue;
        preTrace.positionCount = 0;

        int preMarkerLayer = LayerMask.NameToLayer("PreMarker");
        preMarker.layer = preMarkerLayer;
        preTrace.gameObject.layer = preMarkerLayer;

        // --- //
        // Create a simple marker (e.g., a small sphere) in code
        postMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        postMarker.transform.localScale = new Vector3(2f, 0.1f, 2f); // Adjust the size of the marker
        // Create a new material with an unlit shader
        unlitMaterial.color = Color.red; // Set the color of the marker

        // Apply the unlit material to the marker
        postMarker.GetComponent<Renderer>().material = unlitMaterial;

        // Create a new GameObject for postTrace
        GameObject postTraceObject = new GameObject("PostTrace");
        postTraceObject.transform.parent = this.transform;
        postTrace = postTraceObject.AddComponent<LineRenderer>();
        postTrace.startWidth = 0.5f;
        postTrace.endWidth = 0.5f;
        postTrace.material = new Material(Shader.Find("Sprites/Default"));
        postTrace.startColor = Color.green;
        postTrace.endColor = Color.green;
        postTrace.positionCount = 0;

        int postMarkerLayer = LayerMask.NameToLayer("PostMarker");
        postMarker.layer = postMarkerLayer;
        postTrace.gameObject.layer = postMarkerLayer;
    }

    public Vector3 GetProjectedMidPoint()
    {
        // Get the midpoint between objectA and objectB
        Vector3 midPoint = (objectA.position + objectB.position) / 2;

        // Project the midpoint onto the floor plane
        return Vector3.ProjectOnPlane(midPoint, floorPlane.up);
    }

    public void clearTraces()
    {
        clearPreTrace();
        clearPostTrace();
        
    }

    public void clearPreTrace()
    {
        preMarkerPositions.Clear();
        //preTrace.SetPositions(preMarkerPositions.ToArray());
        preTrace.positionCount = 0;
    }

    public void clearPostTrace()
    {
        postMarkerPositions.Clear();
        //postTrace.SetPositions(postMarkerPositions.ToArray());
        postTrace.positionCount = 0;
      
    }

    void LateUpdate()
    {
        if (preMarker != null && plotMarker)
        {
            Vector3 projectedMidPoint = GetProjectedMidPoint();
            if (gui_controller.preAlign)
            {
                // Move the marker to the projected midpoint position
                preMarker.transform.position = projectedMidPoint;

                // Record this position if it is not already stored
                if (preMarkerPositions.Count == 0 || preMarkerPositions[preMarkerPositions.Count - 1] != projectedMidPoint)
                {
                    preMarkerPositions.Add(projectedMidPoint);
                }

                // Update the preTrace with the new path points
                preTrace.positionCount = preMarkerPositions.Count;
                preTrace.SetPositions(preMarkerPositions.ToArray());
            }
            else
            {
                // Move the marker to the projected midpoint position
                postMarker.transform.position = projectedMidPoint;

                // Record this position if it is not already stored
                if (postMarkerPositions.Count == 0 || postMarkerPositions[postMarkerPositions.Count - 1] != projectedMidPoint)
                {
                    postMarkerPositions.Add(projectedMidPoint);
                }

                // Update the postTrace with the new path points
                postTrace.positionCount = postMarkerPositions.Count;
                postTrace.SetPositions(postMarkerPositions.ToArray());
            }
        }
    }
}
