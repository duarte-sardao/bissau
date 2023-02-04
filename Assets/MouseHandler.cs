using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{

    private UnitLogic holding;
    public PathCalculator calculator;
    private LineRenderer lineRenderer;

    public SectorController lastKnownSect;
    public Stack<Vector3> lastKnownPath;

    public void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.enabled = false;
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    }

    void Update()
    {
        DetectObjectWithRaycast();
        if(holding)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, holding.transform.position);
            lineRenderer.SetPosition(1, GetCurrentMousePosition());
        }
    }

    public void DetectObjectWithRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null &&  hit.collider.transform != this.transform)
            {
                if(hit.collider.transform.CompareTag("unit"))
                {
                    holding = hit.collider.gameObject.GetComponent<UnitLogic>();
                    lineRenderer.enabled = true;
                }
            }
        } else
        {
            if(holding != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                SectorController sect = null;
                try { sect = hit.collider.gameObject.GetComponent<SectorController>(); } catch (System.NullReferenceException) { };
                if (sect != null && sect != lastKnownSect)
                {
                    lastKnownSect = sect;
                    lastKnownPath = calculator.CalculateDistance(holding.sector, sect, true);
                    if(lastKnownPath.Count > 0)
                    {
                        lineRenderer.startColor = Color.white;
                        lineRenderer.endColor = Color.white;
                    } else
                    {
                        lineRenderer.startColor = Color.red;
                        lineRenderer.endColor = Color.red;
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(holding != null && lastKnownPath != null)
            {
                holding.Move(calculator.CalculateDistance(holding.sector, lastKnownSect, true), lastKnownSect); //might have changed
                lastKnownSect = null;
                lastKnownPath = null;
            }
            holding = null;
            lineRenderer.enabled = false;
        }
    }

    private Vector3 GetCurrentMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);

        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);

        }

        return new Vector3();
    }
}
