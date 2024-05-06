using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

// public class CameraController : MonoBehaviour
// {
//     private Camera mainCamera;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Find the main camera in the scene
//         mainCamera = Camera.main;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Move the camera forward when the "Move Forward" button is pressed
//         if (Input.GetKey(KeyCode.W))
//         {
//             mainCamera.transform.position += mainCamera.transform.forward * Time.deltaTime;
//         }

//         // Move the camera backward when the "Move Backward" button is pressed
//         if (Input.GetKey(KeyCode.S))
//         {
//             mainCamera.transform.position -= mainCamera.transform.forward * Time.deltaTime;
//         }

//         // Rotate the camera to the left when the "Rotate Left" button is pressed
//         if (Input.GetKey(KeyCode.A))
//         {
//             mainCamera.transform.Rotate(Vector3.up, -45f * Time.deltaTime);
//         }

//         // Rotate the camera to the right when the "Rotate Right" button is pressed
//         if (Input.GetKey(KeyCode.D))
//         {
//             mainCamera.transform.Rotate(Vector3.up, 45f * Time.deltaTime);
//         }
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private bool isMovingForward;
    private bool isMovingBackward;
    private bool isRotatingLeft;
    private bool isRotatingRight;

    public Button moveForwardButton;
    public Button moveBackwardButton;
    public Button rotateLeftButton;
    public Button rotateRightButton;

    // Start is called before the first frame update
    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;

        // // Add listeners to button events
        // moveForwardButton.onClick.AddListener(OnMoveForwardButtonDown);
        // moveBackwardButton.onClick.AddListener(OnMoveBackwardButtonDown);
        // rotateLeftButton.onClick.AddListener(OnRotateLeftButtonDown);
        // rotateRightButton.onClick.AddListener(OnRotateRightButtonDown);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera based on button states
        if (isMovingForward)
        {
            mainCamera.transform.position += mainCamera.transform.forward * 4 * Time.deltaTime;
        }
        if (isMovingBackward)
        {
            mainCamera.transform.position -= mainCamera.transform.forward * 4 * Time.deltaTime;
        }
        if (isRotatingLeft)
        {
            mainCamera.transform.Rotate(Vector3.up, -60f * Time.deltaTime);
        }
        if (isRotatingRight)
        {
            mainCamera.transform.Rotate(Vector3.up, 60f * Time.deltaTime);
        }
    }

    // Handler for the pointer down event on the move forward button
    public void OnMoveForwardButtonDown()
    {
        isMovingForward = true;
    }

    public void OnMoveForwardButtonUp()
    {
        isMovingForward = false;
    }

    // Handler for the pointer down event on the move backward button
    public void OnMoveBackwardButtonDown()
    {
        isMovingBackward = true;
    }

    // Handler for the pointer down event on the move backward button
    public void OnMoveBackwardButtonUp()
    {
        isMovingBackward = false;
    }

    // Handler for the pointer down event on the rotate left button
    public void OnRotateLeftButtonDown()
    {
        isRotatingLeft = true;
    }

    // Handler for the pointer down event on the rotate left button
    public void OnRotateLeftButtonUp()
    {
        isRotatingLeft = false;
    }

    // Handler for the pointer down event on the rotate right button
    public void OnRotateRightButtonDown()
    {
        isRotatingRight = true;
    }

    // Handler for the pointer down event on the rotate right button
    public void OnRotateRightButtonUp()
    {
        isRotatingRight = false;
    }

}
