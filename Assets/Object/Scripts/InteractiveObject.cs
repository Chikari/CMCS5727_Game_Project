using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InteractiveObject : MonoBehaviour {

	// Initialized variables; the info can be used when reset is triggered
	public Vector3 startingPosition;
	public Vector3 startingScale;
    public Quaternion startingRotation;
    public AbilityBase[] startingAbilities;

    // For interactive objects, a Rigidbody must be initialized
    public Rigidbody objectRigidBody;

    // GameObject for self-reference
    public GameObject baseGameObject;

    // Runtime variables
	//public List<AbilityBase> currentAbilities;
	//public HashSet<string> abilityHash;
	public Dictionary<string, AbilityBase> currentAbilityDict;
	public bool isBeingPicked;
    protected AbilityBase toAddAbility;
    private MeshRenderer gameObjectRenderer;
    private Material objectMaterial;
    private Color calculatedColor;


    // Use this for initialization
    void Start () {
		startingPosition = this.transform.position;
		startingRotation = this.transform.rotation;
		startingScale = this.transform.localScale;
        Debug.Assert(this.GetComponent<Rigidbody>() != null, "No rigidbody found in the interactive object!");
        objectRigidBody = this.GetComponent<Rigidbody>();
        baseGameObject = this.gameObject;
        Debug.Log(baseGameObject);
        startingAbilities = this.GetComponents<AbilityBase>();
		resetAbilities();
		isBeingPicked = false;
    }

	public void resetAbilities() {
		this.transform.localScale = startingScale;
		currentAbilityDict = new Dictionary<string, AbilityBase>();
		// reset object abilities to original state
		foreach(AbilityBase ability in startingAbilities) {
			currentAbilityDict.Add(ability.abilityName, ability.clone());
			Debug.Log(ability);
		}
		UpdateObjectAbility();
	}

	public void removeAllAbilities() {
		// When the gun sucked its abilities, just clear the list
		this.transform.localScale = startingScale;
		this.GetComponent<Collider>().transform.localScale = startingScale;
		currentAbilityDict = new Dictionary<string, AbilityBase>();
		UpdateObjectAbility();
	}

	public void addAbilities(Dictionary<string, AbilityBase> fromAbilityDict) {
		// Add abilities from the gun, clone only if needed to
		foreach (KeyValuePair<string, AbilityBase> kvp in fromAbilityDict)
		{
			AbilityBase ability = kvp.Value;
			currentAbilityDict.Add(kvp.Key, kvp.Value);
		}
		UpdateObjectAbility();
	}
    
    /// <summary>
    /// Whenever an object's attribute list is updaed, this method is called to resolve for conflicts between the abilities
    /// </summary>
    public void UpdateObjectAbility()
    {
        bool canPhysicsBeEnabled = true;
		List<Color> allAbilitiesColor = new List<Color>();
		foreach (KeyValuePair<string, AbilityBase> kvp in currentAbilityDict)
		{
			AbilityBase ability = kvp.Value;
            if(!ability.allowPhysics && canPhysicsBeEnabled)
            {
                canPhysicsBeEnabled = false;
            }
            allAbilitiesColor.Add(ability.abilityColor);
        }

        if(canPhysicsBeEnabled)
        {
            enablePhysics();
        } else
        {
            disablePhysics();
        }
        UpdateColor(allAbilitiesColor);
    }

    /// <summary>
    /// Updates the color of object according to its attribute list
    /// </summary>
    public void UpdateColor(List<Color> colorList)
    {
        Debug.Log(colorList);
        Color calculatedColor;
        if (colorList.Count > 0)
        {
            calculatedColor = Color.black;
            foreach (Color color in colorList)
            {
                calculatedColor += (color / colorList.Count);
            }
            //calculatedColor *= abilityList.Length;
        }
        else
        {
            calculatedColor = AbilityBase.noAbilityColor;
        }
        /*
        // set colors
        if (lights != null && lights.Count > 0)
        {
            //Debug.Log (lights.Count);
            foreach (Light light in lights)
            {
                light.color = calculatedColor;
            }
        }*/
        gameObjectRenderer = baseGameObject.GetComponent<MeshRenderer>();
		// Don't use shared Material, otherwise all objects using same material copy will have its color affected!
        gameObjectRenderer.material.color = calculatedColor;
        gameObjectRenderer.material.SetColor("_EmissionColor", calculatedColor);
    }

    //--------------Physics-related Functions----------------
    public void enablePhysics()
    {
        objectRigidBody.isKinematic = false;
    }

    public void disablePhysics()
    {
        // Remove all forces and set kinematic (Forces, collisions or joints will not affect the rigidbody anymore)
        objectRigidBody.velocity = Vector3.zero;
        objectRigidBody.angularVelocity = Vector3.zero;
        objectRigidBody.isKinematic = true;
    }

	void OnCollisionEnter(Collision hit)
	{
		foreach (KeyValuePair<string, AbilityBase> kvp in currentAbilityDict)
		{
			kvp.Value.AbilityOnCollisionEnter(baseGameObject, hit);
		}
	}

    // Update is called upon fixed interval
    void FixedUpdate()
    {
		foreach (KeyValuePair<string, AbilityBase> kvp in currentAbilityDict)
		{
			AbilityBase ability = kvp.Value;
            ability.AbilityUpdate(baseGameObject);
        }
    }

    void Update()
    {
		/*
        // For coloring in editor mode only!
        if (Application.isEditor && !Application.isPlaying) { 
			List<Color> allAbilitiesColor = new List<Color>();
			foreach (KeyValuePair<string, AbilityBase> kvp in currentAbilityDict)
			{
				AbilityBase ability = kvp.Value;
                allAbilitiesColor.Add(ability.abilityColor);
                ability.AbilityUpdate(baseGameObject);
            }
            UpdateColor(allAbilitiesColor);
        }*/
    }
}
