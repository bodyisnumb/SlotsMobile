using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotValue
{
    Crown,
    Diamond,
    Seven,
    Cherry,
    Bar
}

public class Slot : MonoBehaviour
{
    private int randomValue;
    [HideInInspector] public float timeInterval;
    private float speed;
    public SlotValue stoppedSlot;
    private SlotMachine sm;

    private void Start()
    {
        sm = gameObject.GetComponentInParent<SlotMachine>();
    }

    public IEnumerator Spin()
    {
        timeInterval = sm.timeInterval;
        randomValue = Random.Range(0, 90);
        speed = 300f + randomValue;
        while (speed >= 10f)
        {
            speed -= Time.deltaTime * 50;
            transform.Translate(Vector2.up * Time.deltaTime * -speed);
            if (transform.localPosition.y <= -2.1f)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, 3.4f);
            }
            yield return null;
        }
        StartCoroutine("EndSpin");
        yield return null;
    }

    private IEnumerator EndSpin()
    {
        while (speed >= 2f)
        {
            if (transform.localPosition.y < -0.38f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, -1.3f), speed * Time.deltaTime);
                if (Mathf.Abs(transform.localPosition.y - (-1.3f)) < 0.1f)
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < 0.6f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, -0.38f), speed * Time.deltaTime);
                if (Mathf.Abs(transform.localPosition.y - (-0.38f)) < 0.1f)
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < 1.53f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, 0.6f), speed * Time.deltaTime);
                if (Mathf.Abs(transform.localPosition.y - 0.6f) < 0.1f)
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < 2.5f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, 1.53f), speed * Time.deltaTime);
                if (Mathf.Abs(transform.localPosition.y - 1.53f) < 0.1f)
                {
                    speed = 0;
                }
            }
            else if (transform.localPosition.y < 3.4f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, 2.5f), speed * Time.deltaTime);
                if (Mathf.Abs(transform.localPosition.y - 2.5f) < 0.1f)
                {
                    speed = 0;
                }
            }
            speed = speed / 1.01f;
            yield return new WaitForSeconds(timeInterval);
        }
        speed = 0;
        CheckResults();
        yield return null;
    }

    private void CheckResults()
    {
        float tolerance = 0.35f; // Adjust tolerance as needed

        if (Mathf.Abs(transform.localPosition.y - (-1.3f)) < tolerance)
        {
            stoppedSlot = SlotValue.Crown;
        }
        else if (Mathf.Abs(transform.localPosition.y - (-0.38f)) < tolerance)
        {
            stoppedSlot = SlotValue.Diamond;
        }
        else if (Mathf.Abs(transform.localPosition.y - 0.6f) < tolerance)
        {
            stoppedSlot = SlotValue.Seven;
        }
        else if (Mathf.Abs(transform.localPosition.y - 1.53f) < tolerance)
        {
            stoppedSlot = SlotValue.Cherry;
        }
        else if (Mathf.Abs(transform.localPosition.y - 2.5f) < tolerance)
        {
            stoppedSlot = SlotValue.Bar;
        }

        sm.WaitResults();
    }
}
