using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 dormCameraPosition;
    [SerializeField] Vector3 hospitalCameraPosition;
    [SerializeField] Vector3 libraryCameraPosition;
    [SerializeField] Vector3 parkCameraPosition;
    [SerializeField] Transform centerReference;

    [SerializeField] List<Transform> playerTransform;

    [SerializeField] Vector3 offsetFromPlayer;
    [SerializeField] Vector3 closeOffsetFromPlayer;

    [SerializeField] float movingProportion;

    [SerializeField] Transform diceTransform;
    [SerializeField] Vector3 diceOffset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetMoveToPosition();
        transform.LookAt(GetTargetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveToPosition = GetMoveToPosition();
        Vector3 targetPosition = GetTargetPosition();


        // Move to the target position smoothly
        MoveAndFaceAtTargetSmooth(moveToPosition, targetPosition);
    }

    private void MoveAndFaceAtTargetSmooth(Vector3 moveTo, Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, moveTo, movingProportion);
        transform.LookAt(target);
    }

    private Vector3 GetMoveToPosition()
    {
        switch (GameSettings.cameraDirection)
        {
            case CameraDirection.DORM: return dormCameraPosition;
            case CameraDirection.HOSTPITAL: return hospitalCameraPosition;
            case CameraDirection.LIBRARY: return libraryCameraPosition;
            case CameraDirection.PARK: return parkCameraPosition;
            case CameraDirection.PLAYER: return playerTransform[GameStats.CurrentPlayerIndex].position + (playerTransform[GameStats.CurrentPlayerIndex].rotation * offsetFromPlayer);
            case CameraDirection.CLOSE_PLAYER: return playerTransform[GameStats.CurrentPlayerIndex].position + (playerTransform[GameStats.CurrentPlayerIndex].rotation * closeOffsetFromPlayer);
            case CameraDirection.DICE: return diceTransform.position + diceOffset;
        }

        return Vector3.zero;
    }

    private Vector3 GetTargetPosition()
    {
        switch (GameSettings.cameraDirection)
        {
            case CameraDirection.DORM: 
            case CameraDirection.HOSTPITAL: 
            case CameraDirection.LIBRARY: 
            case CameraDirection.PARK: return centerReference.position;
            case CameraDirection.PLAYER:
            case CameraDirection.CLOSE_PLAYER: return playerTransform[GameStats.CurrentPlayerIndex].position;
            case CameraDirection.DICE: return diceTransform.position;
        }

        return Vector3.zero;
    }
}
