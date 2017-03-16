using UnityEngine;
using System.Collections;

public class CameraController2 : MonoBehaviour
{
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(-0.4f, 1.65f, 0f);    //The offset of the camera from the player. Keep default for the best Results.
        public float cameraLookingSmoothTime = 100f;                          //The Time taken by Camera to reach the Destination. Keep Default for best Results.
        public float distanceFromTarget = -6;                          //Distance of camera form the target. Will be modified by other factors like zoom and Camera Collision
        public float zoomStep = 2;
        public float maxZoom = -2;                                     //Max Zoom In distance.
        public float minZoom = -10;                                    //Max Zoom Out distance.
        public float zoomSpeed = 100;
        public bool smoothFollowEnabled = false;
        public float camFollowingSmoothSpeed = 0.05f;

        [HideInInspector]
        public float adjustedDistanceFromTarget = -6;
        [HideInInspector]
        public float newZoomDistance = -6;

    }

    // Class for handling Orbit Settings.
    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;                                   // X-Rotation of the camera.
        public float yRotation = -180;                                  // Y-Rotation of the camera.
        public float maxXRotation = 25;                                 //Max X angle for Rotation.
        public float minXRotation = -85;                                //Min X angle for rRotation.
        public float verticalOrbitSmoothTime = 150;                     //For smoothing camera rotation on respective Axis.
        public float horizontalOrbitSmoothTime = 150;                   //For smoothing camera rotation on respective Axis.
    }

    //Class for Handling camera input.
    [System.Serializable]
    public class InputSettings
    {
        public string HORIZONAT_ORBIT = "OrbitHorizontal";              //Assign input for respective function from Editor.
        public string VERTICAL_ORBIT = "OrbitVertical";                 //Assign input for respective function from Editor.
        public string SNAP_INPUT = "OrbitSnap";                         //Assign input for respective function from Editor.
        public string ZOOM_INPUT = "Mouse ScrollWheel";                 //Assign input for respective function from Editor.
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }


    public PositionSettings position = new PositionSettings();          // Referencing the respective classes.
    public OrbitSettings orbit = new OrbitSettings();                   // Referencing the respective classes.
    public InputSettings input = new InputSettings();                   // Referencing the respective classes.
    public CollisionHandler collisionHandler = new CollisionHandler();  // Referencing the respective classes.
    public DebugSettings debugSettings = new DebugSettings();

    public Transform target;                                            //Reference For target.
    Vector3 targetPos = Vector3.zero;                                   //The position where Camera will be looking.
    Vector3 destination = Vector3.zero;                                 //The Final destination of the camera.
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camSmoothVelocity = Vector3.zero;

    ThirdPersonCharacterController characterController;                 //Reference for Character Controller Script.
    float hOrbitInput, vOrbitInput, zoomInput, snapInput;               //Variables For Getting Input Values.

    void Start()
    {
        SetTarget(target);
        MoveCamera();

        collisionHandler.Initialize(Camera.main);
        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(destination, transform.rotation, ref collisionHandler.desiredCameraClipPoints);
    }

    //Function for getting the Camera Input.
    void GetInput()
    {
        vOrbitInput = Input.GetAxisRaw(input.VERTICAL_ORBIT);
        hOrbitInput = Input.GetAxisRaw(input.HORIZONAT_ORBIT);
        zoomInput = Input.GetAxisRaw(input.ZOOM_INPUT);
        snapInput = Input.GetAxisRaw(input.SNAP_INPUT);
    }

    void Update()
    {
        GetInput();
        ZoomOnTarget();
    }

    void FixedUpdate()
    {
        OrbitAroundTarget();
        collisionHandler.UpdateCameraClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedCameraClipPoints);
        collisionHandler.UpdateCameraClipPoints(destination, transform.rotation, ref collisionHandler.desiredCameraClipPoints);

        for (int i = 0; i < 5; i++)
        {
            if (debugSettings.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collisionHandler.adjustedCameraClipPoints[i], Color.red);
            }
            if (debugSettings.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collisionHandler.desiredCameraClipPoints[i], Color.cyan);
            }
        }
        collisionHandler.CheckCollision(targetPos);
        position.adjustedDistanceFromTarget = collisionHandler.GetAdjustedDistance(targetPos);
    }

    void LateUpdate()
    {
        MoveCamera();       //To move the camera...
        LookAtTarget();     //To Rotate the camera...
    }

    //Function for Getting the target.
    void SetTarget(Transform t)
    {
        target = t;
    }

    //Function for moving the Camera.
    void MoveCamera()
    {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;

        if (collisionHandler.isColliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustedDistanceFromTarget;
            adjustedDestination += targetPos;

            if (position.smoothFollowEnabled)
            {
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camSmoothVelocity, position.camFollowingSmoothSpeed);
            }
            else
                transform.position = adjustedDestination;
        }
        else
        {
            if (position.smoothFollowEnabled)
            {
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camSmoothVelocity, position.camFollowingSmoothSpeed);
            }
            else
                transform.position = destination;
        }
    }

    //Function for Rotating the camera for looking at targret.
    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.cameraLookingSmoothTime * Time.deltaTime);

    }

    //Function for handling the Orbital rotations.
    void OrbitAroundTarget()
    {
        if (snapInput > 0)                                                                      //Resets The Camera Behind The Player if Snap Key is Pressed.
        {                  
            orbit.xRotation = -20f;                
            orbit.yRotation = -180f;               
        }
        orbit.xRotation += vOrbitInput * orbit.verticalOrbitSmoothTime * Time.deltaTime;        //Adding the X rotation with the given input and smoothtime.
        orbit.yRotation += hOrbitInput * orbit.horizontalOrbitSmoothTime * Time.deltaTime;      //Adding the Y rotation with the given input and smoothtime.

        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;                                               //Stoping the camera from going further the max X Rotation.
        }
        else if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;                                               //Stoping the camera from going further the min X Rotation.
        }
    }

    //Function for Controlling the Zooming Of Camera.
    void ZoomOnTarget()
    {
        position.newZoomDistance += position.zoomStep * zoomInput;
        position.distanceFromTarget = Mathf.Lerp(position.distanceFromTarget, position.newZoomDistance, position.zoomSpeed * Time.deltaTime);

        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }

        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisonLayer;             //Layers with which raycast from camera can collide.

        [HideInInspector]                           //To check if there is any collision happening between camera clip points and target.
        public bool isColliding = false;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;   //The Array for desired camera clip points if there is no colision beteen camera and target.
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;  //The Array for adjusted camera clip points after collision.

        Camera camera;                              //Camera Reference.

        //Function For Initializing the required values
        public void Initialize(Camera cam)
        {
            camera = cam;
            desiredCameraClipPoints = new Vector3[5];       // four ClipPoints + Location Of camera itself.
            adjustedCameraClipPoints = new Vector3[5];      // four ClipPoints + Location Of camera itself.
        }

        //Function for Updating the camera clip points.
        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion cameraRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;

            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;                     // z value of camera clip point.
            float x = Mathf.Tan(camera.fieldOfView / 3f) * z;    // x value of camera clip point. As tan(Fov/2)= X/Z... Common Basic Trigonometry. 
            float y = x / camera.aspect;                        // y value of camera clip point. Aspect Ratio = X/Y... This time It`s Basic Algebra.

            //top left clip point
            intoArray[0] = (cameraRotation * new Vector3(-x, y, z)) + cameraPosition;       //Quadrants... And rotated respectively Accordig to Camera`s Rotation.
            //top right clip point                                                          
            intoArray[1] = (cameraRotation * new Vector3(x, y, z)) + cameraPosition;        //Quadrants... And rotated respectively Accordig to Camera`s Rotation.
            //bottom left clip point                                                        
            intoArray[2] = (cameraRotation * new Vector3(-x, -y, z)) + cameraPosition;      //Quadrants... And rotated respectively Accordig to Camera`s Rotation.
            //bottom right clip point                                                       
            intoArray[3] = (cameraRotation * new Vector3(x, -y, z)) + cameraPosition;       //Quadrants... And rotated respectively Accordig to Camera`s Rotation.
            //Camera`s Position
            intoArray[4] = cameraPosition;                       // Position of Camera from a bit behind...
        }

        //function to check if there is anything between camera and the target.
        public bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float rayDistance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, rayDistance, collisonLayer))
                {
                    return true;
                }
            }
            return false;
        }

        public float GetAdjustedDistance(Vector3 fromPosition)
        {
            float distance = -1;
            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, desiredCameraClipPoints[i] - fromPosition);
                float rayDistance = Vector3.Distance(fromPosition, desiredCameraClipPoints[i]);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance, collisonLayer))
                {
                    if (distance == -1)
                    {
                        distance = hit.distance;
                    }
                    else if (hit.distance < distance)
                    {
                        distance = hit.distance;
                    }
                }
            }
            if (distance == -1)
                return 0;
            else
                return distance;
        }

        //function for returning the value of isColliding Parameter.
        public void CheckCollision(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                isColliding = true;
            }
            else
                isColliding = false;
        }
    }
}
