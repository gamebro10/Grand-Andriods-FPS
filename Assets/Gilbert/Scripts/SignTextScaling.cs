using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignTextScaling : MonoBehaviour
{
    public TextMeshPro words;

    public void Start()
    {
        words = this.GetComponent<TextMeshPro>();
     }

    public void Update()
    {
        Scale();
    }

    public void Scale()
    {
        words.fontSize = (this.transform.localScale.x) * 50;
    }
}
