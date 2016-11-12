using UnityEngine;
using System.Collections;

public class ResetObjectPosition : MonoBehaviour {

    // Referenced from: https://forum.unity3d.com/threads/reset-rigidbody.39998/

    public InteractiveObject[] referencedObjs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown (KeyCode.R))
        {
            foreach (InteractiveObject obj in referencedObjs)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                obj.transform.position = obj.startingPosition;
                obj.transform.rotation = obj.startingRotation;

                obj.enablePhysics();

				obj.resetAbilities ();
            }
        }
	}
}
