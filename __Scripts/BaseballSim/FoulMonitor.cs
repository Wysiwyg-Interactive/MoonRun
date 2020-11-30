using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        DetectFoul(collision);
    }

    private void DetectFoul(Collision collision)
    {
        if (IsFoul())
        {
            MoonShot.ShowResultFailed(GetComponent<Collider>().gameObject.GetComponent<BallMonitor>());
        }
    }

    private bool IsFoul()
    {
        return false; //TODO:
    }
}