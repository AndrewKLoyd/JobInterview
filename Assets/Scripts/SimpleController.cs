using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SimpleController : MonoBehaviour
{
    [Header("Velocity of current sphere")]
    [SerializeField] private float movingVelocity = 10f;


    [Header("Aceleration of sphere (g)")]
    [Range(0.1f, 20f)]
    [SerializeField] private float g;

    [Header("Time the death will be played")]
    [Range(1, 5)]
    [SerializeField] private int deathShowTime;

    [Header("Follow camera")]
    [SerializeField] private GameObject followCamera;


    [Header("Camera offset")]
    [Range(1, 5)]
    [SerializeField] private float cameraOffset;

    [Header("Death animation")]
    [SerializeField] private Animator deathAnimation;


    public int DificultyLvl
    {
        set
        {
            if (value == 1)
            {
                movingVelocity += 5f;
            }
            if (value == 2)
            {
                movingVelocity += 10f;
            }

        }
    }

    public Text ScoreText
    {
        set
        {
            scoreText = value;
        }
    }



    private float yVelocity;

    private float startYPos;

    private int totalDistance;

    private Text scoreText;

    private Button upButton;

    bool falling = true;

    public static event System.Action<int> GameOver;


    void Start()
    {
        if (followCamera == null)
        {
            followCamera = FindObjectOfType<Camera>().gameObject;
        }
        startYPos = transform.position.y;
        upButton = FindObjectOfType<Button>();
        EventTrigger trigger = upButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) =>
        {
            if (falling)
            {
                falling = !falling;
                yVelocity = 0;
            }
        });
        trigger.triggers.Add(entry);

        trigger = upButton.gameObject.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) =>
        {
            if (!falling)
            {
                falling = !falling;
                yVelocity = 0;
            }
        });
        trigger.triggers.Add(entry);



    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (falling)
            {
                falling = !falling;
                yVelocity = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!falling)
            {
                falling = !falling;
                yVelocity = 0;
            }
        }
        PerformGravity();
        SetupCameraPosition();
        MoveForward();
    }


    private void MoveForward()
    {
        transform.position += Vector3.forward * Time.deltaTime * movingVelocity;

        if (scoreText != null)
        {
            scoreText.text = ((int)transform.position.z).ToString();
        }
    }


    private void PerformGravity()
    {
        if (falling)
        {
            yVelocity = yVelocity + g * Time.deltaTime;
            transform.position += Vector3.down * yVelocity * Time.deltaTime;
        }
        else
        {
            yVelocity = yVelocity + g * Time.deltaTime;
            transform.position += Vector3.up * yVelocity * Time.deltaTime;
        }

    }

    private void SetupCameraPosition()
    {
        followCamera.transform.position = new Vector3(followCamera.transform.position.x,
                                            followCamera.transform.position.y, transform.position.z + cameraOffset);
    }


    private void OnCollisionEnter(Collision collision)
    {
        g = 0;
        movingVelocity = 0;
        yVelocity = 0;
        Destroy(GetComponent<Rigidbody>());
        StartCoroutine("DeathAnimation");
        GetComponent<Animator>().SetTrigger("FireUp");
        GameOver?.Invoke((int)transform.position.z);


    }


    private void OnAnimationEnd()
    {

    }

    private IEnumerator DeathAnimation()
    {
        long startTime = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        for (; ; )
        {
            if ((System.DateTimeOffset.Now.ToUnixTimeSeconds() - startTime) > deathShowTime)
            {
                Destroy(gameObject);
                break;
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, startYPos, transform.position.z), 0.01f);


            yield return new WaitForEndOfFrame();
        }
    }

}
