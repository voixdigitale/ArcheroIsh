using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Manager", menuName = "Scriptable Objects/Game Manager")]
    public class GameManager : ScriptableObject {
    
    public int _currentWave;

    private static GameManager _inst;

    public static GameManager Instance {
        get {
            if (!_inst)
                _inst = Object.FindObjectOfType<GameManager>();
            if (!_inst)
                _inst = CreateInstance<GameManager>();
            return _inst;
        }
    }
}