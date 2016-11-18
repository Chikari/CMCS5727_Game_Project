using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class OpenDoor : MonoBehaviour {
	
    // Use this for initialization
    void Start () {
		
	}

	void OnTriggerEnter(Collider collision){
		//gameObject.GetComponent<Animation>().Play("open");
		Animation doorAnim = gameObject.transform.parent.Find("door").GetComponent<Animation>();
		if(doorAnim.isPlaying == false){
			//Do reaction
			doorAnim.Play("open");
		}
	}

	void OnTriggerExit(Collider collision){
		//gameObject.GetComponent<Animation>().Play("open");
		/*
		Animation doorAnim = gameObject.transform.parent.Find("door").GetComponent<Animation>();
		if(doorAnim.isPlaying == false){
			//Do reaction
			doorAnim.Play("close");
		}*/
	}

    void Update()
    {
		
    }
}
