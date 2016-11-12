using UnityEngine;

public class Bounciness : AbilityBase
{
    // Constructors
    public Bounciness() : base()
    {
        abilityName = "bounciness";
        abilityScale = 1.0f;
        allowPhysics = true;
    }

	public override void AbilityOnCollisionEnter(GameObject callerGameObject, Collision hit)
	{
		// Discard bounce back if picked up
		if (callerGameObject.GetComponent<InteractiveObject> () != null && callerGameObject.GetComponent<InteractiveObject> ().isBeingPicked) {
			return;
		}
        float mag = 0;
        float thrust = 4000;
        //Debug.Log("OnCollisionEnter");
        // If it is player
        if (hit.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>() != null)
        {
            Debug.Log("OnCollisionEnter FirstPersonController");
            // If the contact normal is on y-axis, make it jump
            foreach (ContactPoint contact in hit.contacts)
            {
                if (contact.normal.y < 0.00000001)
                {
                    // Need to trigger jumping action from FPS script, since our character does not follow world forces (isKinematic)
                    hit.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().JumpHigh(abilityScale);
                    return;
                }
            }
        }
        else if (hit.gameObject.GetComponent<Rigidbody>() != null)
        {
            // Bounded magnitude when bouncing off
            mag = Mathf.Min(Mathf.Max(hit.gameObject.GetComponent<Rigidbody>().velocity.magnitude, 1), 5);

			float rotation = callerGameObject.transform.localRotation.x;
            rotation = Mathf.Cos(rotation * 3.14159f / 180f);

            hit.gameObject.GetComponent<Rigidbody>().AddForce(hit.contacts[0].normal * mag * -thrust * rotation);

            // Repel force for the object
            mag = Mathf.Min(Mathf.Max(hit.gameObject.GetComponent<Rigidbody>().velocity.magnitude, 1), 5);
            //GetComponent<Rigidbody>().AddForce(hit.contacts[0].normal * mag * thrust);
        }
    }

    public override void AbilityUpdate(GameObject callerGameObject)
    {
    }


    public override void toDestroy()
    {

    }

	public override AbilityBase clone()
	{
		// Clone the current object as a new instance, implemented per ability
		Bounciness instance = new Bounciness();
		instance.abilityScale = this.abilityScale;
		instance.allowPhysics = this.allowPhysics;
		instance.abilityColor = this.abilityColor;
		return instance;
	}
}