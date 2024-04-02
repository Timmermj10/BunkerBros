using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{

    private PopUpSystem popUpSystem;
    private GameObject bunker;

    public GameObject basicEnemyPrefab;
    public GameObject armoredEnemyPrefab;

    void Start()
    {

        popUpSystem = GameObject.Find("TutorialPopUpSystem").GetComponent<PopUpSystem>();
        bunker = GameObject.Find("Objective");

        StartCoroutine(Tutorial());

        //popUpSystem.popUp("Manager", "The system is working! wrap text wrap text wrap text");



    }

    IEnumerator Tutorial()
    {
        yield return new WaitForFixedUpdate();

        Debug.Log("Damaging Bunker");
        bunker.GetComponent<HasHealth>().changeHealth(-10);
        bunker.GetComponent<HasHealth>().changeHealth(-50);


        Instantiate(basicEnemyPrefab, new Vector3(-2, 1, 0), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(1, 1, 1.5f), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(1, 1, 1.5f), Quaternion.identity);


        yield return null;
    }

}
