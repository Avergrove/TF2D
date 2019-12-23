using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Configures a camera to automatically follow an object.
 */
public class CameraController : MonoBehaviour
{
    public Transform followingTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(followingTransform.position.x, followingTransform.position.y, this.transform.position.z);
    }
}
