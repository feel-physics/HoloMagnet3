using System.Collections;
using UnityEngine;
using System;

public class MyHelper : MonoBehaviour
{
    public class MyMonoBehaviour : MonoBehaviour
    {
        public void CallStartCoroutine(IEnumerator iEnumerator)
        {
            StartCoroutine(iEnumerator); //ここで実際にMonoBehaviour.StartCoroutine()を呼ぶ
        }
    }

    // Todo: AsyncAwaitでできる
    private static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    // Todo: AsyncAwaitでできる
    public static void MyDelayMethod(MonoBehaviour i_behaviour, float waitTime, Action action)
    {
        i_behaviour.StartCoroutine(DelayMethod(waitTime, action));
    }

    // ※ILoggableEvery10Seconds
    public static void DebugLogEvery10Seconds(string message, ref bool hasLogged)
    {
        if (DateTime.Now.Second % 10 == 0)
        {
            if (!hasLogged)
            {
                Debug.Log(message);
                hasLogged = true;
            }
        }
        else
        {
            hasLogged = false;
        }
    }
}