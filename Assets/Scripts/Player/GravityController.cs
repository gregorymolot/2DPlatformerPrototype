using System.Collections;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityController : MonoBehaviour
{
    Rigidbody2D rb2D;
    Vector2 gravityDirection {get { return new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * (gravityAngle - 90)), (float)Mathf.Sin(Mathf.Deg2Rad * (gravityAngle-90))).normalized;}}
    [SerializeField]
    float gravityAmount;
    public bool doneChanging {get; private set;}
    [SerializeField]
    float gravityAngle;

    public Vector2 gravity {get { return gravityDirection * gravityAmount;}}

    public static GravityController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("No Controller");
            }
            return _instance;
        }
    }

    private static GravityController _instance;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void SetGravity(float baseAmount)
    {
        doneChanging = true;
        gravityAmount = baseAmount;
    }

    public bool CheckGravity(float amount)
    {
        return gravityAmount == amount;
    }

    public void SetGravity(Vector2 direction, float baseAmount)
    {
        doneChanging = true;
        gravityAngle = Vector2.Angle(Vector2.down, direction);
        gravityAmount = baseAmount;
    }

    public void SetGravity(Vector2 direction, float baseAmount, float endAmount, float timeToamount)
    {
        doneChanging = false;
        gravityAngle = Vector2.Angle(Vector2.down, direction);
        gravityAmount = baseAmount;
        StartCoroutine(ChangeGravity(baseAmount, endAmount, timeToamount));
    }

    public void SetGravity(float baseAmount, float endAmount, float timeToamount)
    {
        doneChanging = false;
        gravityAmount = baseAmount;
        StartCoroutine(ChangeGravity(baseAmount, endAmount, timeToamount));
    }

    public void SetGravity(float angle, float baseAmount)
    {
        doneChanging = true;
        gravityAngle = angle;
        gravityAmount = baseAmount;
    }

    public void SetGravity(float angle, float baseAmount, float endAmount, float timeToamount)
    {
        doneChanging = false;
        gravityAngle = angle;
        gravityAmount = baseAmount;
        StartCoroutine(ChangeGravity(baseAmount, endAmount, timeToamount));
    }

    IEnumerator ChangeGravity(float baseAmount, float end, float time)
    {
        float timer = 0f;
        if (time < Time.fixedDeltaTime)
        {
            gravityAmount = end;
            timer = time;
        }
        while (timer<time && doneChanging == false)
        {
            gravityAmount = Mathf.Lerp(baseAmount, end, timer/time);
            timer+=Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        doneChanging = true;
    }

    void FixedUpdate()
    {
        rb2D.MoveRotation(gravityAngle);

    }
}
