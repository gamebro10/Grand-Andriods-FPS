using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IDamage
{
    void OnTakeDamage(int amount);
}
