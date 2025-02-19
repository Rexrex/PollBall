using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PoolBall : MonoBehaviour
{

    [Header("Ball Appearance")]
    public Color baseColor;
    public bool ChangeColorOnCollision;
    public Color actualBallColor;

    [Header("Rendering")]
    private SpriteRenderer _spriteRenderer;

    [Header("Audio")]
    public AudioClip[] CollisionSounds;
    private AudioSource _audioSource;

    [Header("Ball Physics & Behavior")]
    public float colorChangeDuration = 0.3f;
    bool _markedToDie = false;
    public float KillTimer = 2.0f;
    public float fadeOutDuration = 2.0f;  // Time it takes to fade out

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = CollisionSounds[0];

        // Get the Renderer component and store the original color
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            //Grabbing the original color
            actualBallColor = _spriteRenderer.color;
            //Setting ball to white
            _spriteRenderer.color = baseColor;
        }
        else
        {
            Debug.LogError("No Renderer found on this GameObject!");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (ChangeColorOnCollision)
        {
            StopAllCoroutines(); // Stop any existing color change coroutine
            StartCoroutine(ChangeColorTemporarily());
        }

        PlayRandomCollisionSound();
    }

    private IEnumerator ChangeColorTemporarily()
    {
        ChangeColor();

        // Wait for the specified duration
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert to the original color
        ResetColor();
    }


    public void ChangeColor()
    {
        // Change to the hit color
        if(_spriteRenderer != null)
            _spriteRenderer.color = actualBallColor;
    }

    public void ResetColor()
    {
        // Revert to the original color
        if (_spriteRenderer != null)
            _spriteRenderer.color = baseColor;
    }

    public void PlayRandomCollisionSound()
    {
        if (CollisionSounds.Length == 0 || _audioSource == null) return;

        int rand = Random.Range(0, CollisionSounds.Length);
        _audioSource.PlayOneShot(CollisionSounds[rand]);
    }


    public void Dissolve()
    {
        if (!_markedToDie)
        {
            _markedToDie = true;
            StartCoroutine(DissolveRoutine());
        }
    }

    private IEnumerator DissolveRoutine()
    {

        float timer = 0f;
        Color startColor = actualBallColor;
        Vector3 startScale = transform.localScale;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            // Fade-out effect
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);

            // Make sure to show original color
            _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Shrinking effect
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / fadeOutDuration);

            yield return null; 
        }

        Destroy(gameObject);
    }
}
