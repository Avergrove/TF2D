using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class CollectiblePickup : MonoBehaviour
{

    [SerializeField]
    int pickupId;

    AudioSource audioSource;
    public UnityEvent onPickupCollectible;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Players"))
        {
            // Play a pickup sound!
            audioSource.Play();
            onPickupCollectible.Invoke();
        }
    }
}
