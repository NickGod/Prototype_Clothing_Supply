using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

  public Vector3 jump_height;
  public bool startJumping = false;
	// Use this for initialization
	void Start () {

    jump_height = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		//if (startJumping)
    //{
     // StartCoroutine(JumpAction(transform, transform.position, jump_height, 0.1f));
      //startJumping = false;
    //}
  }

  public void StartJumpAnimation()
  {
    jump_height = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

    StartCoroutine(JumpAction(transform, transform.position, jump_height, 0.4f));
  }
  IEnumerator JumpAction(Transform objectToMove, Vector3 a, Vector3 b, float speed)
  {
    yield return new WaitForSeconds(0.5f);
    yield return StartCoroutine(MoveFromTo(objectToMove, a, b, speed));
    yield return StartCoroutine(MoveFromTo(objectToMove, b, a, speed));
    yield return StartCoroutine(JumpAction(transform, a, b, 0.4f));
  }
  IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
  {
    float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
    float t = 0;
    while (t <= 1.0f)
    {
      t += step; // Goes from 0 to 1, incrementing by step each time
      objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
      yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
    }
    objectToMove.position = b;
  }
}
