using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip walkingSound;
    private AudioSource audioSource;

    [SerializeField] private float chaseRange = 11f; // The distance at which the enemy starts chasing the player
    [SerializeField] private float deathRange = 2.5f; // The distance at which the enemy kills the player

    private bool isChasingPlayer = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        if (audioSource == null) Debug.LogError("No AudioSource component found!");
        if (agent == null) Debug.LogError("No NavMeshAgent component found!");
    }

    private void Update()
    {
        // Calculate the distance between the enemy and the player
        float distance = Vector3.Distance(player.transform.position, transform.position);

        ChasePlayer(distance);
        PlayerDeath(distance);
        HandleSounds();
    }

    private void PlayerDeath(float distance)
    {
        if (distance < deathRange)
        {
            player.SetGameFailed(true);
            animator.SetBool("shouldAttack", true);
            agent.isStopped = true; // stop movement when attacking
        }
        else
        {
            animator.SetBool("shouldAttack", false);
            agent.isStopped = false;
        }
    }

    private void ChasePlayer(float distance)
    {
        if (distance < chaseRange)
        {
            isChasingPlayer = true;
            agent.SetDestination(player.transform.position); // let NavMeshAgent handle pathfinding
            animator.SetBool("isRunning", true);
        }
        else
        {
            isChasingPlayer = false;
            agent.ResetPath(); // stop moving if out of range
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
