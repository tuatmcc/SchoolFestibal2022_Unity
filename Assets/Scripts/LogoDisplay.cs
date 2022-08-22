using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoDisplay : MonoBehaviour
{
    [SerializeField] private Animator LogoAnimator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LogoAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
        {
            gameObject.SetActive(false);
        }
    }
}
