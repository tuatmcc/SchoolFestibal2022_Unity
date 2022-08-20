using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attach this to the Image object
// This script will stop running after it is disactivated.

public class CountDownDisplay : MonoBehaviour
{
    [SerializeField] private RaceManger RManager;
    [SerializeField] private Sprite Number_1;
    [SerializeField] private Sprite Number_2;
    [SerializeField] private Sprite Number_3;

    private Image CountDownImage;


    void Start()
    {
        TryGetComponent(out CountDownImage);
    }

    void Update()
    {
        if (RManager.StartTime <= 0) CountDownImage.gameObject.SetActive(false);
        else if (RManager.StartTime <= 1) CountDownImage.sprite = Number_1;
        else if (RManager.StartTime <= 2) CountDownImage.sprite = Number_2;
        else if (RManager.StartTime <= 3) CountDownImage.sprite = Number_3;
    }
}
