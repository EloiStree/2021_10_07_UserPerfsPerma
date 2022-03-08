using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushScoreToCurrentUserCacheMono : MonoBehaviour
{
    public string m_scoreLabel = "score";
    public string m_bestScoreLabel = "bestscore";


    public void PushScore(float newScore)
    {
       CurrentUserPermaPrefSingleton.GetUserReference(out UserPermaPrefMono userInfo);
       userInfo.User.GetPrimtiveStringFloat(in m_bestScoreLabel, out bool scoreExist, out float previousBestScore, 0);
        if (!scoreExist)
        {
            userInfo.User.SetPrimitive(in m_scoreLabel, in newScore);
            userInfo.User.SetPrimitive(in m_bestScoreLabel, in newScore);
        }
        else
        {
            userInfo.User.SetPrimitive(in m_scoreLabel, in newScore);
            if (newScore> previousBestScore)
                userInfo.User.SetPrimitive(in m_bestScoreLabel, in newScore);
        }
    }

}
