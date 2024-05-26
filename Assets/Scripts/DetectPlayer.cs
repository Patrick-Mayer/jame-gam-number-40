using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject enemyObj;

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject == playerObj){
            enemyObj.GetComponent<EnemyScript>().nonAggressive = false;
        }
    }
};