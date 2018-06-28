using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class Map : Singleton<Map>
{

    [SerializeField] private int widht, height;
    [SerializeField] private int distanceBetweeenLevels;
    [SerializeField] private GameObject cell;
    [SerializeField] private GameObject ramp;
    [SerializeField] private GameObject quarry;
    [SerializeField] private GameObject forest;
    [SerializeField] private GameObject _base;
    [SerializeField] private GameObject water;

    [SerializeField] private ConstructionMode constructor;
    [SerializeField] private ConstructionMode AIconstructor;
    [SerializeField] private Color color;


    private GameObject[] map;

    private List<GameObject[]> LevelMaps;

    public int[,] cellsMap = new int[,] {
          
         {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, 
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1, 0,-0,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1, 0,-0,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1, 0,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
  
    };

    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 
    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    //{1, 1, 2, 2, 2, 2, 2, 2, 2, 1 },
    //{1, 3, 0, 0, 0, 0, 0, 0, 0, 4 },
    //{1, 3, 0, 0, 0, 0, 0, 0, 0, 4 },
    //{1, 1, 5, 5, 5, 5, 5, 5, 5, 1 },
    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    //{1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    //};
    public int[,] buildingsMap = new int[,] {
         { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1,-1,-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-0,-0,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 101, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-0,-0,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 201, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 202, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 102, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 101, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 201, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 101, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 100, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 200, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 201, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 201, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 6, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 0, 8, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,10, 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 101, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 202, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1, 0, 0,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 102, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 101, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1, 0,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 201, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-0,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
         { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}, //80
                                    };
    public int[,] randomMap = new int[50, 50];
    public int[,] cellularAutomaton;

    public float[,] metaballMap;


    void Start()
    {
        //for (int i = 0; i < 80; i++)
        //{
        //    for (int j = 0; j < 40; j++)
        //    {
        //        cellsMap[i, j] = 1;
        //    }

        //}
        LevelMaps = new List<GameObject[]>
        {
            new GameObject[widht * height],
            new GameObject[widht * height],
            new GameObject[widht * height]
        };

        //createMap();
        realParseMap();
        GetComponent<NavMeshSurface>().BuildNavMesh();


    }



    public void destroyMap()
    {
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < widht; column++)
            {
                GameObject.Destroy(map[row * widht + column].gameObject);
            }
        }

        cellularAutomaton = new int[widht, height];
    }



    public void createMap()
    {
        map = new GameObject[widht * height];
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < widht; column++)
            {
                map[row * widht + column] = Instantiate(cell, new Vector3(column * 8, 0, row * 8), cell.transform.rotation, this.transform);
                map[row * widht + column].GetComponent<Cell>().setCoords(column, row);
                if ((column + row) % 2 == 0)
                    map[row * widht + column].GetComponent<Renderer>().material.color = new Color(0.5f, 1, 0, 1);
                else
                    map[row * widht + column].GetComponent<Renderer>().material.color = new Color(0, 1, 0.5f, 1);

            }
        }
    }

    public void realParseMap()
    {
        LevelMaps = new List<GameObject[]>
        {
            new GameObject[widht * height],
            new GameObject[widht * height],
            new GameObject[widht * height]

        };

        Cell tempCell;
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < widht; column++)
            {
                switch (cellsMap[column, row])
                {
                    case 0:
                        //altura baja
                        LevelMaps[0][row * widht + column] = Instantiate(cell, new Vector3(column * 8, transform.position.y - distanceBetweeenLevels, row * 8), cell.transform.rotation, this.transform);
                        tempCell = LevelMaps[0][row * widht + column].GetComponent<Cell>();
                        tempCell.Level = 0;
                        tempCell.setCoords(column, row);
                        tempCell.name = "c" + column.ToString() + " " + row.ToString();
                        tempCell.ProductionBoost = 1.5f;
                        //altura alta
                        LevelMaps[1][row * widht + column] = null;

                        LevelMaps[2][row * widht + column] = null;
                        break;

                    case 1:
                        //altura baja
                        LevelMaps[0][row * widht + column] = null;

                        //altura alta
                        LevelMaps[1][row * widht + column] = Instantiate(cell, new Vector3(column * 8, transform.position.y, row * 8), cell.transform.rotation, this.transform);
                        tempCell = LevelMaps[1][row * widht + column].GetComponent<Cell>();
                        tempCell.Level = 1;
                        tempCell.setCoords(column, row);
                        tempCell.name = "c" + column.ToString() + " " + row.ToString();
                        tempCell.ProductionBoost = 1;

                        LevelMaps[2][row * widht + column] = null;
                        break;

                    case 2:
                        //altura baja
                        LevelMaps[0][row * widht + column] = null;

                        //altura alta
                        LevelMaps[1][row * widht + column] = null;


                        LevelMaps[2][row * widht + column] = Instantiate(cell, new Vector3(column * 8, transform.position.y + distanceBetweeenLevels, row * 8), cell.transform.rotation, this.transform);
                        tempCell = LevelMaps[2][row * widht + column].GetComponent<Cell>();
                        tempCell.Level = 2;
                        tempCell.setCoords(column, row);
                        tempCell.name = "c" + column.ToString() + " " + row.ToString();
                        tempCell.ProductionBoost = 1;
                        break;


                    case 3:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 50, row * 8), Quaternion.Euler(0, 180, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;

                    case 4:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 58, row * 8), Quaternion.Euler(0, 180, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 5:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 42, row * 8), Quaternion.Euler(0, 180, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 6:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 34, row * 8), Quaternion.Euler(0, 180, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 7:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 42, row * 8), Quaternion.Euler(0, 0, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 8:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 34, row * 8), Quaternion.Euler(0, 0, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 9:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 58, row * 8), Quaternion.Euler(0, 0, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    case 10:
                        LevelMaps[0][row * widht + column] = null;
                        LevelMaps[1][row * widht + column] = null;
                        LevelMaps[2][row * widht + column] = null;

                        tempCell = Instantiate(ramp, new Vector3(column * 8, transform.position.y + 50, row * 8), Quaternion.Euler(0, 0, 0), this.transform).GetComponent<Cell>();
                        tempCell.setCoords(column, row);
                        break;
                    default:
                        break;
                }
            }
        }

        StartCoroutine(InstantiateBuildings());
        StartCoroutine(changeCellsColor());
    }


    /// <summary>
    /// Call througt coroutine because of need for initialization of other objects before placing objects of class building.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateBuildings()
    {
        yield return new WaitForEndOfFrame();

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < widht; column++)
            {
                Cell cell;
                Vector2Int dimension;

                switch (buildingsMap[column, row])
                {
                    case 100:
                        cell = getCell(column, row);
                        dimension = _base.GetComponent<Building>().getDimensions();
                        constructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), _base, _base.transform.rotation, orientation.South);
                        break;
                    case 200:
                        cell = getCell(column, row);
                        dimension = _base.GetComponent<Building>().getDimensions();
                        AIconstructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), _base, _base.transform.rotation, orientation.South);
                        break;
                    case 101:
                        cell = getCell(column, row);
                        dimension = forest.GetComponent<Building>().getDimensions();
                        constructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), forest, forest.transform.rotation, orientation.South);
                        break;
                    case 201:
                        cell = getCell(column, row);
                        dimension = forest.GetComponent<Building>().getDimensions();
                        AIconstructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), forest, forest.transform.rotation, orientation.South);
                        break;
                    case 102:
                        cell = getCell(column, row);
                        dimension = quarry.GetComponent<Building>().getDimensions();
                        constructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), quarry, quarry.transform.rotation, orientation.South);
                        break;
                    case 202:
                        cell = getCell(column, row);
                        dimension = quarry.GetComponent<Building>().getDimensions();
                        AIconstructor.placeBuilding(cell, cell.CalculateConstructionPlace(dimension), quarry, quarry.transform.rotation, orientation.South);
                        break;

                }
            }
        }

        InitBases();
    }


    IEnumerator changeCellsColor()
    {
        yield return new WaitForEndOfFrame();
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < widht; column++)
            {
                if (row >= height / 2)
                {
                    if (getCell(column, row) != null)
                    {
                        (getCell(column, row).gameObject.GetComponentInChildren(typeof(Renderer)) as Renderer).material.color = color;
                        getCell(column, row).setGameModeManager(AIconstructor.getGameModeManager());
                    }
                }

                else
                {
                    if (getCell(column, row) != null)
                        getCell(column, row).setGameModeManager(constructor.getGameModeManager());

                }

            }
        }
    }

    public void InitBases()
    {
        Base[] bases = FindObjectsOfType(typeof(Base)) as Base[];
        foreach (Base _base in bases)
        {
            foreach (Base __base in bases)
            {
                if (_base.getPlayer() != __base.getPlayer())
                {
                    _base.getPlayer().setRivalBase(__base);
                }
            }   
        }
        water.SetActive(true);
    }





    /*
     metaball =                  r^2   
                    ______________________________

                    (   (x-x0)^2  +  (y-y0)^2    )


*/


    public int getWidth()
    {
        return widht;
    }
    public int getHeight()
    {
        return height;
    }

    public Cell getCell(int column, int row)
    {
        if (LevelMaps[0][row * widht + column] != null)
            return LevelMaps[0][row * widht + column].GetComponent<Cell>();
        else if (LevelMaps[1][row * widht + column]!= null)
            return LevelMaps[1][row * widht + column].GetComponent<Cell>();
        else if (LevelMaps[2][row * widht + column] != null)
            return LevelMaps[2][row * widht + column].GetComponent<Cell>();

        return null;
    }

    public Cell getCell(int column, int row, int level)
    {
        return LevelMaps[level][row * widht + column].GetComponent<Cell>();

        //return map[row * widht + column].GetComponent<Cell>();
    }


    public Cell getRightCell(int column, int row)
    {
        return map[row * widht + (column + 1)].GetComponent<Cell>();
    }
    public Cell getUpCell(int column, int row)
    {
        return map[(row + 1) * widht + column].GetComponent<Cell>();
    }
    public Cell getLeftCell(int column, int row)
    {
        return map[row * widht + (column - 1)].GetComponent<Cell>();
    }
    public Cell getDownCell(int column, int row)
    {
        return map[(row - 1) * widht + column].GetComponent<Cell>();
    }

    public Cell getUpRightCell(int column, int row)
    {
        return map[(row + 1) * widht + (column + 1)].GetComponent<Cell>();
    }
    public Cell getDownLeftCell(int column, int row)
    {
        return map[(row - 1) * widht + (column - 1)].GetComponent<Cell>();
    }
    public Cell getUpLeftCell(int column, int row)
    {
        return map[(row + 1) * widht + (column - 1)].GetComponent<Cell>();
    }
    public Cell getDownRightCell(int column, int row)
    {
        return map[(row - 1) * widht + (column + 1)].GetComponent<Cell>();
    }


    public bool cellExist(int column, int row)
    {
        if (column >= 0 && row >= 0 && column < widht && row < height)
        {
            if (map[row * widht + column] != null)
                return true;
            else
                return false;
        }
        return false;
    }

    public bool cellExist(Cell cell, int column, int row)
    {
        GameObject[] cells = LevelMaps[cell.Level];

        if (column >= 0 && row >= 0 && column < widht && row < height)
        {
            if (cells[row * widht + column] != null)
                return true;
            else
                return false;
        }
        return false;
    }


}




//[CustomEditor(typeof(Map))]
//public class MapGenerator : Editor
//{

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        Map map = (Map)target;
//        if (GUILayout.Button("Generate Map"))
//        {
//            map.realParseMap();
//        }

//        if (GUILayout.Button("Cellular Automaton"))
//        {

        
//        }
//    }
//}
