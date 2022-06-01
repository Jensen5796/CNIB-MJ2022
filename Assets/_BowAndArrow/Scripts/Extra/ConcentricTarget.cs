using UnityEngine;

public class ConcentricTarget : MonoBehaviour, IArrowHittable
{
    public float forceAmount = 1.0f;
    public Material otherMaterial = null;
    public int ascore; // Track points based on region hit by arrow
    //TODO: make other material if going to change target colour when hit

    public AudioClip hitTarget;
    public AudioClip celebration;

    public void Hit(Arrow arrow)
    {

        ApplyMaterial();
        Debug.Log("You hit the " + name);
        //Debug.Log("Increase score here");
        SumScore.Add(ascore);
        Debug.Log("Add sound effect/celebration here");
        GetComponent<AudioSource>().PlayOneShot(hitTarget);
       // GetComponent<AudioSource>().PlayOneShot(celebration);
        ApplyForce(arrow.transform.forward);

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
}
