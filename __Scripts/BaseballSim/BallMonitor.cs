using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Wysiwyg.Common;

public class BallMonitor : MonoBehaviour {
    private bool isHit;
    private QTransform startTransform;

    private TMP_Text distanceText;
    private TMP_Text angleText;
    private TMP_Text stateText;

    public Vector3 fakeVelocity = new Vector3 (-0.3f, 0.15f, 0);
    public float meterPerUnit = 2f;

    public State state;
    public enum BallState {
        Pitch,
        Hit,
        Stop
    }
    public class State {
        public BallMonitor ball;
        public BallState state;

        public State (BallMonitor ball) {
            this.ball = ball;
        }
    }

    public class PitchState : State {
        public PitchState (BallMonitor ball) : base (ball) {
            state = BallState.Pitch;
            //ball.ThrowBall ();
        }
        ~ PitchState () {
            //ball.StopThrowBall ();
        }
    }

    public class HitState : State {
        public HitState (BallMonitor ball) : base (ball) {
            state = BallState.Hit;
        }
    }
    public class StopState : State {
        public StopState (BallMonitor ball) : base (ball) {
            Debug.Log ($"Ball is stopped!");
            state = BallState.Stop;
            MoonShot.ShowResultFailed (ball);
        }
    }

    private Coroutine pitchRoutine;
    public void ThrowBall () {
        pitchRoutine = StartCoroutine (ThrowBallRoutine ());
    }
    public void StopThrowBall () {
        StopCoroutine (pitchRoutine);
    }

    private IEnumerator ThrowBallRoutine () {
        while (true) {
            while (state.state != BallState.Pitch) {
                yield return new WaitForSeconds (0.1f);
            }
            Debug.Log ("Throw ball");
            Reset ();
            GetComponent<TrailRenderer> ().emitting = false;
            //放在投手丘
            transform.localPosition = new Vector3 (8933, -1849, -11.4f);
            yield return new WaitForSeconds (0.5f);
            //投向好球帶
            GetComponent<Rigidbody> ().useGravity = true;
            GetComponent<Rigidbody> ().AddForce (new Vector3 (21f, 0, 0), ForceMode.Impulse);

            GetComponent<TrailRenderer> ().emitting = true;
            Debug.Log ("Throw ball ended");
            yield return new WaitForSeconds (3);
        }
    }

    // Start is called before the first frame update
    void Start () {
        startTransform = new QTransform (this.gameObject);

        distanceText = QUnity.FindGameObject ("distance").GetComponent<TMP_Text> ();
        angleText = QUnity.FindGameObject ("angle").GetComponent<TMP_Text> ();
        stateText = QUnity.FindGameObject ("state").GetComponent<TMP_Text> ();

        Reset ();
    }

    // Update is called once per frame
    void FixedUpdate () {
        stateText.text = state.state.ToString ();
        if (state.state == BallState.Hit) {
            if (!isHit) {
                GetComponent<Rigidbody> ().useGravity = true;
                Debug.Log ($"球初速 {GetComponent<Rigidbody>().velocity}");
                Q.Delay (() => CalculateAngle (), 0.1f);
                AddOnHitForce ();
                isHit = true;
            }

            //    Unity's scale by default, because of the physics engine, is one Unit = 1 meter.
            //    However, The scale can be whatever you like. If you want your imported mesh to be in meters,
            //    You will have to make sure it is scaled correctly, either in your modelling app,
            //   or in Unity's FBX Importer

            //Debug.Log($"Ball Moves! {position} {transform.position}");
            float distance = meterPerUnit * Vector3.Distance (transform.position, startTransform.position);
            distanceText.text = "Distance " + (distance).ToString ("0.0") + " M";

            if (isHit && IsBallStopped ()) {
                state = new StopState (this);
            }
        }
    }

    public bool IsBallStopped () {
        return GetComponent<Rigidbody> ().IsSleeping () || transform.position.y < -3;
    }

    private Vector3 angleVelocity;
    public void CalculateAngle () {
        Debug.Log ($"擊球向量 {transform.position - startTransform.position}");
        angleText.text = "Launch Angle " + Vector3.Angle (new Vector3 (-1, 0, 0), transform.position - startTransform.position).ToString ("0.0") + " degree";
        angleVelocity = (transform.position - startTransform.position).normalized;
    }

    public int power = 10;
    public void AddOnHitForce () {
        //打到的力量不夠，手動增加
        GetComponent<Rigidbody> ().AddForce (angleVelocity * power, ForceMode.Impulse);
    }

    public void Reset () {
        Debug.Log ("Reset ball");
        GetComponent<TrailRenderer> ().Clear ();
        GetComponent<Rigidbody> ().useGravity = false;
        startTransform.Set (this.gameObject);
        isHit = false;

        Stop ();

        state = new PitchState (this);

        angleText.text = "";
        distanceText.text = "";
    }

    public override string ToString () {
        return $"{angleText.text}\n\n{distanceText.text}";
    }

    public void Stop () {
        GetComponent<Rigidbody> ().velocity = Vector3.zero;
        GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
    }

    public AudioClip swingClip;
    private void OnCollisionEnter (Collision collision) {
        Debug.Log ($"Ball collision with {collision.collider.tag}");
        if (collision.collider.tag == "Bat") {
            MoonShot.PlaySound (collision.collider.gameObject, swingClip);
            state = new HitState (this);
        }
    }
}