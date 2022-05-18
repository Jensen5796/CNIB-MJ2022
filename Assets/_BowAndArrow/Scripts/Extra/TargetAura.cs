using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetAura : MonoBehaviour, IArrowHittable
{
    public float forceAmount = 1.0f;
    public Material otherMaterial = null;
    //TODO: make other material if going to change target colour when hit
    public Transform targetCenter;

    public void Hit(Arrow arrow)
    {
        //how to make this detect a raycast hit instead of arrow?

        //Calculate position distance and direction from center of spherical target

        //Tell user how to direct aim to reach target
        SumScore.Add(1);

        //ApplyMaterial();
        //Debug.Log("You hit the " + name);
        //Debug.Log("Increase score here");
        //Debug.Log("Add sound effect/celebration here");
        //ApplyForce(arrow.transform.forward);

    }

    private void ApplyMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = otherMaterial;
    }

    private void ApplyForce(Vector3 direction)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(direction * forceAmount);
    }

    private void CalculateDirection()
    {
        //not void, figure out what to return...

    }

    private void CalculateDistanceFromCenter()
    {
        //not void, figure out what to return...
    }

    private T GetChildComponentByName<T>(string name) where T : Component
    {
        foreach (T component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }
}
