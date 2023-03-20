using UnityEngine;

namespace WPM.Connect.Core
{
    public struct Line
    {
        public float initialSpeed { get; set; }
        public float currentSpeed { get; set; }
        public Vector3 spawnPosition { get; set; }
    }
}