using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float globalTimer = 0f;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        globalTimer += Time.deltaTime;
    }
}
