using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME_.Scripts
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private List<Texture> crosshairs;
        [SerializeField] private RawImage currentCrossHair;

        public void ChangeCrosshair(int newCrosshair)
        {
            currentCrossHair.texture = crosshairs[newCrosshair];
        }
    }
}
