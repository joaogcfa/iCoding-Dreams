using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GravityGun : MonoBehaviour {

    [Header("Gravity Gun Settings")]

    [Tooltip("The speed the object will be throw.")] [SerializeField]
    private float throwSpeed = 1000.0f;
    
    [Tooltip("The maximum distance the gravity gun can reach.")] [SerializeField]
    private float distanceToGrab = 10.0f;

    [Tooltip("The speed the object will come.")] [SerializeField]
    private float speedGrab = 20.0f;

    [Tooltip("The speed the object will rotate until it reaches final rotation.")] [SerializeField]
    private float speedGrabRotate = 500.0f;

    [Tooltip("The force that holds the object")] [SerializeField]
    private float gripForce = 100.0f;

    [Tooltip("The max distance the object can be from the attachment point before being dropped")] [SerializeField] [Range(0.0f, 2.0f)]
    private float maxDistanceFromAttachment = 1.0f;
    
    [Tooltip("Align object axis.")] [SerializeField]
    private bool alignObject = true;

    private Transform orignalAttachmentPoint;
    private bool hasObject = false;
    private bool objectReachedAttachmentPoint = false;
    private GameObject grabbedObject;
    private float distanceFromAttachmentPoint;

    [Header("Hover Effect")]

    [Tooltip("The point where the objects will hover over.")] [SerializeField]
    private Transform attachmentPoint;

    [Tooltip("The speed at which the object will hover.")] [SerializeField] [Range(0f, 20f)]
    private float objectHoverSpeed = 10.0f;
    
    [Tooltip("The magnitude at which the object will hover.")] [SerializeField] [Range(0f, 0.01f)] 
    private float objectHoverMagnitude = 0.009f;

    [Header("Inventory Settings")]

    [Tooltip("The point where the object's miniature will hover.")] [SerializeField]
    private Transform miniatureAttachmentPoint;

    [Tooltip("The speed at which the object's miniature will hover.")] [SerializeField] [Range(0f, 20f)]
    private float miniatureHoverSpeed = 2.0f;

    [Tooltip("The magnitude at which the object's miniature will hover.")] [SerializeField] [Range(0f, 0.01f)] 
    private float miniatureHoverMagnitude = 0.002f;

    [Tooltip("The direction at which the object's miniature will spin.")] [SerializeField]
    private Vector3 miniatureSpinDirection = new Vector3(1f, 1f, 0f);

    [Tooltip("The speed at which the object's miniature will spin.")] [SerializeField] [Range(0f, 1000f)]
    private float miniatureSpinSpeed = 500.0f; 

    private GameObject objectInInventory;
    private GameObject objectInInventoryMiniature;

    private float objectRandomTimeOffset; // Used to randomize the time offset for the object hovering effect.
    private float miniatureRandomTimeOffset; // Used to randomize the time offset for the object miniature hovering effect.

    [Header("Gravity Gun Sounds")]

    [Tooltip("The sound that will played when a object is being held.")] [SerializeField]
    private AudioSource audioHolding;

    Vector3 mousescroll;
    float x;
    float y;
    float z;


    void Start() {
        orignalAttachmentPoint = attachmentPoint;
        distanceFromAttachmentPoint = (transform.position - attachmentPoint.position).magnitude;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (!hasObject) { // only try grab a object if the gravity gun doesn't have one.
                Raycast();
            } else if (objectReachedAttachmentPoint) { // if the object is grabbed and it reached the attachment point, throw it.
                ThrowObject();
            }
        }

        if (Input.GetMouseButtonDown(1) && hasObject) { // only release the object if it is grabbed.
            ReleaseObject();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (hasObject && objectInInventory == null) { // only add a object if there is no object in the inventory and has a object to be added.
                miniatureRandomTimeOffset = objectRandomTimeOffset; // keeps the same time offset for the miniature.
                PutObjectInInventory();
            } else if (objectInInventory != null && !hasObject) { // only remove a object if there is a object in the inventory and has no object being held.
                objectRandomTimeOffset = miniatureRandomTimeOffset; // keeps the same time offset for the object when releasing the it from the inventory.
                ReleaseObjectFromInventory();
            }
        }
        if (Input.mouseScrollDelta.y != 0){
            if(hasObject){
                ResizeObject();
            }
        }
    }

    void FixedUpdate() {
        if (hasObject) { // if a object is being held, lerp it to the attachment point then apply a hover effect.
            if (objectReachedAttachmentPoint) {
                VerticalHoverEffect(grabbedObject, objectRandomTimeOffset, objectHoverMagnitude, objectHoverSpeed);
                // DropObjectDistanceCheck();
                DropObjectRaycast();
            } else {
                LerpObject();
            }
        }

        if (objectInInventoryMiniature != null) { // if a miniature exists apply a hover effect and spin it.
            VerticalHoverEffect(objectInInventoryMiniature, miniatureRandomTimeOffset, miniatureHoverMagnitude, miniatureHoverSpeed);
            SpinMiniature();
        }
    }

    void OnDrawGizmos() {
        // Draws a line tha shows the distance that the gravity gun can reach.
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distanceToGrab);

        // Draws a wire sphere that shows the max distance that the object can be from the attachment point.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attachmentPoint.position, maxDistanceFromAttachment);

        // Draws a sphere that shows the attachment point.
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(attachmentPoint.position, 0.1f);

        // Draws a sphere that shows the object's miniature attachment point.
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(miniatureAttachmentPoint.position, 0.1f);
    }

    // ##############################################################################################################################################################
    //                                                                          Gravity Gun                                                                         
    // ##############################################################################################################################################################

    // Does a raycast from the camera to the center of the screen and if it hits a object that has a component Holdable, it will grab it.
    void Raycast() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceToGrab)) {
            if (hit.collider.gameObject.GetComponent<Holdable>() != null) {
                grabbedObject = hit.collider.gameObject;
                hasObject = true;
                GrabObject();
            }
        }
    }

    // Attaches the object to the gravity gun and disables the object's gravity.
    void GrabObject() {
        hasObject = true;
        grabbedObject.GetComponent<Holdable>().gripForce = gripForce;
        grabbedObject.GetComponent<Holdable>().gun = this;
        grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        grabbedObject.transform.parent = attachmentPoint; // Set the object's parent to the attachment point so it will follow the gravity gun.
        objectRandomTimeOffset = Random.Range(0f, 1f);
    }
    
    // Moves and rotate the object to the attachment point.
    void LerpObject() {
        if (grabbedObject == null) return;
        grabbedObject.transform.position = Vector3.MoveTowards(grabbedObject.transform.position, attachmentPoint.position, speedGrab * Time.deltaTime);
        if (alignObject)
            grabbedObject.transform.rotation = Quaternion.RotateTowards(grabbedObject.transform.rotation, attachmentPoint.rotation, speedGrabRotate * Time.deltaTime);

        if ((grabbedObject.transform.position - attachmentPoint.position).magnitude <= 0.01f && Quaternion.Angle(grabbedObject.transform.rotation, attachmentPoint.rotation) <= 0.01f) {
            objectReachedAttachmentPoint = true;
        }
    }

    // Set that the object has not reached the attachment point, is called by Holdable.cs when the object is stop colliding with another object.
    public void ResetReachedAttachmentPoint() {
        objectReachedAttachmentPoint = false;
    }

    // Detaches the object from the gravity gun and add a velocity to throw it.
    void ThrowObject() {
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        Vector3 throwDirection = (grabbedObject.transform.position - transform.position).normalized;
        ReleaseObject();
        rb.velocity = throwDirection * throwSpeed;
    }

    // Detaches the object from the gravity gun and re-enables the object's gravity.
    public void ReleaseObject() {
        if (grabbedObject == null) return;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabbedObject.GetComponent<Holdable>().gun = null;

        // Reseting variables to when the object is not grabbed.
        grabbedObject.transform.parent = null;
        grabbedObject = null;
        hasObject = false;
        objectReachedAttachmentPoint = false;
        attachmentPoint = orignalAttachmentPoint;
    }

    // Drops the object if it is too far away from the attachment point.
    void DropObjectDistanceCheck() {
        if (grabbedObject == null) return;
        if ((grabbedObject.transform.position - attachmentPoint.position).magnitude > maxDistanceFromAttachment) {
            ReleaseObject();
        }
    }

    // Drops the object if a raycast stops colliding with the object.
    void DropObjectRaycast() {
        if (grabbedObject == null) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanceFromAttachmentPoint)) {
            if (hit.collider.gameObject != grabbedObject) {
                ReleaseObject();
            }
        }
    }

    void ResizeObject(){
        mousescroll = Input.mouseScrollDelta;
        x += mousescroll.y;
        y += mousescroll.y;
        z += mousescroll.y;
        grabbedObject.transform.localScale = new Vector3(x,y,z);
        // grabbedObject.transform.position += new Vector3(x,0,z);
        if (x>3)
        {
            x = 3;
        }
        if (x < 0.1f)
        {
            x = 0.1f;
        }
        if (y>3)
        {
            y = 3;
        }
        if (y<0.1f)
        {
            y = 0.1f;
        }
        if (z>3)
        {
            z = 3;
        }
        if (z<0.1f)
        {
            z = 0.1f;
        }
    }

    // ##############################################################################################################################################################
    //                                                                          Inventory
    // ##############################################################################################################################################################

    // Put the object in the inventory and spawn a miniature.
    void PutObjectInInventory() {
        // Spawn the miniature.
        objectInInventoryMiniature = Instantiate(grabbedObject, attachmentPoint.position, attachmentPoint.rotation);
        objectInInventoryMiniature.transform.position = miniatureAttachmentPoint.position;
        objectInInventoryMiniature.transform.parent = miniatureAttachmentPoint;
        objectInInventoryMiniature.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Scale the miniature.
        // disable gravity and collider.
        objectInInventoryMiniature.GetComponent<Rigidbody>().useGravity = false;
        objectInInventoryMiniature.GetComponent<Collider>().enabled = false;

        // Set the object in the inventory and disable the object.
        objectInInventory = grabbedObject;
        objectInInventory.SetActive(false);
        grabbedObject = null;
        hasObject = false;
        objectReachedAttachmentPoint = false;
    }

    // Remove the object from the inventory and destroy the miniature.
    void ReleaseObjectFromInventory() {
        // Reenable the object from inventory.
        objectInInventory.SetActive(true);
        grabbedObject = objectInInventory;
        GrabObject();
        // Destroy the miniature.
        Destroy(objectInInventoryMiniature);
        // Reset the variables.
        objectInInventory = null;
        objectInInventoryMiniature = null;
    }

    // ##############################################################################################################################################################
    //                                                                         Visual Effects                                                                        
    // ##############################################################################################################################################################

    // Moves a object up and down in a sinodal way.
    void VerticalHoverEffect(GameObject objectToHover, float timeOffset, float magnitude, float speed) {
        Vector3 aa = new Vector3(0f, Mathf.Sin(Time.time * speed + timeOffset) * magnitude, 0f);
        objectToHover.transform.position += aa;
    }

    // Moves a object left and right in a sinodal way.
    void HorizontalHoverEffect(GameObject objectToHover, float timeOffset, float magnitude, float speed) {
        grabbedObject.transform.position += new Vector3(Mathf.Sin(Time.time * speed + timeOffset) * magnitude, 0f, 0f);
    }

    // Spin the object's miniature.
    void SpinMiniature() {
        objectInInventoryMiniature.transform.Rotate(miniatureSpinDirection, Time.deltaTime * miniatureSpinSpeed);
    }
}
