using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    //Vector3 targetPos;
    float laneOffset = 3f;
    public float laneChangeSpeed = 15;
    Quaternion startGamerotation;
    Vector3 startGamePosition;
    Vector3 targetVelocity;
    public float Speed =10;
    float pointStart;
    float pointFinish;
    bool isMoving = false;
    Coroutine movingCoroutine;
    public GameObject UI;
    public GameObject GroundCheck;
    private bool isGround = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)&& isGround)
        {
            Jump();
            isGround = false;
        }
   
        if (Input.GetKeyDown(KeyCode.A)&& pointFinish >-laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }

        if (Input.GetKeyDown(KeyCode.D) && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }


       
    }

    void MoveHorizontal(float speed)
    {
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;
        if (isMoving) { StopCoroutine(movingCoroutine); isMoving = false; }
        movingCoroutine = StartCoroutine(MoveCoroutinue(speed));
        targetVelocity = new Vector3(-laneChangeSpeed, 0, 0);
    }

    void Jump()
    {
        Debug.Log("Test");
        rb.AddForce(Vector3.up * 10f,ForceMode.Impulse);
    }
   
    IEnumerator MoveCoroutinue(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart- transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        isMoving = false;
    }


   public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        UI.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeadPlayer")
        {
            UI.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("DeadPlayer");
        }
        if (GroundCheck.gameObject.tag == "Ground")
        {
            isGround = true;
            Debug.Log("Test2222");
        }
    }

   
}
