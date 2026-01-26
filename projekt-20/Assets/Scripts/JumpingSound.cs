using UnityEngine;

public class JumpSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip landClip;

    public CharacterController controller; // použij CharacterController
    private bool wasGrounded = true;

    void Update()
    {
        // Zkontroluj pøistání
        if (!wasGrounded && controller.isGrounded)
        {
            PlayLandSound();
        }

        // Zkontroluj skok
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            PlayJumpSound();
        }

        wasGrounded = controller.isGrounded;
    }

    private void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private void PlayLandSound()
    {
        if (landClip != null)
        {
            audioSource.PlayOneShot(landClip);
        }
    }
}
