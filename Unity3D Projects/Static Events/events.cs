using UnityEngine;

public class events : MonoBehaviour
{

    public delegate void RequestingGameState(GameStates GameStateRequest);
    public static event RequestingGameState OnRequestingGameState;

    public static void RequestGameState(GameStates request)
    {
        if (OnRequestingGameState != null)
        {
            OnRequestingGameState(request);
        }
    }


    public delegate void GameStateChanging(GameStates newstate);
    public static event GameStateChanging OnGameStateChanging;

    public static void ChangeGameState(GameStates currentstate)
    {
        if (OnGameStateChanging != null)
        {
            OnGameStateChanging(currentstate);
        }
    }

    public delegate void GridChanging();
    public static event GridChanging OnGridChanging;

    public static void GridChanged()
    {
        if (OnGridChanging != null)
        {
            OnGridChanging();
        }
    }


    public delegate void RequestingStoryGameLevel(int LevelNumber);
    public static event RequestingStoryGameLevel OnRequestingStoryGameLevel;

    public static void RequestStoryGameLevel(int number)
    {
        if (OnRequestingStoryGameLevel != null)
        {
            OnRequestingStoryGameLevel(number);
        }
    }


    public delegate void RequestingGameLogicState(GameLogicStates newState);
    public static event RequestingGameLogicState OnRequestingGameLogicState;

    public void RequestGameLogicState(GameLogicStates request)
    {
        if (OnRequestingGameLogicState != null)
        {
            OnRequestingGameLogicState(request);
        }
    }


    public delegate void GameLogicStateChanging(GameLogicStates changed);
    public static event GameLogicStateChanging OnGameLogicStateChanging;

    public static void ChangeGameLogicState(GameLogicStates changing)
    {
        if (OnGameLogicStateChanging != null)
        {
            OnGameLogicStateChanging(changing);
        }
    }


    public delegate void SendingPulse();
    public static event SendingPulse OnSendingPulse;

    public static void SendPulse()
    {
        if (OnSendingPulse != null)
        {
            OnSendingPulse();
        }
    }


    public delegate void ReportingScore(int amt, PieceColours col);
    public static event ReportingScore OnReportingScore;

    public static void ReportScore(int amount, PieceColours Colour)
    {
        if (OnReportingScore != null)
        {
            OnReportingScore(amount, Colour);
        }
    }


    public delegate void ReportingSpecial(PieceStates state, coords c);
    public static event ReportingSpecial OnReportingSpecial;

    public static void ReportSpecial(PieceStates special, coords coordinates)
    {
        if (OnReportingSpecial != null)
        {
            OnReportingSpecial(special, coordinates);
        }
    }


    public delegate void DamagingPlayer(float amount);
    public static event DamagingPlayer OnDamagingPlayer;

    public static void DamagePlayer(float damage)
    {
        if (OnDamagingPlayer != null)
        {
            OnDamagingPlayer(damage);
        }
    }


    public delegate void PlayerDying();
    public static event PlayerDying OnPlayerdying;

    public static void PlayerDied()
    {
        if (OnPlayerdying != null)
        {
            OnPlayerdying();
        }
    }


    public delegate void PlayerDyingMP(string ID);
    public static event PlayerDyingMP OnPlayerdyingMP;

    public static void PlayerDiedMP(string id)
    {
        if (OnPlayerdyingMP != null)
        {
            OnPlayerdyingMP(id);
        }
    }



    public delegate void SettingForbiddenInputTiles(int[,] tiles);
    public static event SettingForbiddenInputTiles OnSettingForbiddenInputTiles;

    public static void SetForbiddenInputTiles(int[,] tilemap)
    {
        if (OnSettingForbiddenInputTiles != null)
        {
            OnSettingForbiddenInputTiles(tilemap);
        }
    }


    public delegate void SettingGridPieces(int[,] colours);
    public static event SettingGridPieces OnSettingGridPieces;

    public static void SetGridPieces(int[,] cols)
    {
        if (OnSettingGridPieces != null)
        {
            OnSettingGridPieces(cols);
        }
    }


    public delegate void RequestingPulse();
    public static event RequestingPulse OnRequestingPulse;

    public static void RequestPulse()
    {
        if (OnRequestingPulse != null)
        {
            OnRequestingPulse();
        }
    }


    public delegate void SettingPulseAllowed(bool state);
    public static event SettingPulseAllowed OnSettingPulseAllowed;

    public static void SetPulseAllowed(bool setPulse)
    {
        if (OnSettingPulseAllowed != null)
        {
            OnSettingPulseAllowed(setPulse);
        }
    }


    public delegate void PlayerSelectingProfile(PlayerProfile p);
    public static event PlayerSelectingProfile OnPlayerSelectingProfile;

    public static void PlayerSelectedProfile(PlayerProfile pee)
    {
        if (OnPlayerSelectingProfile != null)
        {
            OnPlayerSelectingProfile(pee);
        }
    }



    public delegate void RequestingMultiplierLevel();
    public static event RequestingMultiplierLevel OnRequestingMultiplayerLevel;

    public static void RequestMultiplayerLevel()
    {
        if (OnRequestingMultiplayerLevel != null)
        {
            OnRequestingMultiplayerLevel();
        }
    }


    public delegate void GameLogicMultiplayerStateChanging(GameLogicMultiplayerStates state);
    public static event GameLogicMultiplayerStateChanging OnGameLogicMultiplayerStateChanging;

    public static void ChangeMultiplayerState(GameLogicMultiplayerStates s)
    {
        if (OnGameLogicMultiplayerStateChanging != null)
        {
            OnGameLogicMultiplayerStateChanging(s);
        }
    }


    public delegate void ClearingPiece(coords c, Vector3 pos, PieceColours col);
    public static event ClearingPiece OnClearingPiece;

    public static void ClearPiece(coords coord, Vector3 pos, PieceColours c)
    {
        if (OnClearingPiece != null)
        {
            OnClearingPiece(coord, pos, c);
        }
    }


    public delegate void RequestingMultiplayerDisconnect();
    public static event RequestingMultiplayerDisconnect OnRequestingMultiplayerDisconnect;

    public static void RequestMultiplayerDisconnect()
    {
        if (OnRequestingMultiplayerDisconnect != null)
        {
            OnRequestingMultiplayerDisconnect();
        }
    }


    public delegate void RequestingOneShotFX(OneShotFXTypes type, Vector3 pos);
    public static event RequestingOneShotFX OnRequestingOneShotFX;

    public static void RequestOneShotFX(OneShotFXTypes t, Vector3 here)
    {
        if (OnRequestingOneShotFX != null)
        {
            OnRequestingOneShotFX(t, here);
        }
    }


    public delegate void RequestingVolumeChange(VolumeRequest req, float amt);
    public static event RequestingVolumeChange OnRequestingVolumeChange;

    public static void RequestVolumeChange(VolumeRequest request, float volume)
    {
        if (OnRequestingVolumeChange != null)
        {
            OnRequestingVolumeChange(request, volume);
        }
    }


    public delegate void RequestingNotification(NotificationTypes type, string text, Vector3 startPos);
    public static event RequestingNotification OnRequestingNotification;

    public static void RequestNotification(NotificationTypes t, string notification, Vector3 startPos)
    {
        if (OnRequestingNotification != null)
        {
            OnRequestingNotification(t, notification, startPos);
        }
    }



    public delegate void RequestingCurvySpark(CurvySparkTypes type, Vector3 startpos, Vector3 endpos, float flighttime);
    public static event RequestingCurvySpark OnRequestingCurvySpark;

    public static void RequestCurvySpark(CurvySparkTypes t, Vector3 start, Vector3 end, float time)
    {
        if (OnRequestingCurvySpark != null)
        {
            OnRequestingCurvySpark(t, start, end, time);
        }
    }
}

