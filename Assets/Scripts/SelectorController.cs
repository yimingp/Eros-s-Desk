using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SelectorController : MonoBehaviour
{
    [Title("Setting")]
    public float effectSpeed;
    
    [Title("Data")]
    public Transform follow;

    public void SetFollow(Transform t)
    {
        follow = t;
        transform.position = follow.position;
    }

    private void Update()
    {
        if (follow == null)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.position = follow.position;
    }

    private void OnEnable()
    {
        TurnOnEffect();
    }

    private void TurnOnEffect()
    {
        transform.DOScale(new Vector3(1.25f, 1.25f, 1), effectSpeed).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
