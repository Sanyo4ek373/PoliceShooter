﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsSolid;

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up,  whatIsSolid);
        
        if (hitInfo.collider != null)
        {
            Destroy(gameObject);
        }
    }
}
