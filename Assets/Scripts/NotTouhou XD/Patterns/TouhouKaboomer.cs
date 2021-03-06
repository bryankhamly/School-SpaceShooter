﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouKaboomer : TouhouPattern
{
    [Header ("[Pattern]")]
    [Range (0f, 360f)]
    public float angle = 180f;

    [Header ("[Kaboomer]")]
    public TouhouPattern patternToExplode;
    public float explosionDelay; //When to explode.

    [Header ("[AutoAim]")]
    public bool autoAim;
    public Transform target;

    [Header ("[Effect]")]
    public PooledObject explosionPrefab; //Spawn on explosion

    [Header ("[Follow]")]
    public bool follow;

    private void Start ()
    {
        StartCoroutine(GetMeTarget());
    }

    IEnumerator GetMeTarget ()
    {
        yield return new WaitForSeconds (1);
        if (autoAim)
        {
            target = transform.root.GetComponent<BossHealth> ().target;
        }
    }

    public override void ShootBullet ()
    {
         if (target == null)
        {
            //target = transform.root.GetComponent<BossHealth>().target;
        }

        if (autoAim)
        {
            LookAtTarget ();
        }

        StartCoroutine (Shoot ());

        if (autoAim)
        {
            StartCoroutine (Aim ());
        }
    }

    IEnumerator Shoot ()
    {
        if (!available)
            yield break;

        available = false;

        var bullet = CreateBullet (transform.position, transform.rotation);
        InitBullet (this, bullet, angle, bulletSpeed, bulletAccel, bulletDamage, bulletStrafe);

        available = true;

        yield break;
    }

    void LookAtTarget ()
    {
        if (target)
        {
            angle = TrigStuff.CalculateZAngleDifference (transform.position, target.transform.position);
        }
    }

    IEnumerator Aim ()
    {
        while (autoAim)
        {
            if (available)
            {
                yield break;
            }

            LookAtTarget ();

            yield return 0;
        }
        yield break;
    }
}