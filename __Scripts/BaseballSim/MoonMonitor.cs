using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wysiwyg.UI;

public class MoonMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Ball")
        {
            Explode();
            MoonShot.ShowResultSuccess();
        }
    }

    public void Explode()//Rigidbody.AddExplosionForce
    {
        //GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1, 3.0F);

    }

    public void Show()
    {
        QUI.Show(this.gameObject);
    }

    public void Hide()
    {
        QUI.Hide(this.gameObject);
    }
}