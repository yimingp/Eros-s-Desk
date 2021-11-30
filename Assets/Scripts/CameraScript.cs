using System;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;

    [Title("Setting")]
    public bool canPanCamera = false;
    public bool isFollowing = true;
    public bool showingFollowingLinks = false;
    public float followTime;
    public float zoomSpeed;
    public float zoomMax;
    public float zoomMin;
    public float manualMoveTime;
    
    [Title("Reference")] 
    public CinemachineVirtualCamera cam;
    public UnitsManager manager;
    public UnitRelationshipGUIDrawer linkDrawer;
    public SelectorController cameraSelector;
    
    private float _timeCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _timeCounter = followTime;
    }

    private void LateUpdate()
    {
        if (InputRequestManager.Instance.canRequest)
        {
            var scrollDelta = Input.mouseScrollDelta.y;

            if (scrollDelta != 0)
            {
                cam.m_Lens.OrthographicSize += (scrollDelta > 0) ? -1 * zoomSpeed * Time.deltaTime : zoomSpeed * Time.deltaTime;
                cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, zoomMin, zoomMax);
            }
        }

        if (isFollowing)
        {
            if(cam.Follow == null)
                RandomFollow();
        
            _timeCounter -= Time.deltaTime;

            if (_timeCounter <= 0)
            {
                RandomFollow();
            }
        }
        else
        {
            if (InputRequestManager.Instance.canRequest && canPanCamera && Input.GetMouseButton(0))
            {
                transform.DOMove(Camera.main.ScreenToWorldPoint(Input.mousePosition), manualMoveTime);
            }
        }
    }

    public void ToggleFollowing()
    {
        isFollowing = !isFollowing;
        canPanCamera = !isFollowing;

        if (!isFollowing)
        {
            cam.Follow = null;
            linkDrawer.UndoDrawing();
            cameraSelector.gameObject.SetActive(false);
        }
    }

    public void ToggleFollowingLinks()
    {
        if(!isFollowing) return;

        showingFollowingLinks = !showingFollowingLinks;

        if (showingFollowingLinks)
        {
            if(cam.Follow is null) return;
            linkDrawer.DrawUnitRelations(cam.Follow.gameObject.GetComponent<Unit.Unit>());
        }
        else
        {
            linkDrawer.UndoDrawing();
        }
    }

    [Button]
    public void RandomFollow()
    {
        if(!isFollowing) return;

        _timeCounter = followTime;
        cam.Follow = manager.units.TakeRandomElementFromList().gameObject.transform;
        
        if (showingFollowingLinks)
        {
            linkDrawer.DrawUnitRelations(cam.Follow.gameObject.GetComponent<Unit.Unit>());
        }
        
        cameraSelector.SetFollow(cam.Follow);
        cameraSelector.gameObject.SetActive(true);
    }
}