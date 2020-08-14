using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
//using UnityEngine.EventSystems;
//using Liminal.SDK.VR.EventSystems;


//IPointerClickHandler to detect pointer click
public class ControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10f;
        Ray myray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(transform.position, forward, Color.red);

        if(Physics.Raycast(myray, out hit, 100f))
        {    
            if(hit.collider != null )
            {
                //hit.collider.gameObject.GetComponent<LadleAnimation>().invokeAnimation();
                print(hit.collider.tag);
            }
        }
    }*/
    void Update()
    {
        var vrDevice = VRDevice.Device;

        if(vrDevice == null)
            return;

        var hitResult = vrDevice.PrimaryInputDevice.Pointer.CurrentRaycastResult;
        if(hitResult.gameObject.CompareTag("Ladle"))
        {
            print("Ladle");
        }

    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        var raycast_result = eventData.pointerCurrentRaycast;
        print(raycast_result.gameObject.tag);
    }*/
}
