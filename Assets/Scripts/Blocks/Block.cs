using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    protected virtual void Start()
    {

    }

    public void Blast()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        // Blast();
    }
}
