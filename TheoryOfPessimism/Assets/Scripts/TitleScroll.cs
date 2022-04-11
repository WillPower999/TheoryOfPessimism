using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScroll : MonoBehaviour
{
    public Vector3 translation;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Translate(translation * Time.deltaTime);

        //gameObject.transform.
    }
}
