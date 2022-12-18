using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverThingController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public GameObject FrontRaySourceObject;

    public GameObject BackRaySourceObject;
 
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public ForceMode mode = ForceMode.Acceleration;
    public float appliedForce = 0.8f;

    // Update is called once per frame
    void FixedUpdate()
    {
        //front
        ApplyGravityForce(new Vector3(0f, appliedForce, 0f), FrontRaySourceObject.transform.TransformDirection(Vector3.down), FrontRaySourceObject.transform.position, Vector3.down);
        //back
        ApplyGravityForce(new Vector3(0f, appliedForce, 0f), BackRaySourceObject.transform.TransformDirection(Vector3.down), BackRaySourceObject.transform.position, Vector3.down);
    
        //Forward
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if(vertical != 0){
            _rigidbody.drag = 0.2f;
            _rigidbody.AddForce(transform.TransformDirection(Vector3.forward) * vertical * 500);
        }else{
            _rigidbody.drag = 1;
        }

        if(horizontal != 0 ){
            //lean
            var lean = 0f;
            if(transform.rotation.eulerAngles.z > -30 && horizontal < 0){
                lean = 1f;
            }
            Debug.Log(horizontal);

            if((transform.rotation.eulerAngles.z > 330 || transform.rotation.eulerAngles.z < 30) && horizontal > 0){
                lean = -1f;
            }

            transform.Rotate(0, horizontal, lean, Space.Self);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(0,0,-horizontal * 25), 50 * Time.deltaTime);
        }

        // if(horizontal != 0){
        //     Quaternion target = transform.rotation * Quaternion.Euler(transform.rotation.x, transform.rotation.y + horizontal * 100, -horizontal * 30);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 50 * Time.deltaTime);
        // }else{
        //     transform.rotation = transform.rotation * Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.y);
        // }
    }

    private void ApplyGravityForce(Vector3 force, Vector3 forcePosition, Vector3 rayPosition, Vector3 rayDirection){
        RaycastHit hit;

        float height = 0f;

        if(Physics.Raycast(rayPosition, rayDirection, out hit, Mathf.Infinity)){
            height = hit.distance;
            Debug.DrawRay(rayPosition, rayDirection * hit.distance, Color.blue);
        }

        if(height >= 1.7f){
            var diff = height - 1f * 0.1f;
            var vec = new Vector3(0, diff, 0);
            _rigidbody.AddForceAtPosition(-force - vec, rayPosition, mode);
        }else if(height <= 1.4f){
            var diff = 1f - height * 2;
            var vec = new Vector3(0, diff, 0);
            _rigidbody.AddForceAtPosition(force + vec, rayPosition, mode);
        }else{
            _rigidbody.AddForceAtPosition(new Vector3(0, -_rigidbody.velocity.y, 0), transform.position, mode);
        }


    }

}
