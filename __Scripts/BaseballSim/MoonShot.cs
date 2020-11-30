using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wysiwyg.Common;

//https://otologic.jp/free/se/game-sports01.html
//https://on-jin.com/sound/spo.php?bunr=%E9%87%8E%E7%90%83&kate=%E3%82%B9%E3%83%9D%E3%83%BC%E3%83%84

//QUICK: 爆炸特效、月光(螢光)bloom
//QUICK: https://www.youtube.com/watch?v=w4aMtKeD2w4
//左鍵集氣
//TODO: 畫面改進(post-processing, graphic setting, 更新modal?)
//TODO: 上傳至github//如何private的上傳(做不到，只好用新帳號)
//https://github.com/Wysiwyg-Interactive/MoonRun.git

//FURTHER TODO: Judge
//FURTHER TODO: 右下角顯示catcher CAMERA
//FURTHER TODO: camera follow ball

//FURTHER TODO: 觀眾fans(先用圓柱)
//XXX: 改善揮棒速度及力量(animation)
//XXX: moonlight, spotlight
//TODO: 真實的球會受擠壓變形
//XXX: 彈性調整: 球場、球、球棒
//XXX: 牛頓打棒球

public class MoonShot : MonoBehaviour {
    public static int level = 0;
    public static readonly int maxLevel = 5;

    public AudioClip mainTheme;
    public AudioClip pitchSound;
    public AudioClip hitSound;
    public AudioClip fanSound;
    public AudioClip ResultSound;

    public static void ShowResultSuccess () {
        BallMonitor ball = FindObjectsOfType<BallMonitor> () [0];
        DisableBat ();
        FindObjectsOfType<MoonShotResultUI> () [0].Show (ball, true);

        GameObject.Find ("Firework").GetComponent<ParticleSystem> ().Play ();
    }

    public static void ShowResultFailed (BallMonitor ball) {
        DisableBat ();
        FindObjectsOfType<MoonShotResultUI> () [0].Show (ball, false);
    }

    public static void DisableBat () {
        FindObjectsOfType<BatController> () [0].Disable ();
    }
    public static void EnableBat () {
        FindObjectsOfType<BatController> () [0].Enable ();
    }

    public static void ThrowBall () {
        BallMonitor ball = FindObjectsOfType<BallMonitor> () [0];
        ball.ThrowBall ();
    }

    public static void Test () { }

    [SerializeField]
    public static Vector3[] levelLocation = new Vector3[] { Vector3.zero, new Vector3 (160,0,-5), new Vector3 (160,0,-5), new Vector3 (150,0,-116), new Vector3 (89,0,-19), new Vector3 (0,30,0) };
    [SerializeField]
    public static Vector3[] levelScale = new Vector3[] { Vector3.zero, new Vector3 (1, 1, 1), new Vector3 (0.6f, 0.6f, 0.6f), new Vector3 (0.7f, 0.7f, 0.7f), new Vector3 (0.6f, 0.6f, 0.6f), new Vector3 (1f, 1f, 1f) };

    public static void PlaceMoon (int level) {
        FindObjectsOfType<MoonMonitor> () [0].transform.localPosition = levelLocation[level];
        FindObjectsOfType<MoonMonitor> () [0].transform.localScale = levelScale[level];
    }

    public static void PlaySound (GameObject go, AudioClip clip = null) { //xxx
        AudioSource source = QUnity.GetOrAddComponent<AudioSource> (go);
        if (clip != null) {
            source.clip = clip;
        }
        source.Play ();
    }

    public static void NextLevel () {
        level++;
        if (level > maxLevel) {
            level = maxLevel;
        }
        Debug.Log ($"now level {level}");
        PlaceMoon (level);
    }

    void Start () {
        QAudio.Play (GetComponent<AudioSource> (), mainTheme);
        NextLevel ();
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.R)) { //QUICK: restart text ui
            MoonShot.Reset ();
        }
        if (Input.GetKeyDown (KeyCode.C)) {
            MoonShot.ThrowBall ();
        }
        if (Input.GetKeyDown (KeyCode.N)) {
            MoonShot.NextLevel ();
        }
        if (Input.GetKeyDown (KeyCode.Z)) {
            MoonShot.Test ();
        }
    }

    public static void ResetBat () {
        FindObjectsOfType<BatController> () [0].Reset ();
    }

    public static void ResetBall () {
        FindObjectsOfType<BallMonitor> () [0].Reset ();
    }

    public static void Reset () {
        FindObjectsOfType<MoonShotResultUI> () [0].Hide();
        MoonShot.ResetBat ();
        MoonShot.ResetBall ();
        //TODO: reset camera
    }
}