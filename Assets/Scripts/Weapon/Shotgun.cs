using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public GameObject tracer;
    public GameObject collisionParticle;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnFirePressed()
    {
        // Fire a hitscan raycast
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorInWorldPos - (Vector2)transform.parent.position;

        string[] layers = { "Platforms", "Players" };
        LayerMask mask = LayerMask.GetMask(layers);
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, direction, float.MaxValue, mask);

        // Create a bullet trail to indicate shot
        GameObject generatedTracer = GameObject.Instantiate(tracer);
        generatedTracer.transform.localPosition = Vector2.zero;

        LineRenderer tracerLineRenderer = generatedTracer.GetComponent<LineRenderer>();
        Vector3[] linePositions = new Vector3[2] { transform.position, raycastHit.point };
        tracerLineRenderer.SetPositions(linePositions);

        // Instantiate particle on coollided position
        GameObject shotParticle = GameObject.Instantiate(collisionParticle);
        shotParticle.transform.position = raycastHit.point;
        shotParticle.transform.rotation = Quaternion.Euler(raycastHit.normal);
    }

    public override void OnFireHeld()
    {
    }

    public override void OnFireReleased()
    {
    }
}
