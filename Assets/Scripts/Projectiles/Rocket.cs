using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines a basic rocket launched from a rocket launcher.
 * A rocket explodes upon impact with a body, causing explosion damage and knockback.
 */
public class Rocket : MonoBehaviour
{

    private Rigidbody2D rgbd;

    public float velocity;
    public int directDamage;
    public float knockback;
    public float effectiveExplosiveDistance;

    public GameObject explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Explode on collision

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(string.Format("Colliding with {0} of layer {1}", collision.transform.name, LayerMask.LayerToName(collision.gameObject.layer)));
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            // DIE
            GameObject generatedParticles = GameObject.Instantiate(explosionParticles);
            generatedParticles.transform.position = this.transform.position;

            // Rotate particle to correct angle.
            generatedParticles.transform.rotation = Quaternion.Euler(Vector2.Reflect(this.GetComponent<Rigidbody2D>().velocity, collision.contacts[0].normal));

            Destroy(generatedParticles.gameObject, 2f);

            Destroy(this.gameObject);
        }
    }
}
