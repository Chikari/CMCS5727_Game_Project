using UnityEngine;

public class Size : AbilityBase
{
	private float lerpFrac = 5.0f;	
	private float currentScale = 1.0f;

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
			currentScale += (abilityScale - currentScale) / lerpFrac;
			InteractiveObject obj = callerGameObject.GetComponent<InteractiveObject>();
			Vector3 toScale = obj.startingScale * currentScale;

			// Resize object and collider scale
			callerGameObject.transform.localScale = toScale;
			Collider collider = callerGameObject.GetComponent<Collider>();
			if(collider != null)
			{
				collider.transform.localScale = toScale;
			}
		}
		/*
		if (this.gameObject.GetComponent<Rigidbody> () != null) {
			if (isShrink) {
				this.gameObject.GetComponent<Rigidbody>().mass = 100.0f;
			} else {
				this.gameObject.GetComponent<Rigidbody>().mass = 10000.0f;
				Vector3 toVel = this.gameObject.GetComponent<Rigidbody>().velocity;
				toVel.x = 0.0f;
				toVel.z = 0.0f;
				this.gameObject.GetComponent<Rigidbody>().velocity = toVel;
			}
		}
		currentScale += (targetScale - currentScale) / lerpFrac;
		transform.localScale = new Vector3 (currentDimens.x * currentScale, currentDimens.y * currentScale, currentDimens.z * currentScale);
		*/
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