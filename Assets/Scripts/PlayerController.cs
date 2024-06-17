using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10;
    private Animator playerAnim;
    private Rigidbody playerRB;
    public bool isOnGround = true;
    public float gravityModifier = 1.0f;
    public bool gameOver = false;
    public ParticleSystem explosionParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;

    public ParticleSystem dirtParticle;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        
        if (Mathf.Abs(Physics.gravity.y - 9.81f) < 0.01f) // Checa se é próximo da gravidade padrão
        {
            Physics.gravity *= gravityModifier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            if (!gameOver)
            {
                dirtParticle.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    // Método para resetar o estado do jogador
    public void ResetPlayer()
    {
        isOnGround = true;
        gameOver = false;
        playerAnim.SetBool("Death_b", false);
        playerAnim.SetInteger("DeathType_int", 0);
        explosionParticle.Stop();
        dirtParticle.Play();
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
