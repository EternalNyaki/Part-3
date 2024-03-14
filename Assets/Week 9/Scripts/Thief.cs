using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Villager
{
    public float dashDistance;

    public GameObject daggerPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    protected override void Attack()
    {
        float targetRadians = Vector2.SignedAngle(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) * Mathf.Deg2Rad;
        Vector2 deltaPosition = new Vector2(Mathf.Cos(targetRadians), Mathf.Sin(targetRadians)) * dashDistance;
        transform.Translate(deltaPosition);
        base.destination = transform.position;

        base.Attack();

        Instantiate(daggerPrefab, spawnPoint1.position, spawnPoint1.rotation);
        Instantiate(daggerPrefab, spawnPoint2.position, spawnPoint2.rotation);
    }

    public override ChestType CanOpen()
    {
        return ChestType.Thief;
    }
}
