using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoundsTest
{
    center,
    onScreen,
    offScreen
}

public class Utils : MonoBehaviour {

    //============================Bounds Functions==========================\\

    //Creates bounds that encapsulate the two Bounds passed in.
    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        if (b0.size == Vector3.zero && b1.size == Vector3.zero)
        {
            return (b1);
        }
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        // Stretch b0 to include the b1.min and b1.max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        //obtain references to go renderer and collider if they exist
        Collider collider = go.GetComponent<Collider>();
        Renderer renderer = go.GetComponent<Renderer>();

        //create a new bounds object
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);

        //if this game object has a renderer component, encapsulate in the bounds
        if (renderer != null)
        {
            b = BoundsUnion(b, renderer.bounds);
        }
        //if this game object has a collider component, encapsulate in the bounds
        if (collider != null)
        {
            b = BoundsUnion(b, collider.bounds);
        }

        //Recursively iterate through each child of this gameObject.transform
        foreach (Transform t in go.transform)
        {
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }
        return (b);
    }

    //Make a static read-only public property camBounds
    private static Bounds _camBounds;

    public static Bounds cambounds
    {
        get
        {
            //if _camBounds hasn't been set yet
            if (_camBounds.size == Vector3.zero)
            {
                SetCameraBounds();
            }
            return (_camBounds);
        }
    }
    //This function is used by camBounds to set _camBounds and can also be called directly
    public static void SetCameraBounds(Camera cam = null)
    {
        //If no camera was passed in, use the main Camera
        if (cam == null) cam = Camera.main;
        //This makes a couple of important assumptions about the camera!:
        //   1. The camera is Orthographic
        //   2. The camera is at a rotation of R:[0,0,0]

        // Make Vector3s at the topLeft and bottomRight of the Screen coords
        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

        // Convert these to world coordinates
        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        // Adjust ther zs to be at the near and far Cmera clipping planes
        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;

        // Find the center of the bounds
        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        // Expand _camBounds to encapsulate the extents
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // Checks to see whether the bounds bnd are within the camBounds
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return (BoundsInBoundsCheck(cambounds, bnd, test));
    }

    //Checks to see whether bounds lilB are within bounds bigB
    public static Vector3 BoundsInBoundsCheck( Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        //The behavior of this function is different based on the BoundsTest that has been selected.

        //get the center of lilb
        Vector3 pos = lilB.center;

        // Initialize the offset at [0,0,0]
        Vector3 off = Vector3.zero;

        switch (test)
        {
            //the center test determines what off (offset) would have to be applied to lilB to move its center back inside bigB
            case BoundsTest.center:
                if( bigB.Contains( pos ))
                {
                    return (Vector3.zero);
                }
                if (pos.x > bigB.max.x)
                {
                    off.x = pos.x - bigB.max.x;
                }
                else if (pos.x < bigB.min.x)
                {
                    off.x = pos.x - bigB.min.x;
                }
                if (pos.y > bigB.max.y)
                {
                    off.y = pos.y - bigB.max.y;
                }
                else if (pos.y < bigB.min.y)
                {
                    off.y = pos.y - bigB.min.y;
                }
                if (pos.z > bigB.max.z)
                {
                    off.z = pos.z - bigB.max.z;
                }
                else if (pos.z < bigB.min.z)
                {
                    off.z = pos.z - bigB.min.z;
                }
                return (off);
            //the onScreen test determines what off would have to be applied to keep all of lilB inside bigB
            case BoundsTest.onScreen:
                if (bigB.Contains( lilB.min) && bigB.Contains(lilB.max))
                {
                    return (Vector3.zero);
                }

                if (lilB.max.x > bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x < bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                if (lilB.max.y > bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y < bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                if (lilB.max.z > bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z < bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }
                return (off);
            //The offScreen test determines what off would need to be applied to move any tiny part of lilB inside of bigB
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if( cMin || cMax)
                {
                    return (Vector3.zero);
                }

                if(lilB.min.x > bigB.max.x)
                {
                    off.x = lilB.min.x - bigB.max.x;
                }
                else if(lilB.max.x < bigB.min.x)
                {
                    off.x = lilB.max.x - bigB.min.x;
                }
                if (lilB.min.y > bigB.max.y)
                {
                    off.y = lilB.min.y - bigB.max.y;
                }
                else if (lilB.max.y < bigB.min.y)
                {
                    off.y = lilB.max.y - bigB.min.y;
                }
                if (lilB.min.z > bigB.max.z)
                {
                    off.z = lilB.min.z - bigB.max.z;
                }
                else if (lilB.max.z < bigB.min.z)
                {
                    off.z = lilB.max.z - bigB.min.z;
                }

                return (off);     
        }

        return (Vector3.zero);
    }

//==============================Transform Functions====================================\\

    //This function will iteratively climb up the transform.parent tree
    //  until it either finds a parent with a tag != "Untagged" or no parent
    public static GameObject FindTaggedParent(GameObject go)
    {
        //If this gameObject has a tag
        if(go.tag != "Untagged")
        {
            return go;
        }
        //we've reached the top of the hierarchy without finding an interesting tag
        if(go.transform.parent == null)
        {
            return null;
        }
        //recursively climb up the hierarchy
        return (FindTaggedParent(go.transform.parent.gameObject));
    }

    //this version of the function handles things if a Transform is passed in
    public static GameObject FindTaggedParent(Transform t)
    {
        return FindTaggedParent(t.gameObject);
    }
}