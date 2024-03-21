using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Thief : Villager
{
    public GameObject daggerPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    Coroutine dashing;

    protected override void Attack()
    {
        if(dashing != null)
        {
            StopCoroutine(dashing);
        }
        dashing = StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {

        destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        speed = 7f;

        while(speed > 3)
        {
            yield return null;
        }

        base.Attack();

        yield return new WaitForSeconds(9/60);
        Instantiate(daggerPrefab, spawnPoint1.position, spawnPoint1.rotation);
        yield return new WaitForSeconds(13/60);
        Instantiate(daggerPrefab, spawnPoint2.position, spawnPoint2.rotation);
    }

    public override ChestType CanOpen()
    {
        return ChestType.Thief;
    }
}
