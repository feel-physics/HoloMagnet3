using System.Collections;
using UnityEngine;
using System;

public static class MyHelper
{
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

    public static Pole[] ToPoleArray(Transform[] transforms)
    {
        var poles = new Pole[transforms.Length];
        for (var index = 0; index < poles.Length; index++)
        {
            poles[index] = new Pole { position = transforms[index].position };
        }
        return poles;
    }

    public static bool CopyPoleArray(Pole[] outPoles, Transform[] transforms)
    {
        if( outPoles.Length != transforms.Length ){
            return false;
        }
        for( int i = 0; i < outPoles.Length; i ++ ){
            outPoles[i].position = transforms[i].position;
        }

        return true;
    }

}