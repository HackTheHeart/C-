using System.Collections;
using UnityEngine;

public class FishingAnimation : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private GameObject Fishing;
    private static readonly int IsFishing = Animator.StringToHash("IsFishing");
    private static readonly int IsHooked = Animator.StringToHash("IsHooked");
    private static readonly int IsRolling = Animator.StringToHash("IsRolling");
    private static readonly int CatchFish = Animator.StringToHash("CatchFish");
    private static readonly int PrimaryDirection = Animator.StringToHash("PrimaryDirection");
    public Animator _animator;
    public static FishingAnimation Instance;
    private float originalMinY, originalMaxY;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _animator = GetComponent<Animator>();
    }
    private void HandleMiniGameEnd(bool success)
    {
        Debug.Log("Animation nhận sự kiện, phát animation phù hợp");
        _animator.SetBool(CatchFish, success);
        _animator.SetBool(IsRolling, false);
        StartCoroutine(WaitForAnimationToEnd());
    }
    private IEnumerator WaitForAnimationToEnd()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Animation kết thúc, phát sự kiện cho Manager");
        ResetAnimation();
        FishingManager.Instance.ResetFishingState();
        Player.SetPerformingAction(false);
    }
    public void PlayFailedFishingAnimation()
    {
        StartCoroutine(FailedFishingAnimation());
    }
    private IEnumerator FailedFishingAnimation()
    {
        Player.SetPerformingAction(true);
        _animator.SetTrigger(IsFishing);
        _animator.SetInteger(PrimaryDirection, GetPrimaryDirection(Player.LastInteractDir));
        yield return new WaitForSeconds(1.3f);
        _animator.SetBool("FailFishing", true);
        Player.SetPerformingAction(false);
        _animator.SetBool("FailFishing",false);
    }
    public void StartFishing()
    {
        StartCoroutine(PerformFishingAction(IsFishing, Player.LastInteractDir, 1.3f));
    }
    public IEnumerator PerformFishingAction(int animationTrigger, Vector2 actionDirection, float waitTime)
    {
        Player.SetPerformingAction(true);
        _animator.SetTrigger(animationTrigger);
        _animator.SetInteger(PrimaryDirection, GetPrimaryDirection(actionDirection));
        yield return new WaitForSeconds(waitTime);
        if (animationTrigger == IsFishing)
        {
            _animator.SetBool(IsFishing, false);
        }
        else if (animationTrigger == IsRolling)
        {
            FishingMiniGame.Instance.OnMiniGameEnd += HandleMiniGameEnd;
        }
    }
    public IEnumerator WaitForHook()
    {
        Player.SetPerformingAction(false);
        float hookTime = Random.Range(2f, 5f);
        float timer = 0f;
        while (timer < hookTime)
        {
            if (Player.IsWalking())
            {
                FishingManager.Instance.CancelFishing();
                FishingManager.Instance.ResetFishingState();
                Player.SetPerformingAction(false);
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        float reactionTime = 3f;
        timer = 0f;
        Fishing.SetActive(true);
        _animator.SetBool(IsHooked, true);
        while (timer < reactionTime)
        {
            
            Player.SetPerformingAction(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //_animator.SetBool(IsHooked, false);
                _animator.SetBool(IsRolling, true);
                Fishing.SetActive(false);
                FishingManager.Instance.TryStartRolling();
                StartCoroutine(PerformFishingAction(IsRolling, Player.LastInteractDir, 3f));
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        _animator.SetBool(IsHooked, false);
        FishingManager.Instance.ResetFishingState();
        Player.SetPerformingAction(false);
    }
    private bool DetermineCatchResult()
    {
        return FishingMiniGame.Instance.GetProgress() >= 1f;
    }
    public void ResetAnimation()
    {
        _animator.SetBool(IsFishing, false);
        _animator.SetBool(IsHooked, false);
        _animator.SetBool(IsRolling, false);
        _animator.SetBool(CatchFish, false);
    }
    private int GetPrimaryDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            return (direction.x > 0) ? 0 : 1;
        else
            return (direction.y > 0) ? 2 : 3;
    }
    public void PlayCatchFishAnimation()
    {
        _animator.SetBool(CatchFish, true);
        StartCoroutine(ResetAnimation(nameof(CatchFish)));
    }
    public void PlayNoFishAnimation()
    {
        _animator.SetBool(CatchFish, false);
        StartCoroutine(ResetAnimation(nameof(CatchFish)));
    }
    private IEnumerator ResetAnimation(string parameter)
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetBool(parameter, false);
    }
}
