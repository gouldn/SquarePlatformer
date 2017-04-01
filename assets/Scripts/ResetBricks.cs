using UnityEngine;
using System.Collections;

public class ResetBricks : MonoBehaviour {

    public bool colliderEnabled, renderEnabled;

    private SpriteRenderer sprite;
    private BoxCollider2D collision;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {

        colliderEnabled = sprite.enabled;
        renderEnabled = collision.enabled;

        if ((!GameManager.gameStarted) && (!colliderEnabled && !renderEnabled))
        {
            sprite.enabled = true;
            collision.enabled = true;
        }
	}
}
