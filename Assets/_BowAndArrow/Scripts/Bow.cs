using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{

    private Notch notch = null;
    public GameObject arrowPrefab = null;

    protected override void Awake()
    {
        base.Awake();
        notch = GetComponentInChildren<Notch>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Only notch an arrow if the bow is held
        //selectEntered.AddListener(notch.SetReady);
        //selectExited.AddListener(notch.SetReady);
        //selectEntered.AddListener(CreateAndSelectArrow);

    }
    protected override void OnActivated(ActivateEventArgs args)
    {
        CreateAndSelectArrow(args);
       
        // Make sure to do this
        base.OnActivated(args);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //selectEntered.RemoveListener(notch.SetReady);
        //selectExited.RemoveListener(notch.SetReady);
        //selectEntered.RemoveListener(CreateAndSelectArrow);
    }
    //protected override void OnSelectExited(SelectExitEventArgs args)
    //{
    //    // Make sure to do this
    //    base.OnSelectExited(args);

    //    selectEntered.RemoveListener(CreateAndSelectArrow);
    //}

    //public void CreateAndSelectArrow(SelectEnterEventArgs args)
    //{
    //    // Create arrow, force into interacting hand
    //    Arrow arrow = CreateArrow(args.interactor.transform);
    //    interactionManager.ForceSelect(args.interactor, arrow);
    //}
    public void CreateAndSelectArrow(ActivateEventArgs args)
    {
        
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactor.transform);
        interactionManager.ForceSelect(args.interactor, arrow);

    }
    

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
      
        return arrowObject.GetComponent<Arrow>();
    }
}
