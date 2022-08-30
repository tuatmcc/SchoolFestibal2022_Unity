using UnityEngine;

public class LogoDisplay : MonoBehaviour
{
    [SerializeField] private Animator logoAnimator;

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (logoAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
        {
            gameObject.SetActive(false);
        }
    }
}
