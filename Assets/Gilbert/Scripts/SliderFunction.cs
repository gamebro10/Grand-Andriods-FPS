using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderFunction : MonoBehaviour
{
    [SerializeField] Slider SliderBar;
    [SerializeField] TextMeshProUGUI SliderText;

    void Start()
    {
        SliderBar.onValueChanged.AddListener((v) =>
        {
            SliderText.text = v.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
