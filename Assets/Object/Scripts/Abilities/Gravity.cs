using UnityEngine;

public class Gravity : AbilityBase
{
    public static float gravitationConstant = -250.0f;

    // Constructors
    public Gravity() : base()
    {
        abilityName = "gavity";
        abilityScale = -1.0f;
        allowPhysics = true;
    }

    public override void AbilityUpdate(GameObject callerGameObject)
    {
        // If rigidbody exists from the caller game object, add a "local" force from existing gravitational pull
        Rigidbody rb = callerGameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.AddForce(new Vector3(0, abilityScale * gravitationConstant, 0));
        }
    }


    public override void toDestroy()
    {

    }

	public override AbilityBase clone()
	{
		// Clone the current object as a new instance, implemented per ability
		Gravity instance = new Gravity();
		instance.abilityScale = this.abilityScale;
		instance.allowPhysics = this.allowPhysics;
		instance.abilityColor = this.abilityColor;
		return instance;
	}
}