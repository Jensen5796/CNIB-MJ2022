using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Arrow : XRGrabInteractable
{
    [Header("Settings")]
    public float speed = 2000.0f;

    [Header("Hit")]
    public Transform tip = null;
    public LayerMask layerMask = ~Physics.IgnoreRaycastLayer;

    private new Collider collider = null;
    private new Rigidbody rigidbody = null;

    private Vector3 lastPosition = Vector3.zero;
    private bool launched = false;
    //Debugging
    private Text test;

    public LayerMask targetAuraLayermask;
    public SoundCues soundcue;


    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        targetAuraLayermask = LayerMask.GetMask("AuraColliders");
        test = FindObjectOfType<Text>();
        soundcue = GetComponent<SoundCues>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        // Do this first, so we get the right physics values
        if (args.interactor is XRDirectInteractor)
            Clear();

        // Make sure to do this
        base.OnSelectEntering(args);
    }

    private void Clear()
    {
        SetLaunch(false);
        TogglePhysics(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Make sure to do this
        base.OnSelectExited(args);

        // If it's a notch, launch the arrow
        if (args.interactor is Notch notch)
            Launch(notch);
    }

    private void Launch(Notch notch)
    {
        // Double-check incase the bow is dropped with arrow socketed
        //if (notch.IsReady)
        {
            SetLaunch(true);
            UpdateLastPosition();
            ApplyForce(notch.PullMeasurer);
        }
    }

    private void SetLaunch(bool value)
    {
        collider.isTrigger = value;
        launched = value;
    }

    private void UpdateLastPosition()
    {
        // Always use the tip's position
        lastPosition = tip.position;
    }

    private void ApplyForce(PullMeasurer pullMeasurer)
    {
        // Apply force to the arrow
        float power = pullMeasurer.PullAmount;
        Vector3 force = transform.forward * (power * speed);
        rigidbody.AddForce(force);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        RaycastIntoScene(tip.position, tip.TransformDirection(Vector3.down));
        RemoveDroppedArrow();

        if (launched)
        {
            // Check for collision as often as possible
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (CheckForCollision())
                    launched = false;

                UpdateLastPosition();
            }

            // Only set the direction with each physics update
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
                SetDirection();
        }
    }

    private void SetDirection()
    {
        // Look in the direction the arrow is moving
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

    private bool CheckForCollision()
    {
        // Check if there was a hit
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hit, layerMask))
        {
            // don't let arrow hit/stick to aura collider
            if (hit.collider.CompareTag("AuraCollider"))
            {
                //don't stop the arrow
                return false;
            }
            else
            {
                //check to see if it missed
                if (hit.collider.CompareTag("GameAreaBoundary")){
                    //regular stuff that would happen
                    TogglePhysics(false);
                    ChildArrow(hit);
                    CheckForHittable(hit);

                    //arrow hit either ground or game boundary dome
                    //cue Missed Target reactions
                    //destroy arrow
                    test.text = "missed target";
                    MissedTarget();
                    Destroy(this.gameObject, 2);

                    
                }
                    //otherwise it hit something else
                else {
                    TogglePhysics(false);
                    ChildArrow(hit);
                    CheckForHittable(hit);
                }
            }
        }

        return hit.collider != null;
    }

    private void TogglePhysics(bool value)
    {
        // Disable physics for childing and grabbing
        rigidbody.isKinematic = !value;
        rigidbody.useGravity = value;
    }

    private void ChildArrow(RaycastHit hit)
    {
        // Child to hit object
        Transform newParent = hit.collider.transform;
        transform.SetParent(newParent);
    }

    private void CheckForHittable(RaycastHit hit)
    {
        // Check if the hit object has a component that uses the hittable interface
        GameObject hitObject = hit.transform.gameObject;
        IArrowHittable hittable = hitObject ? hitObject.GetComponent<IArrowHittable>() : null;

        // If we find a valid component, call whatever functionality it has
        if (hittable != null)
            hittable.Hit(this);
    }

    private void RaycastIntoScene(Vector3 targetPosition, Vector3 direction)
    {

        RaycastHit targetHit;
        if (Physics.Raycast(targetPosition, direction, out targetHit, Mathf.Infinity, targetAuraLayermask))
        {
            test.text = "Aura Collider detected";
            //Transform targetCenter = targetHit.collider.gameObject.GetComponentInChildren<SphereCollider>().transform;
            Transform targetCenter = targetHit.collider.gameObject.GetComponentInChildren<ConcentricTarget>().gameObject.transform;
            test.text = targetCenter.position.x.ToString();
            GivePlayerFeedback(targetHit.point, targetCenter.transform);
        }
        else
        {
            test.text = "Aura Collider not detected";
        }

    }

    public void GivePlayerFeedback(Vector3 targetHit, Transform targetCenter)
    {

        Vector3 targetDirection = CalculateDirection(targetCenter, tip.transform);
        Vector3 hitDirection = CalculateDirection(targetHit, tip.transform);



        //get signed angle
        float angleBetween_X = Vector3.SignedAngle(targetDirection, hitDirection, Vector3.up);
        //X direction: negative is left of target, positive is right of target
        float angleBetween_Y = Vector3.SignedAngle(targetDirection, hitDirection, Vector3.right);
        //Y direction: negative is above target, positive is below target

        test.text = "X=" + angleBetween_X.ToString() + " Y=" + angleBetween_Y.ToString();
        //if (distance < 0.5)
        //{
        //    //audio: shoot the arrow,
        //    GetComponent<AudioSource>().PlayOneShot(clip_shootArrow);
        //    //haptic trigger
        //}
        //else
        {
            if (angleBetween_X > 1.5)   // aim left
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().panStereo = -1;
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_turnLeft);
                }

                //check height
                if (angleBetween_Y < -1.5) // aim lower
                {
                    //test.text = "Aim left and lower";
                    test.text = "X=" + angleBetween_X.ToString() + " Y=" + angleBetween_Y.ToString();
                    //audio: aim higher?? or high pitch beep
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().panStereo = -1;
                        //  GetComponent<AudioSource>().PlayOneShot(soundcue.clip_aimUp);
                        GetComponent<AudioSource>().PlayOneShot(soundcue.clip_leftLower);
                    }
                }
                else if (angleBetween_Y > 1.5) // aim higher
                {
                    //test.text = "Aim left and higher";
                    test.text = "X=" + angleBetween_X.ToString() + " Y=" + angleBetween_Y.ToString();
                    //audio: aim lower?? or low pitch beep

                    if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().panStereo = -1;
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_leftHigher);
                }

            }
            else if (angleBetween_X < -1.5) //aim right
            {

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().panStereo = 1;
                    //audio: aim to left?? or beep from left
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_turnRight);
                }


                //check height
                if (angleBetween_Y < -1.5) //aim lower
                {
                    //test.text = "Aim right and lower";
                    test.text = "X=" + angleBetween_X.ToString() + " Y=" + angleBetween_Y.ToString();

                    if (!GetComponent<AudioSource>().isPlaying) 
                    {
                        GetComponent<AudioSource>().panStereo = 1;
                        //audio: aim higher?? or high pitch beep
                        GetComponent<AudioSource>().PlayOneShot(soundcue.clip_rightLower);
                    }

                }
                else if (angleBetween_Y > 1.5) // aim higher
                {
                    //test.text = "Aim right and higher";
                    test.text = "X=" + angleBetween_X.ToString() + " Y=" + angleBetween_Y.ToString();

                    if (!GetComponent<AudioSource>().isPlaying ) 
                    {
                        GetComponent<AudioSource>().panStereo = 1;
                        //audio: aim lower?? or low pitch beep
                        GetComponent<AudioSource>().PlayOneShot(soundcue.clip_rightHigher);
                    }

                }
            }
            else if (angleBetween_X > -1.5 && angleBetween_X < 1.5 && angleBetween_Y > -1.5 && angleBetween_Y < 1.5)
            {
                test.text = "Shoot the arrow";
                //audio: shoot the arrow,
                if (!GetComponent<AudioSource>().isPlaying) 
                {
                    GetComponent<AudioSource>().panStereo = 0;
                    GetComponent<AudioSource>().PlayOneShot(soundcue.clip_shootArrow);
                }
                   
            }


        }

    }
    private Vector3 CalculateDirection(Transform target, Transform arrowTip)
    {
        Vector3 direction = target.position - arrowTip.position;
        direction.Normalize();
        return direction;
    }
    private Vector3 CalculateDirection(Vector3 target, Transform arrowTip)
    {
        Vector3 direction = target - arrowTip.position;
        direction.Normalize();
        return direction;
    }

    void MissedTarget()
    {
        //reactions from missing target
        test.text = "missed the target";
        //sound cue:
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().PlayOneShot(soundcue.clip_arrowMissed);
        }

    }

    void RemoveDroppedArrow()
    {
        
        if (this.gameObject.transform.position.y < 0.2)
        {
            if (!launched)
            {
                //arrow is on ground, and it was not launched (it dropped)
                Destroy(this.gameObject, 2);
            }
            
        }
    }
}
