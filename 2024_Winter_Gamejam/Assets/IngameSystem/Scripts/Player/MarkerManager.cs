using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public class Marker
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public Marker(Vector3 pPosition, Quaternion pRotation)
        {
            Position = pPosition;
            Rotation = pRotation;
        }
    }

    public List<Marker> MarkerList = new List<Marker>();

    private int counter = 5;

    private void FixedUpdate()
    {
        UpdateMarkerList();
    }

    public void UpdateMarkerList()
    {
        if (counter >= 4 || MarkerList.Count <= 0)
        {
            MarkerList.Add(new Marker(transform.position, transform.rotation));
            counter = 0;
        }
        else counter++;
    }

    public void ClearMarkerList()
    {
        MarkerList.Clear();
        counter = 5;
        UpdateMarkerList();
    }
}
