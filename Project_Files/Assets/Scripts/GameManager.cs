using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public Transform startPoint;
    public Transform[] path;
    public Transform pickleCharge;

    // As soon as the game starts, set the game manager to this script basically
    // Every other script will be able to access it
    private void Awake() {
        manager = this;
    }
    
}
