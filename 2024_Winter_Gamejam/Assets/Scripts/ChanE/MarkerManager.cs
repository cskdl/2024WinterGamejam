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

    private void FixedUpdate()
    {
        UpdateMarkerList();
    }

    public void UpdateMarkerList()
    {
        MarkerList.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        MarkerList.Clear();
        UpdateMarkerList();
    }
}
