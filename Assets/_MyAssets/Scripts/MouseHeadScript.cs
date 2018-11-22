using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHeadScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(transform.parent.gameObject);
    }
}
