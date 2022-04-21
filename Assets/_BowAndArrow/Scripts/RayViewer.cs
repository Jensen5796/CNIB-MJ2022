using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    // Just to draw a line in scene view => recommend to remove the script in the final product

    public float weaponRange = 50f;
    //private Camera fpsCam;

    //void Start()
    //{
    //    fpsCam = GetComponentInParent<Camera>();
    //}

    private void Update()
    {
        //Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        //Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
        Debug.DrawRay(this.transform.position, this.transform.forward * weaponRange, Color.green);

    }

}
