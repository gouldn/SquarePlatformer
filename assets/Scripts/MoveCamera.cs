using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    public float moveSpeed;

    private bool shouldMove;
	
	// Update is called once per frame
	void Update () {
        if (shouldMove)
            transform.position += new Vector3(moveSpeed, 0, 0);
	}

    public void setShouldMove(bool move) {
        shouldMove = move;
    }
}
