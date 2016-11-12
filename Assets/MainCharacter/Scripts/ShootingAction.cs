using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingAction : MonoBehaviour
{
	
    // Pickup ability referenced from: http://answers.unity3d.com/questions/822261/how-to-make-an-object-your-carrying-with-your-grav.html

	// Charachter and gun currently-stored abilities
    private GameObject currentCharacter;
	private Dictionary<string, AbilityBase> storedAbilityDict;

    public GameObject referencedCameraGameObject;
    public float holdDistance = 0.0f;
    public float grabLerpFactor = 10f;
    public float shootThrustFactor = 10f;

    public GameObject pickedUpObject;
    public GameObject gunReference;
    public GameObject bullet;

    // Use this for initialization
    void Start()
    {
        currentCharacter = this.gameObject;
		resetStoredAbilities();
    }

	void resetStoredAbilities()
	{
		storedAbilityDict = new Dictionary<string, AbilityBase>();
	}

	void Grab(GameObject pickedUpObject,Rigidbody rigidObject, GameObject gravityGun, Camera characterViewCamera)
    {
        // lerp the rigidbody object from the current position to the holding position
        Vector3 currentPos = rigidObject.transform.position;
        Vector3 desiredPos = gravityGun.transform.position + (characterViewCamera.transform.forward.normalized * holdDistance);
        Vector3 calcPos = Vector3.Lerp(currentPos, desiredPos, grabLerpFactor * Time.deltaTime);

        // reset the velocity of the rigidObject so it doesn't fly off when released
        rigidObject.velocity = Vector3.zero;

        // disable gravity on the held object, or else it will slip down while held
        // ** ideally this would be done when the object is first assigned to the rigidObject variable
        // ** and then set back to true when the object is thrown/dropped
        rigidObject.useGravity = false;

        // move the object in relation to the GravityGun (without going through other colliders)
        rigidObject.MovePosition(calcPos);
        rigidObject.isKinematic = false;

		// Action for Interactive Object
		if (pickedUpObject.GetComponent<InteractiveObject>() != null) {
			pickedUpObject.GetComponent<InteractiveObject>().isBeingPicked = true;
		}
    }

	void Shoot(GameObject pickedUpObject,Rigidbody rigidObject, GameObject gravityGun, Camera characterViewCamera)
    {
        // lerp the rigidbody object from the current position to the holding position
        Vector3 currentPos = rigidObject.transform.position;
        Vector3 desiredPos = gravityGun.transform.position + (characterViewCamera.transform.forward * holdDistance);
        Vector3 calcPos = Vector3.Lerp(currentPos, desiredPos, grabLerpFactor * Time.deltaTime);

        // disable gravity on the held object, or else it will slip down while held
        // ** ideally this would be done when the object is first assigned to the rigidObject variable
        // ** and then set back to true when the object is thrown/dropped
        rigidObject.useGravity = true;

        // move the object in relation to the GravityGun (without going through other colliders)
        //rigidObject.MovePosition(calcPos);
        Vector3 forwardVector = desiredPos - gravityGun.transform.position;
        rigidObject.AddForce(characterViewCamera.transform.forward * shootThrustFactor * 1000);
		rigidObject.isKinematic = false;

		// Action for Interactive Object
		if (pickedUpObject.GetComponent<InteractiveObject>() != null) {
			pickedUpObject.GetComponent<InteractiveObject>().isBeingPicked = false;
		}
    }

	void PlaceDown(GameObject pickedUpObject, Rigidbody rigidObject, GameObject gravityGun, Camera characterViewCamera)
    {
        // lerp the rigidbody object from the current position to the holding position
        Vector3 currentPos = rigidObject.transform.position;
        Vector3 desiredPos = gravityGun.transform.position + (characterViewCamera.transform.forward * holdDistance);
        Vector3 calcPos = Vector3.Lerp(currentPos, desiredPos, grabLerpFactor * Time.deltaTime);

        // disable gravity on the held object, or else it will slip down while held
        // ** ideally this would be done when the object is first assigned to the rigidObject variable
        // ** and then set back to true when the object is thrown/dropped
        rigidObject.useGravity = true;
		rigidObject.isKinematic = false;

		// Action for Interactive Object
		if (pickedUpObject.GetComponent<InteractiveObject>() != null) {
			pickedUpObject.GetComponent<InteractiveObject>().isBeingPicked = false;
		}
    }
	
    // Update is called once per frame
	void Update () {
		checkForRetrieveAbility ();
		checkForPrimaryFire ();
		if (Input.GetKeyDown(KeyCode.R)) {
			// Reset gun
			resetStoredAbilities();
		}
	}

    void checkForRetrieveAbility()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && pickedUpObject == null)
		{
			// Left-click and no picked up object => suck out ability
			// See which object the casted ray hit onto
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				// Ray testing
				// TODO: Add gun point object
				Debug.DrawRay(currentCharacter.transform.position, hit.point - currentCharacter.transform.position, Color.blue, 10.0f);
				GameObject shotObject = hit.collider.gameObject;
				InteractiveObject interactiveObj = shotObject.GetComponent<InteractiveObject>();
				if(interactiveObj != null && interactiveObj.currentAbilityDict.Count > 0) {		
					// Create copy of object's ability to this instance
					foreach(KeyValuePair<string, AbilityBase> kvp in interactiveObj.currentAbilityDict) {
						AbilityBase ability = kvp.Value;
						storedAbilityDict.Add(ability.abilityName, ability.clone());
					}
					interactiveObj.removeAllAbilities();
				}
			}

		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && pickedUpObject == null)
		{
			// Right-click and no picked up object => push ability
			// See which object the casted ray hit onto
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				// Ray testing
				// TODO: Add gun point object
				Debug.DrawRay(currentCharacter.transform.position, hit.point - currentCharacter.transform.position, Color.blue, 10.0f);
				GameObject shotObject = hit.collider.gameObject;
				InteractiveObject interactiveObj = shotObject.GetComponent<InteractiveObject>();
				if(interactiveObj != null) {		
					// Put the stored ability to newly-pointed object
					interactiveObj.addAbilities(storedAbilityDict);
					resetStoredAbilities();
				}
			}

		}
	}

    void checkForPrimaryFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && pickedUpObject != null)
        {
            if (pickedUpObject != null)
            {
                // Shoot the object from hand
                Rigidbody grabObjectRigidBody = pickedUpObject.GetComponent<Rigidbody>();
                Camera refCamera = referencedCameraGameObject.GetComponent<Camera>();
                try
                {
					Shoot(pickedUpObject, grabObjectRigidBody, currentCharacter, refCamera);
                    pickedUpObject = null;
                }
                catch (MissingComponentException ex)
                {
                    pickedUpObject = null;
                }
            }
        }
        if (Input.GetKeyDown("e"))       // if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pickedUpObject == null)
            {
                // See which object the casted ray hit onto
                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    pickedUpObject = hit.collider.gameObject;
                    //if(hit.collider.gameObject==gameObject) Destroy(gameObject);
                }
            } else
            {
                // Shoot the object from hand
                Rigidbody grabObjectRigidBody = pickedUpObject.GetComponent<Rigidbody>();
                Camera refCamera = referencedCameraGameObject.GetComponent<Camera>();
                try
                {
					PlaceDown(pickedUpObject, grabObjectRigidBody, currentCharacter, refCamera);
                    pickedUpObject = null;
                }
                catch (MissingComponentException ex)
                {
                    pickedUpObject = null;
                }
            }

        } else if (pickedUpObject != null)
        {
            Rigidbody grabObjectRigidBody = pickedUpObject.GetComponent<Rigidbody>();
            Camera refCamera = referencedCameraGameObject.GetComponent<Camera>();
            try
            {
				Grab(pickedUpObject, grabObjectRigidBody, currentCharacter, refCamera);
            } catch(MissingComponentException ex)
            {
                pickedUpObject = null;
            }
        }
    }
}
