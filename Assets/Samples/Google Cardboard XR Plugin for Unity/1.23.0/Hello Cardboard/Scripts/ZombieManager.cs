using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private PlayerManager player; // Reference to the player's transform
    [SerializeField] private Animator animator; // Reference to the enemy's Animator component

    [SerializeField] private AudioClip idleSound; // Idle sound for the zombie
    [SerializeField] private AudioClip walkingSound; // Walking sound for the zombie
    private AudioSource audioSource; // Audio source for playing sounds

    [SerializeField] private float moveSpeed = 0.73f; // The enemy's move speed
    [SerializeField] private float rotationSpeed = 5f; // The speed at which the enemy rotates
    [SerializeField] private float chaseRange = 11f; // The distance at which the enemy starts chasing the player
    [SerializeField] private float deathRange = 2.5f; // The distance at which the enemy kills the player

    private bool isChasingPlayer = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found! Please add one to the Zombie GameObject.");
        }
    }

    private void Update()
    {
        // Calculate the distance between the enemy and the player
        float distance = Vector3.Distance(player.transform.position, transform.position);

        ChasePlayer(distance); // Logic for chasing the player
        PlayerDeath(distance); // Check if the player is caught
        HandleSounds(); // Manage sound playback based on zombie state
    }

    private void PlayerDeath(float distance)
    {
        // Reload the scene if the zombie gets too close
        if (distance < deathRange)
        {
            player.SetGameFailed(true);
            animator.SetBool("shouldAttack", true);
        }
        else
        {
            animator.SetBool("shouldAttack", false);
        }
    }

    private void ChasePlayer(float distance)
    {
        // If the player is within chase range, move and play the running sound
        if (distance < chaseRange)
        {
            isChasingPlayer = true;

            Vector3 movementVector = player.transform.position - transform.position;
            movementVector.y = 0;

            transform.Translate(Time.deltaTime * moveSpeed * movementVector.normalized, Space.World);

            // Calculate the rotation towards the player
            Quaternion lookRotation = Quaternion.LookRotation(movementVector);

            // Smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Set running animation
            animator.SetBool("isRunning", true);
        }
        else
        {
            isChasingPlayer = false;

            // If out of range, set idle animation
            animator.SetBool("isRunning", false);
        }
    }

    private void HandleSounds()
    {
        if (isChasingPlayer)
        {
            if (audioSource.clip != walkingSound || !audioSource.isPlaying)
            {
                audioSource.clip = walkingSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != idleSound || !audioSource.isPlaying)
            {
                audioSource.clip = idleSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }
}
