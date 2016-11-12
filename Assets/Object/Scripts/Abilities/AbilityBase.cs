using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    // Variables for an ability
	public string abilityName = "ability_base";
    public Color abilityColor = Color.gray;
    public float abilityScale = 0.0f;
    public bool allowPhysics = true;

    // Global (static) variables
    public static Color noAbilityColor = Color.gray;
    protected bool isPlatform = false;
    
    // Constructors
    public AbilityBase()
    {
    }

    public AbilityBase(Color targetColor)
    {
        abilityColor = targetColor;
    }


	public virtual void AbilityOnCollisionEnter(GameObject callerGameObject, Collision hit)
    {

    }

    public virtual void AbilityUpdate(GameObject callerGameObject)
    {

    }

    public virtual void FixedUpdate()
    {
    }


    public virtual void toDestroy()
    {

    }

	public virtual AbilityBase clone()
	{
		// Clone the current object as a new instance, implemented per ability
		return null;
	}
    
}