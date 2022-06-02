using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Quiver : XRBaseInteractable
{
    public GameObject arrowPrefab = null;
    private Text testing;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(CreateAndSelectArrow);
        GameObject go2 = GameObject.Find("TestText");
        testing = go2.GetComponent<Text>();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(CreateAndSelectArrow);
    }

    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        if (!Bow.isArrowLoaded)
        {
            // Create arrow, force into interacting hand
            testing.text = args.interactor.name;
            if (CanvasManager.LRHandSelection == "L" && args.interactor.name == "RightHand Controller")
            {
                Arrow arrow = CreateArrow(args.interactor.transform);
                interactionManager.ForceSelect(args.interactor, arrow);
                Bow.isArrowLoaded = true;
            }
            else if (CanvasManager.LRHandSelection == "R" && args.interactor.name == "LeftHand Controller")
            {
                Arrow arrow = CreateArrow(args.interactor.transform);
                interactionManager.ForceSelect(args.interactor, arrow);
                Bow.isArrowLoaded = true;
            }
        }
        
        
        
    }

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();

    }
}
