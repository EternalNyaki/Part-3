using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float polesBuildTime = 1f;
    public float lineBuildTime = 1f;
    public float clothesBuildTime = 1f;
    public float clipsBuildTime = 1f;

    public GameObject rightPole;
    public GameObject leftPole;

    public GameObject clothesline;

    public GameObject cloth1;
    public GameObject cloth2;
    public GameObject cloth3;

    public GameObject clip1;
    public GameObject clip2;
    public GameObject clip3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
        yield return StartCoroutine(BuildPoles());

        yield return StartCoroutine(BuildLine());

        yield return StartCoroutine(BuildClothes());

        yield return StartCoroutine(BuildClips());
    }

    private IEnumerator BuildPoles()
    {
        rightPole.GetComponent<SpriteRenderer>().enabled = true;
        leftPole.GetComponent<SpriteRenderer>().enabled = true;

        Transform rPole = rightPole.transform;
        Transform lPole = leftPole.transform;
        rPole.localScale = new Vector3(1, 0, 1);
        lPole.localScale = new Vector3(-1, 0, 1);

        //I think this might be the least for-loop-like for loop ever lmao
        //A while loop is probably better for readability here but this is too funny not to do
        for(float a = 0; a < 1; a += Time.deltaTime / polesBuildTime)
        {
            rPole.localScale = new Vector3(1, a, 1);
            lPole.localScale = new Vector3(1, a, 1);

            yield return null;
        }

        rPole.localScale = Vector3.one;
        lPole.localScale = Vector3.one;
    }

    private IEnumerator BuildLine()
    {
        yield return new WaitForSeconds(lineBuildTime / 2);

        clothesline.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(lineBuildTime / 2);
    }

    private IEnumerator BuildClothes()
    {
        cloth1.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(clothesBuildTime / 2);

        cloth2.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(clothesBuildTime / 2);

        cloth3.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator BuildClips()
    {
        clip1.GetComponent<SpriteRenderer>().enabled = true;

        Transform transform = clip1.transform;
        Vector3 startingPosition = transform.localPosition;
        transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);

        while (transform.localPosition.y > startingPosition.y)
        {
            transform.localPosition -= new Vector3(0, Time.deltaTime / (clipsBuildTime / 3), 0);

            yield return null;
        }

        clip2.GetComponent<SpriteRenderer>().enabled = true;

        transform = clip2.transform;
        startingPosition = transform.localPosition;
        transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);

        while (transform.localPosition.y > startingPosition.y)
        {
            transform.localPosition -= new Vector3(0, Time.deltaTime / (clipsBuildTime / 3), 0);

            yield return null;
        }

        clip3.GetComponent<SpriteRenderer>().enabled = true;

        transform = clip3.transform;
        startingPosition = transform.localPosition;
        transform.localPosition = transform.localPosition + new Vector3(0, 1, 0);

        while (transform.localPosition.y > startingPosition.y)
        {
            transform.localPosition -= new Vector3(0, Time.deltaTime / (clipsBuildTime / 3), 0);

            yield return null;
        }
    }
}
