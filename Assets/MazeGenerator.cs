using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject roomPrefab;

    public int amountOfRooms = 2;
    public LayerMask roomLayer;

    private List<RoomBehaviour> builtRooms = new List<RoomBehaviour>();
    private RoomBehaviour currentRoom, joinedRoom;
    private Transform currentDoor, joinedDoor;
    private GameObject mazeParent;

    private void Start() {
        mazeParent = new GameObject("Maze Parent");
        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, mazeParent.transform).GetComponent<RoomBehaviour>();
        currentRoom.gameObject.name = "StartRoom";
        builtRooms.Add(currentRoom);
    }

    private void Update() {
        if (amountOfRooms <= 0) {
            Debug.Log("Generated");
        }
        else {
            PlaceRoom();
        }
    }

    private void PlaceRoom() {
        currentRoom = builtRooms[Random.Range(0, builtRooms.Count)];
        if(currentRoom.doors.Count <= 0) {
            builtRooms.Remove(currentRoom);
            return;
        }

        joinedRoom = Instantiate(roomPrefab, Vector3.one * 999, Quaternion.identity, mazeParent.transform).GetComponent<RoomBehaviour>();

        RandomDoor();
    }

    private void RandomDoor() {
        currentDoor = currentRoom.doors[Random.Range(0, currentRoom.doors.Count)];
        joinedDoor = joinedRoom.doors[Random.Range(0, joinedRoom.doors.Count)];

        SetRot();
        SetPos();
    }

    private void SetRot() {
        Vector3 entranceDoorwayEuler = joinedDoor.transform.eulerAngles;
        Vector3 exitDoorwayEuler = currentDoor.transform.eulerAngles;

        float deltaAngle = Mathf.DeltaAngle(entranceDoorwayEuler.y, exitDoorwayEuler.y);

        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        joinedRoom.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);
    }

    private void SetPos() {
        Vector3 roomPositionOffset = joinedDoor.position - joinedRoom.transform.position;
        Vector3 newRoomPosition = currentDoor.position - roomPositionOffset;

        if(joinedRoom.CheckRoom(newRoomPosition, roomLayer)) {
            Debug.Log("FailedPlace");
            Destroy(joinedRoom.gameObject);
            currentRoom.doors.Remove(currentDoor);
        } else {
            joinedRoom.transform.position = newRoomPosition;
            builtRooms.Add(joinedRoom);

            joinedRoom.doors.Remove(joinedDoor);
            Destroy(joinedDoor.gameObject);

            currentRoom.doors.Remove(currentDoor);
            Destroy(currentDoor.gameObject);

            amountOfRooms--;
        }
    }
}
