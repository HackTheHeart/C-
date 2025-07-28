using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void UpdateAnimation(float x, float y, bool walking)
    {
        anim.SetFloat("XInput", x);
        anim.SetFloat("YInput", y);
        anim.SetBool("IsWalking", walking);
    }
    public void PlayEatAnimation()
    {
        anim.SetTrigger("IsEating");
    }
    public void UpdateMood(int mood)
    {
        anim.SetFloat("Mood", mood / 100f);
    }
    public void UpdateFriendship(int friendship)
    {
        anim.SetFloat("Friendship", friendship / 100f);
    }
}
