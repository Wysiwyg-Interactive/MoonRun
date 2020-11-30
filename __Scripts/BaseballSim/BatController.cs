using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wysiwyg.Common;

public class BatController : MonoBehaviour {
    public Vector3 cursorOffset;
    public bool showMousePos = false;
    private bool isSwing;
    public Vector3 batForce;
    public Vector3 batTorque;
    public float swingDuration;
    private QTransform startTransform;
    public bool enabled;
    public string swingMethod;
    private QTransform originalTransform;

    public bool useAnimationSwing;
    public bool usePhysicsSwing;

    void Start () {
        startTransform = new QTransform (this.gameObject);
        GetComponent<Rigidbody> ().maxAngularVelocity = 90;

        Enable ();

        cursorOffset = new Vector3 (0.1f, -0.2f, 0.05f);
        batForce = new Vector3 (-100, 0, 0);
        batTorque = new Vector3 (200, -1000, 400);
        swingDuration = 0.25f;
        //FollowCursor();
    }

    public void Enable () {
        enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Disable () {
        enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Stop () {
        GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody> ().velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isSwing) {
            Stop ();
            FollowCursor ();
        }
    }

    private void FollowCursor () {
        var position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.4f));
        Vector3 newPos = new Vector3 (position.x + cursorOffset.x, position.y + cursorOffset.y, position.z + cursorOffset.z);
        GetComponent<Rigidbody> ().MovePosition (newPos);

        if (showMousePos) {
            Debug.Log ("mouse position: " + Input.mousePosition);
            Debug.Log ("bat follow mouse position: " + newPos);
        }
    }

    void Update () {
        if (enabled && !isSwing) {
            if (Input.GetMouseButtonDown (0)) {
                Debug.Log ("Swing");
                StartCoroutine (SwingRoutine ());
            }
        }
    }

    private IEnumerator SwingRoutine () {
        isSwing = true;
        originalTransform = new QTransform (this.gameObject);


        if (useAnimationSwing) {
            AnimationSwing ();
        } else if (usePhysicsSwing) {
            yield return PhysicsSwing ();
        }
    }


    private void AnimationSwing () {
        GetComponent<Animator> ().SetTrigger ("Swing");
        //EditorApplication.isPaused = true;
    }

    private IEnumerator PhysicsSwing () {
        //TODO: use Rigidbody.MoveRotation instead
        GetComponent<Rigidbody> ().AddForce (batForce);
        GetComponent<Rigidbody> ().AddRelativeTorque (batTorque, ForceMode.Acceleration); //旋轉
        //-y 90度 z 25度
        yield return new WaitForSeconds (swingDuration);
        originalTransform.Set (this.gameObject);
        AfterSwing ();
    }

    public void Reset () {
        startTransform.Set (this.gameObject);
        Enable ();
    }

    public void AfterSwing () {
        //Stop the bat and recover position
        Stop ();
        isSwing = false;
        originalTransform.Set (this.gameObject);
    }
}