using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 5.0f;
    public float gravity = 1000000.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Rotation mit Pfeiltasten
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 turn = new Vector3(0, moveHorizontal, 0);
            characterController.transform.Rotate(turn * Time.deltaTime * speed * 10);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 turn = new Vector3(moveVertical * -1, 0, 0);
            characterController.transform.Rotate(turn * Time.deltaTime * speed * 10);
        }

        //Bewegung mit WASD
        if (WasdIsPressed())
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
        else
        {
            moveDirection = new Vector3(0, 0, 0);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        transform.rotation = q;


        //Bewegung mit Joystick
        //RightJoystick rightJoystick = GameObject.FindGameObjectWithTag("joystick").GetComponent<RightJoystick>();
        //Vector3 rightJoystickInput = rightJoystick.GetInputDirection();
        //float xMovementRightJoystick = rightJoystickInput.x;
        //float zMovementRightJoystick = rightJoystickInput.y;

        //if (rightJoystickInput != Vector3.zero)
        //{
        //    rightJoystickInput = new Vector3(xMovementRightJoystick, 0, zMovementRightJoystick);
        //    rightJoystickInput = transform.TransformDirection(rightJoystickInput);
        //    rightJoystickInput *= moveSpeed;
        //    characterController.Move(rightJoystickInput * Time.fixedDeltaTime);
        //}

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            //Move forward and back
            characterController.Move(transform.forward * deltaMagnitudeDiff * -0.005f );

            characterController.Move(transform.right * (touchZero.deltaPosition.x + touchOne.deltaPosition.x) * -0.0025f);
            characterController.Move(transform.forward * (touchZero.deltaPosition.y + touchOne.deltaPosition.y) * -0.0025f);

        }
    }

    private bool WasdIsPressed()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);
    }
}
