using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public float projectileSpeed;
    public AudioClip explosionSound;

    private float rightSide;

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        transform.position += gameObject.transform.right * projectileSpeed;

        if(gameObject.transform.position.x > CameraController.rightSideOfScreen || 
            gameObject.transform.position.x < CameraController.leftSideOfScreen)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
        if (other.gameObject.CompareTag("Brick")) {
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            GameManager.audioSource.PlayOneShot(explosionSound);
        }
    }
}
