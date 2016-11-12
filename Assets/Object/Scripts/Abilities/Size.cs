using UnityEngine;

public class Size : AbilityBase
{
    // Constructors
    public Size() : base()
    {
        abilityName = "size";
        abilityScale = 1.0f;
        allowPhysics = true;
    }

    public override void AbilityUpdate(GameObject callerGameObject)
    {
		if (callerGameObject.GetComponent<InteractiveObject>() != null) {
			InteractiveObject obj = callerGameObject.GetComponent<InteractiveObject>();
			Vector3 toScale = obj.startingScale * abilityScale;
			callerGameObject.transform.localScale = toScale;
			// If collider exists from the caller game object, resize it as well
			Collider collider = callerGameObject.GetComponent<Collider>();
			if(collider != null)
			{
				collider.transform.localScale = toScale;
			}
		}
    }


    public override void toDestroy()
    {

    }

	public override AbilityBase clone()
	{
		// Clone the current object as a new instance, implemented per ability
		Size instance = new Size();
		instance.abilityScale = this.abilityScale;
		instance.allowPhysics = this.allowPhysics;
		instance.abilityColor = this.abilityColor;
		return instance;
	}
}