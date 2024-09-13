using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gui_Controller : MonoBehaviour
{

    public Animator animator;

    public Toggle leftLegToggle;
    public Toggle rightLegToggle;
    public Toggle PreAlignmentToggle;
    public Toggle PostAlignmentToggle;

    public GameObject Volumebtn;
    public GameObject playbackSpeedDropdown;
    public GameObject playPauseGO;
    private bool isPlaying = false;

    private float[] speeds = { 1f, 0.5f, 2f };
    private Vector3 targetPelvisPos = new Vector3( 780f, 215f, 267f);
    private Vector3 rightLegCamPos = new Vector3(803f, 195f, 280.200012f);
    private Vector3 leftLegCamPos = new Vector3(765.900024f, 195f, 280.200012f);


    public GameObject SkeleRig;
    public GameObject SkelePelvis;
    private Vector3 SkeleOriginPoint;
    private Quaternion SkeleOriginRotation;
    public ProjectToPlane ProjectToPlane;
    public bool leftLeg = true;
    public bool preAlign = true;
    private Matrix4x4 originMatrix;
    public Camera preTraceCam;
    public Camera postTraceCam;
    private bool clearTraces = false;
    private bool clearPost = false;
    private bool clearPre = false;
    void Start()
    {

        Button volBtn = Volumebtn.GetComponent<Button>();
        volBtn.onClick.AddListener(fireRotate);
        animator.speed = 0;
        Button playPauseButton = playPauseGO.GetComponent<Button>();
        // Initialize Play/Pause Button
        playPauseButton.onClick.AddListener(TogglePlayPause);
        TMP_Dropdown playbackSpeed = playbackSpeedDropdown.GetComponent<TMP_Dropdown>();
        playbackSpeed.onValueChanged.AddListener(SetPlayBackSpeed);

        PostAlignmentToggle.onValueChanged.AddListener(UpdateAnimationState);
        PreAlignmentToggle.onValueChanged.AddListener(UpdateAnimationState);
        leftLegToggle.onValueChanged.AddListener(UpdateAnimationState);
        rightLegToggle.onValueChanged.AddListener(UpdateAnimationState);

        SkeleOriginPoint = SkelePelvis.transform.localPosition;

        SkeleOriginRotation = SkelePelvis.transform.localRotation;
        

    }




    public void fireRotate()
    {
        //Debug.Log(Quaternion.FromToRotation(SkelePelvis.transform.up, Vector3.forward).eulerAngles.ToString());
        //SkeleRig.transform.rotation *= Quaternion.FromToRotation(SkelePelvis.transform.up, Vector3.forward);
        //SkeleRig.transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        //SkeleRig.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Vector3 rotToPerform = new Vector3( 0f, Quaternion.FromToRotation(SkelePelvis.transform.up, Vector3.forward).eulerAngles.y, 0f );
        SkeleRig.transform.rotation *= Quaternion.Euler(rotToPerform);
        

        //Debug.Log(Quaternion.FromToRotation(SkelePelvis.transform.up, Vector3.forward).eulerAngles.ToString());

        Vector3 globalPosOffset = SkelePelvis.transform.position - targetPelvisPos;
        globalPosOffset.y = 0f;
        SkeleRig.transform.position -= globalPosOffset;

        if (clearTraces)
        {
            ProjectToPlane.clearTraces();
            clearTraces = false;
        }
        if (clearPre)
        {
            ProjectToPlane.clearPreTrace();
            clearPre = false;
        }
        if (clearPost)
        {
            ProjectToPlane.clearPostTrace();
            clearPost = false;
        }
    }
    void LoadAnimation(string animationName)
    {
        //ProjectToPlane.plotMarker = false;
        animator.Play(animationName);
    
    }
    void UpdateAnimationState(bool change)
    {
        ProjectToPlane.plotMarker = false;
        string currClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if ((currClip.Contains("Left") && rightLegToggle.isOn) || (currClip.Contains("Right") && leftLegToggle.isOn))
        {
            animator.SetBool("clearTraces", true);
            //clearTraces = true;
        }
        // Switch from post to pre, clear pre
        if (PreAlignmentToggle.isOn && !preAlign) {
            animator.SetBool("clearPreTrace", true);
            clearPre = true;
        }
        if (PostAlignmentToggle.isOn && preAlign)
        {
            animator.SetBool("clearPostTrace", true);
            clearPost = true;
        }
        if ((rightLegToggle.isOn && leftLeg) || (leftLegToggle.isOn && !leftLeg))
        {
            clearTraces = true;
        }


        // Set cameras to match left/right position:
        if (leftLegToggle.isOn)
        {
            preTraceCam.transform.transform.position = leftLegCamPos;
            postTraceCam.transform.transform.position = leftLegCamPos;
        } else
        {
            preTraceCam.transform.transform.position = rightLegCamPos;
            postTraceCam.transform.transform.position = rightLegCamPos;
        }
        if (PreAlignmentToggle.isOn)
        {
            preAlign = true;
            if (leftLegToggle.isOn)
            {
                
                leftLeg = true;
                animator.Play("LeftBalancePre");
            }
            else
            {
                leftLeg = false;
                animator.Play("RightBalancePre");
            }
        } else
        {

            preAlign=false;
            if (leftLegToggle.isOn)
            {
                leftLeg = true;
                animator.Play("LeftBalancePost");
            } else
            {
                leftLeg = false;
                animator.Play("RightBalancePost");
            }
        }

    }
    void SetPlayBackSpeed(int speed)
    {
        animator.speed = speeds[speed];
    }
    void TogglePlayPause()
    {
        isPlaying = !isPlaying;
        playPauseGO.GetComponentInChildren<TextMeshProUGUI>().SetText(isPlaying ? "II" : "►");
        if (isPlaying) {
            animator.speed = speeds[playbackSpeedDropdown.GetComponent<TMP_Dropdown>().value];
            ProjectToPlane.plotMarker = true;
        } else {
            animator.speed = 0;
            ProjectToPlane.plotMarker = false;
        };
    }

}
