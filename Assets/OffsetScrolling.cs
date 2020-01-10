using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour
{
    public float scrollSpeed;

    private Renderer renderer;

    private Vector2 initialPosition;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        initialPosition = this.transform.position;
    }

    void Update()
    {
        // Parallax scrolling should be scaled to initial position.
        Vector2 diff = (Vector2) this.transform.position - initialPosition;
        float x = Mathf.Repeat(diff.x * scrollSpeed, 1);
        float y = Mathf.Repeat(diff.y * scrollSpeed, 1);

        Vector2 offset = new Vector2(x, y);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}