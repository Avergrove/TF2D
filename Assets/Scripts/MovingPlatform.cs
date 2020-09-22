using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D rgbd;
    public Vector2[] controlPoints;

    private int state;
    private float timeStamp;
    public float transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = this.GetComponent<Rigidbody2D>();
        this.transform.position = controlPoints[0];
        this.state = 0;
        this.timeStamp = 0;

        UpdateDirection(this.state);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate current time stamp and determine state
        timeStamp += Time.deltaTime;
        if (timeStamp >= transitionTime)
        {
            this.state = (this.state + 1) % controlPoints.Length;
            timeStamp = 0;
            this.transform.position = controlPoints[state];
            UpdateDirection(state);
        }
    }

    void UpdateDirection(int state)
    {
        Vector2 displacement = (controlPoints[(state + 1) % controlPoints.Length] - controlPoints[state]);
        Vector2 nextVelocity = displacement / transitionTime;

        rgbd.velocity = nextVelocity;
    }

    // A platform parents a character if they land on one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Characters")))
        {
            collision.gameObject.GetComponent<Character>().InformAttachmentToMovingPlatform(rgbd, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Characters")))
        {
            collision.gameObject.GetComponent<Character>().InformAttachmentToMovingPlatform(null, false);
        }
    }

    // Draws lines between control points
    private void OnDrawGizmos()
    {
        for (int i = 0; i < controlPoints.Length; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(controlPoints[i], controlPoints[(i + 1) % controlPoints.Length]);
        }
    }
}
