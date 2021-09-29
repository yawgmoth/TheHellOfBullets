using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Unit
{
    public enum ControlMode
    {
        TOPDOWN, FIRSTPERSON
    }
    public float speed;
    public float aimspeed;

    // How far out of the center the player can go before the camera starts to follow them
    public float camera_follow;

    // How fast the camera should follow the player
    public float camera_speed;

    public GameObject aimer;

    public ControlMode control_mode;

    public Vector2 camera_target;

    public Vector3 last_position;

    public Text healthtxt;
    // Start is called before the first frame update
    void Start()
    {
        team = Team.PLAYER;
        camera_target = new Vector2(transform.position.x, transform.position.z);
        if (control_mode == ControlMode.FIRSTPERSON)
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        if (Mathf.Abs(dy) > 0.001f || Mathf.Abs(dx) > 0.001f)
        {
            RaycastHit hitInfo;
            Vector3 movement = new Vector3(dx * speed * Time.deltaTime, dy * speed * Time.deltaTime, 0);
            bool hit = Physics.Raycast(transform.position + (transform.rotation*movement.normalized) * transform.localScale.x / 1.9f, (transform.rotation * movement.normalized), out hitInfo, 0.25f);
            if (!hit || !hitInfo.collider.gameObject.CompareTag("Wall"))
                transform.Translate(movement, Space.Self);
            
        }

        if (control_mode == ControlMode.TOPDOWN)
        {
            // We only care about the distance of the camera and the player in the 2D (x,z) plane
            Vector2 cam = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);
            Vector2 plr = new Vector2(transform.position.x, transform.position.z);

            // If the player moves further away than the follow distance, we set a new camera target
            if ((cam-plr).magnitude > camera_follow)
            {
                camera_target = plr;
            }

            // If the camera is not at the target, move it towards that target
            if ((cam - camera_target).magnitude > 0.1f)
            {
                Vector3 follow_direction = new Vector3(camera_target.x - cam.x, 0, camera_target.y - cam.y);
                Camera.main.transform.position += follow_direction.normalized * camera_speed * Time.deltaTime;
            }

            // cursor keys move the aim ball
            dx = Input.GetAxis("AimX");
            dy = Input.GetAxis("AimY");

            if (Mathf.Abs(dy) > 0.001f) aimer.transform.Translate(new Vector3(0, 0, dy * aimspeed * Time.deltaTime), Space.Self);
            if (Mathf.Abs(dx) > 0.001f) aimer.transform.Translate(new Vector3(dx * aimspeed * Time.deltaTime, 0, 0), Space.Self);


            transform.rotation = Quaternion.LookRotation(aimer.transform.position - transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0);
        }
        else
        {
            dx = Input.GetAxis("Mouse X") * aimspeed * 10;
            transform.Rotate(new Vector3(0, 0, -dx * Time.deltaTime));
        }

       

        if (Input.GetButton("Fire1"))
        {
            if (control_mode == ControlMode.TOPDOWN)
            {
                Fire(10, 8, 20, (aimer.transform.position - transform.position).normalized);
            }
            else
            {
                Fire(10, 8, 20, transform.up);
            }
        }

        if (Input.GetButtonDown("ControlMode"))
        {
            SwitchControlMode();
        }

        healthtxt.text = "Health: " + Mathf.RoundToInt(health) + "/" + Mathf.RoundToInt(maxhealth);
    }

    public override void OnDeath()
    {
        Debug.Log("YOU DIE");
    }

    public void SwitchControlMode()
    {
        if (control_mode == ControlMode.TOPDOWN)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, -1.1f, -0.29f);
            Camera.main.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            Camera.main.orthographic = false;
            Cursor.lockState = CursorLockMode.Locked;
            control_mode = ControlMode.FIRSTPERSON;
        }
        else
        {
            Camera.main.transform.SetParent(null);
            Camera.main.transform.position = new Vector3(transform.position.x, 20, transform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
            Camera.main.orthographic = true;
            Cursor.lockState = CursorLockMode.None;
            control_mode = ControlMode.TOPDOWN;
        }
    }
}
