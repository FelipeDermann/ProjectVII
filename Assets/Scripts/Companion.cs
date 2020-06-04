using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField]
    private Animator anim;
    public Transform playerPoints;
    public Transform target;

    public float normalSpeed;
    public float catchUpSpeed;
    public float decelerationTime;
    public float accelerationTime;
    public float currentSpeed;

    public bool playerInRange;
    public bool canRotate;

    private void OnEnable()
    {
        //PlayerAnimation.SpawnMagicHitbox += PlaySpellAnim;
        MagicBehaviour.StartMagicAnim += PlaySpellAnim;
        //PlayerAnimation.SpawnComboHitbox += PlayComboAnim;
        ComboBehaviour.StartComboAnim += PlayComboAnim;
    }
    private void OnDisable()
    {
        //PlayerAnimation.SpawnMagicHitbox -= PlaySpellAnim;
        MagicBehaviour.StartMagicAnim -= PlaySpellAnim;
        //PlayerAnimation.SpawnComboHitbox -= PlayComboAnim;
        ComboBehaviour.StartComboAnim -= PlayComboAnim;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
        playerPoints = Transform.FindObjectOfType<PlayerUtilities>().companionDestinationPointsParent;

        DecideDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) nav.SetDestination(target.position);

        if (canRotate && nav.velocity.sqrMagnitude < 2) StartCoroutine(nameof(RotateForward));
        anim.SetFloat("Blend", nav.velocity.magnitude);

    }

    void PlayComboAnim()
    {
        anim.SetTrigger("Combo");
    }

    void PlaySpellAnim()
    {
        anim.SetTrigger("Spell");
    }

    void DecideDestination()
    {
        float distanceToClosestPoint = Mathf.Infinity;
        Transform closestPoint = null;

        for (int i = 0; i < playerPoints.childCount; i++)
        {
            Transform currentPoint = playerPoints.GetChild(i).transform;
            float distanceToPoint = (currentPoint.position - this.transform.position).sqrMagnitude;

            if (distanceToPoint < distanceToClosestPoint)
            {
                distanceToClosestPoint = distanceToPoint;
                closestPoint = currentPoint.transform;
            }
        }

        target = closestPoint;
    }

    IEnumerator IncreaseSpeed()
    {
        float time = 0;
        currentSpeed = nav.speed;

        while(time < accelerationTime)
        {
            time += Time.deltaTime;
            float newSpeed = Mathf.Lerp(currentSpeed, catchUpSpeed, time / accelerationTime);
            nav.speed = newSpeed;

            yield return null;
        }

        yield break;
    }

    IEnumerator DecreaseSpeed()
    {
        float time = 0;
        currentSpeed = nav.speed;

        while (time < decelerationTime)
        {
            time += Time.deltaTime;
            float newSpeed = Mathf.Lerp(currentSpeed, normalSpeed, time / accelerationTime);
            nav.speed = newSpeed;

            yield return null;
        }

        yield break;
    }

    IEnumerator RotateForward()
    {
        canRotate = false;

        var direction = target.position - transform.position;

        float dot = 0;
        while (dot < 0.96f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);

            dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);

            yield return null;
        }

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRotate = true;
            playerInRange = true;
            StopCoroutine(nameof(IncreaseSpeed));
            StartCoroutine(nameof(DecreaseSpeed));

            DecideDestination();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            StopCoroutine(nameof(DecreaseSpeed));
            StartCoroutine(nameof(IncreaseSpeed));
        }
    }
}
