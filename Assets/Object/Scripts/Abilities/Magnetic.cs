using UnityEngine;

public class Magnetic : AbilityBase
{
    // Constructors
    public Magnetic() : base()
    {
        abilityName = "magnetic";
        abilityScale = 1.0f;
        allowPhysics = true;
    }

    public override void AbilityUpdate(GameObject callerGameObject)
    {
        // Find all collider that could be interacted magnetically, and add force accordingly
        float overlappedRadius = Mathf.Abs(abilityScale) * 5;
        foreach (Collider collider in Physics.OverlapSphere(callerGameObject.transform.position, overlappedRadius))
        {
			InteractiveObject obj = collider.gameObject.GetComponent<InteractiveObject> ();
			if (obj == null) {
				// Skip non-magnetic objects
				continue;
			}
			AbilityBase magneticAbility = null;
			bool haveAbility = obj.currentAbilityDict.TryGetValue(abilityName, out magneticAbility);
			if (haveAbility && collider.gameObject != callerGameObject)
            {
                // calculate direction from target to me
                Vector3 forceDirection = callerGameObject.transform.position - collider.transform.position;
				if (magneticAbility.abilityScale * abilityScale > 0)
                    forceDirection *= -1;
                // The attractive/repelling force follows inverse square law
                float magnitude = 750 / Mathf.Max((forceDirection.sqrMagnitude / 25 + 1), 1);
                // apply force on target towards me
				//Debug.Log(collider.gameObject);
                collider.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * magnitude);
            }
        }
    }


    public override void toDestroy()
    {

    }

	public override AbilityBase clone()
	{
		// Clone the current object as a new instance, implemented per ability
		Magnetic instance = new Magnetic();
		instance.abilityScale = this.abilityScale;
		instance.allowPhysics = this.allowPhysics;
		instance.abilityColor = this.abilityColor;
		return instance;
	}
}