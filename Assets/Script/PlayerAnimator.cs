using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Player Player;
    //private const string Is_Walk_Down = "IsWalkDown";
    //private const string Is_Walk_Left = "IsWalkLeft";
    //private const string Is_Walk_Right = "IsWalkRight";
    //private const string Is_Walk_Up = "IsWalkUp";
    private const string Is_Walking = "IsWalking";
    private const string Is_Choping = "IsChopping";
    private const string Is_Mining = "IsMining";
    private const string Is_Hoeing = "IsHoeing";
    private const string Is_Watering = "IsWatering";
    private const string Is_PickingUp = "IsPickingUp";
    private const string Is_PickUpRun = "IsPickUpRun";
    private const string Is_PickUpFruit = "IsPickUpFruit";

    private const string Xinput = "Xinput";
    private const string Yinput = "Yinput";
    //private const string IsFacingRight = "IsFacingRight";
    //private const string IsFacingLeft = "IsFacingLeft";
    //private const string IsFacingDown = "IsFacingDown";

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    //private void LateUpdate()
    //{
    //    //_animator.SetBool(Is_Walk_Down, Player.IsWalkDown());
    //    //_animator.SetBool(Is_Walk_Left, Player.IsWalkLeft());
    //    //_animator.SetBool(Is_Walk_Right, Player.IsWalkRight());
    //    //_animator.SetBool(Is_Walk_Up, Player.IsWalkUp());
    //    _animator.SetBool(Is_Walking, Player.IsWalking());

    //    _animator.SetFloat(Xinput, Player.Xinput());
    //    _animator.SetFloat(Yinput, Player.Yinput());
    //    //_animator.SetBool(IsFacingRight, Player.IsFacingRight());
    //    //_animator.SetBool(IsFacingLeft, Player.IsFacingLeft());
    //    //_animator.SetBool(IsFacingDown, Player.IsFacingDown());
    //}
    //public void PlayChopAnimation()
    //{
    //    _animator.SetTrigger(Is_Choping);
    //}
    private void LateUpdate()
    {
        if (!Player.IsPerformingAction())
        {
            _animator.SetBool(Is_Walking, Player.IsWalking());
            _animator.SetFloat("Xinput", Player.Xinput());
            _animator.SetFloat("Yinput", Player.Yinput());
            _animator.SetBool(Is_PickingUp, Player.IsPickingUp());
            _animator.SetBool(Is_PickUpRun, Player.IsPickUpRun());
            _animator.SetBool("IsSleeping", Player.IsSleeping());
        }
    }
    public void PlayHoeAnimation()
    {
        _animator.SetTrigger(Is_Hoeing);
        int direction = GetPrimaryDirection(Player.Instance.LastInteractDir.x, Player.Instance.LastInteractDir.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    public void PlayWateringAnimation()
    {
        _animator.SetTrigger(Is_Watering);
        int direction = GetPrimaryDirection(Player.Instance.LastInteractDir.x, Player.Instance.LastInteractDir.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    public void PlayChoppingAnimation(Player player)
    {
        StartCoroutine(PerformToolAction(Is_Choping, player.LastInteractDir));
    }
    public void PlayMiningAnimation(Player player)
    {
        StartCoroutine(PerformToolAction(Is_Mining, player.LastInteractDir));
    }
    public void PlayHoeAnimation(Player player)
    {
        StartCoroutine(PerformToolAction(Is_Hoeing, player.LastInteractDir));
    }
    public void PlayWateringAnimation(Player player)
    {
        StartCoroutine(PerformToolAction(Is_Watering, player.LastInteractDir));
    }
    private IEnumerator PerformToolAction(string animationTrigger, Vector2 actionDirection)
    {
        Player.SetPerformingAction(true);
        _animator.SetTrigger(animationTrigger);
        int direction = GetPrimaryDirection(actionDirection.x, actionDirection.y);
        _animator.SetInteger("PrimaryDirection", direction);
        yield return new WaitForSeconds(0.4f);
        Player.SetPerformingAction(false);
    }
    public void PlaySleepAnimation(bool IsSleeping)
    {
        _animator.SetBool("IsSleeping", IsSleeping);
    }
    public void PlayChopAnimation(Vector2 chopDirection)
    {
        _animator.SetTrigger(Is_Choping);
        int direction = GetPrimaryDirection(chopDirection.x, chopDirection.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    public void PlayMineAnimation(Vector2 mineDirection)
    {
        _animator.SetTrigger(Is_Mining);
        int direction = GetPrimaryDirection(mineDirection.x, mineDirection.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    private int GetPrimaryDirection(float x, float y)
    {
        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            return (x > 0) ? 0 : 1; 
        }
        else
        {
            return (y > 0) ? 2 : 3; 
        }
    }
    public void OnActionAnimationEnd()
    {
        Player.SetPerformingAction(false);
    }
    public void PlayAttackAnimation()
    {
        StartCoroutine(PerformActionWithLock("IsAttacking", 0.4f));
    }
    public void PlayDamageAnimation()
    {
        _animator.SetTrigger("IsDamaged");
        int direction = GetPrimaryDirection(Player.Instance.LastInteractDir.x, Player.Instance.LastInteractDir.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    public void PlayDeathAnimation()
    {
        _animator.SetTrigger("IsDead");
        int direction = GetPrimaryDirection(Player.Instance.LastInteractDir.x, Player.Instance.LastInteractDir.y);
        _animator.SetInteger("PrimaryDirection", direction);
    }
    public void SetSleepingState(bool isSleeping)
    {
        _animator.SetBool("IsSleeping", isSleeping);
    }
    private IEnumerator PerformActionWithLock(string animationTrigger, float lockDuration)
    {
        Player.Instance.SetPerformingAction(true);
        _animator.SetTrigger(animationTrigger);
        int direction = GetPrimaryDirection(Player.Instance.LastInteractDir.x, Player.Instance.LastInteractDir.y);
        _animator.SetInteger("PrimaryDirection", direction);
        yield return new WaitForSeconds(lockDuration);
        Player.Instance.SetPerformingAction(false);
    }
    public void PlayPickUpFruitAnimation(Vector2 pickUpDirection)
    {
        StartCoroutine(PerformActionWithLock(Is_PickUpFruit, 0.4f, pickUpDirection));
    }
    private IEnumerator PerformActionWithLock(string animationTrigger, float lockDuration, Vector2 actionDirection)
    {
        Player.Instance.SetPerformingAction(true);
        _animator.SetTrigger(animationTrigger);
        int direction = GetPrimaryDirection(actionDirection.x, actionDirection.y);
        _animator.SetInteger("PrimaryDirection", direction);
        yield return new WaitForSeconds(lockDuration);
        Player.Instance.SetPerformingAction(false);
    }

}
