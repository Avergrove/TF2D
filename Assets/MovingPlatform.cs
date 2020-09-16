using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D childRgbd;
    public Vector2[] controlPoints;
    private int state;
    private float timeStamp;
    public float transitionTime;


    // Start is called before the first frame update
    void Start()
    {
        childRgbd = this.GetComponent<Rigidbody2D>();
        this.transform.position = controlPoints[0];
        this.state = 0;
        this.timeStamp = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate current time stamp and determine state
        timeStamp += Time.deltaTime;
        if(timeStamp >= transitionTime)
        {
            state = (state + 1) % controlPoints.Length;
            timeStamp = 0;
        }

        // Then determine at what position between the two control points we are at.
        childRgbd.MovePosition(Vector2.Lerp(controlPoints[state], controlPoints[(state + 1) % controlPoints.Length], timeStamp / transitionTime));
    }

    // A platform parents a character if they land on one
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Characters")))
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Characters")))
        {
            collision.gameObject.transform.SetParent(null);
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
