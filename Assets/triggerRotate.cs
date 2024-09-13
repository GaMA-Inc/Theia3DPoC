using UnityEngine;
using System.Collections;

public class triggerRotate : MonoBehaviour
{
    Animator animator;
    public Gui_Controller controller;
    public ProjectToPlane projectToPlane;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void fireRealign()
    {
        Debug.Log("Fired");
        controller.fireRotate();
    }

    public void checkForClear()
    {
        if (animator.GetBool("clearTraces"))
        {
            projectToPlane.clearTraces();
            animator.SetBool("clearTraces", false);
        }

        if (animator.GetBool("clearPreTrace"))
        {
            projectToPlane.clearPreTrace();
            animator.SetBool("clearPreTrace", false);
        }
        if (animator.GetBool("clearPostTrace"))
        {
            projectToPlane.clearPostTrace();
            animator.SetBool("clearPostTrace", false);
        }
    }
}