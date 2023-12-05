using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

    public class GameManager : MonoBehaviour {
    
    public int CurrentWave;
    public int CurrentBulletSpeed;
    public GameObject Player;
    public static GameManager Instance;

    private void Awake() {
        Instance = this;
    }
}