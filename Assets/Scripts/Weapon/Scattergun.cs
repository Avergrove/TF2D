using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A Scattergun: Deals damage proportional to distance of colliding object.
 * 
 */
public class Scattergun : Weapon
{
    public GameObject tracer;
    public GameObject collisionParticle;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnFirePressed(Vector2 direction)
    {
        string[] layers = { "Platforms", "Players" };
        LayerMask mask = LayerMask.GetMask(layers);
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, direction, float.MaxValue, mask);

        // Create a bullet trail to indicate shot
        GameObject generatedTracer = GameObject.Instantiate(tracer);
        generatedTracer.transform.localPosition = Vector2.zero;

        LineRenderer tracerLineRenderer = generatedTracer.GetComponent<LineRenderer>();
        Vector3[] linePositions = new Vector3[2] { transform.position, raycastHit.point};
        tracerLineRenderer.SetPositions(linePositions);

        // Instantiate particle on coollided position
        GameObject shotParticle = GameObject.Instantiate(collisionParticle);
        shotParticle.transform.position = raycastHit.point;
    }

    public override void OnFireHeld()
    {
    }

    public override void OnFireReleased()
    {
    }
}
