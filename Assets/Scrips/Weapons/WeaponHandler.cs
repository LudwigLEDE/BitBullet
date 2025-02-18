using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] GameObject pointer;
    [SerializeField] ParticleSystem flash;

    public GameObject GetPointer()
    {
        return pointer;
    }

    public ParticleSystem GetMuzzleFlash()
    {
        return flash;
    }
}
