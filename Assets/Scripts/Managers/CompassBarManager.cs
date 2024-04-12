using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassBarManager : MonoBehaviour
{
    public RectTransform compassBarTransform;

    // public RectTransform objectiveMarkerTransform;
    public RectTransform northMarkerTransform;
    public RectTransform northeastMarkerTransform;
    public RectTransform eastMarkerTransform;
    public RectTransform southeastMarkerTransform;
    public RectTransform southMarkerTransform;
    public RectTransform southwestMarkerTransform;
    public RectTransform westMarkerTransform;
    public RectTransform northwestMarkerTransform;

    public Transform cameraObjectTransform;
    public Transform objectiveObjectTransform;

    private Camera playerCam;

    private void Start()
    {
        playerCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 1000);
        SetMarkerPosition(northeastMarkerTransform, (Vector3.forward + Vector3.right) * 1000 );
        SetMarkerPosition(southMarkerTransform, Vector3.back * 1000);
        SetMarkerPosition(southeastMarkerTransform, (Vector3.back + Vector3.right) * 1000);
        SetMarkerPosition(eastMarkerTransform, Vector3.right * 1000);
        SetMarkerPosition(southwestMarkerTransform, (Vector3.back + Vector3.left) * 1000);
        SetMarkerPosition(westMarkerTransform, Vector3.left * 1000);
        SetMarkerPosition(northwestMarkerTransform, (Vector3.forward + Vector3.left) * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        //Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;

        //float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));

        //float compassPositionX = Mathf.Clamp(2 * angle / playerCam.fieldOfView, -1, 1);

        //markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);

        Vector3 toDirection = cameraObjectTransform.InverseTransformPoint(worldPosition).normalized;

        // Check if the target is behind the camera. If so, don't show the marker.
        if (toDirection.z < 0)
        {
            markerTransform.gameObject.SetActive(false); // Hides the marker
            return; // Exit the function early as we don't want to process further.
        }

        // Calculate the angle to the target
        float angle = Vector2.SignedAngle(new Vector2(toDirection.x, toDirection.z), Vector2.up);

        // Check if the angle is within the field of view
        bool withinFieldOfView = Mathf.Abs(angle) < playerCam.fieldOfView / 2;

        // Set marker position or make it inactive based on whether it is within the field of view
        if (withinFieldOfView)
        {
            float compassPositionX = angle / (playerCam.fieldOfView / 2); // Normalize angle to [-1, 1] based on camera FOV

            // Set marker's position on the compass bar
            markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);

            markerTransform.gameObject.SetActive(true); // Shows the marker
        }
        else
        {
            markerTransform.gameObject.SetActive(false); // Hides the marker
        }
    }
}
