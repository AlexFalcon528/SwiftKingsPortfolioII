using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManagerVariables : MonoBehaviour
{
    [Header("~~~~~~~Singleton~~~~~~~")]
    public static StatManagerVariables instance;

    [Range(1, 3)] [SerializeField] public int difficulty;
    private void Awake()
    {
        instance = this;
    }

}
