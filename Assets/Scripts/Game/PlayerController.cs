using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] List<Transform> playerTransform;
    [SerializeField] float playerMovingSpeed;
    [SerializeField] Transform gamePlatform;
    [SerializeField] float moveError;
    [SerializeField] GameExecutor gameExecutor;

    List<Player> players;
    PlatformHelper platformHelper;

    // Start is called before the first frame update
    void Start()
    {
        players = GameStats.GetPlayerList();
        platformHelper = gamePlatform.GetComponent<PlatformHelper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStats.currentState == GameState.MOVE)
        {
            if (GameStats.UI.DiceResult == 0)
            {
                GameStats.currentState = GameState.EXECUTE_FEATURE;
                GameSettings.cameraDirection = CameraDirection.CLOSE_PLAYER;
                return;
            }

            GameSettings.cameraDirection = CameraDirection.PLAYER;

            int playerIndex = GameStats.CurrentPlayerIndex;
            Vector3 destPoint = platformHelper.GetWalkingPoint((players[playerIndex].StandingPos + 1) % 32).position;
            destPoint.y = playerTransform[playerIndex].position.y;
            Vector3 playerPos = playerTransform[playerIndex].position;
            
            if (MyTools.Distance2D(playerPos, destPoint) > (GameStats.UI.DiceResult > 1 ? moveError : 0.3))
            {
                Vector3 temp = (destPoint - playerPos);
                Vector3 dirToDest = new Vector3(temp.x, 0, temp.z).normalized;
                playerTransform[playerIndex].localPosition += dirToDest * (playerMovingSpeed * Time.deltaTime);
            } else
            {
                GameStats.UI.DiceResult--;
                players[playerIndex].StandingPos = (players[playerIndex].StandingPos + 1) % 32;
                if (players[playerIndex].StandingPos % 8 == 0)
                {
                    playerTransform[playerIndex].Rotate(new Vector3(0, 90, 0));
                }
                if (players[playerIndex].StandingPos == 0)
                {
                    GameStats.currentState = GameState.PASS_START;
                }
            }
            return;
        }
    }

    public void TeleportPlayerToCorner(int playerIndex, CornerBlock corner)
    {
        int blockIndex = 0;
        switch (corner)
        {
            case CornerBlock.DORM: blockIndex = 0; break;
            case CornerBlock.HOSPITAL: blockIndex = 8; break;
            case CornerBlock.LIRBARY: blockIndex = 16; break;
            case CornerBlock.PLAYGROUND: blockIndex = 24; break;
        }
        Transform trans = playerTransform[playerIndex];
        Vector3 destPoint = platformHelper.GetWalkingPoint(blockIndex).position;
        destPoint.y = trans.position.y;
        trans.SetPositionAndRotation(destPoint, Quaternion.identity);
        trans.Rotate(new Vector3(0, 90 * (blockIndex / 8), 0));
        players[playerIndex].StandingPos = blockIndex;
    }
}
