using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

  public Vector3 jump_height;
	// Use this for initialization
	void Start () {

    jump_height = new Vector3(transform.localPosition.x, transform.localPosition.y+1, transform.localPosition.z);
    Vector3 local_position = transform.localPosition;
    StartCoroutine(JumpAction(transform, local_position, jump_height, 0.1f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  IEnumerator JumpAction(Transform objectToMove, Vector3 a, Vector3 b, float speed)
  {
    yield return new WaitForSeconds(1.0f);
    yield return StartCoroutine(MoveFromTo(objectToMove, a, b, speed));
    yield return StartCoroutine(MoveFromTo(objectToMove, b, a, speed));
    yield return StartCoroutine(JumpAction(transform, a, b, 0.1f));
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
