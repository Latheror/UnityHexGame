using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour {

    public GameObject volumeValue;
    public GameObject waterProportionValue;
    public GameObject mapSizeValue;

    public void SetVolumeValue(int value)
    {
        volumeValue.GetComponent<TextMeshProUGUI>().text = value.ToString() + " %";
    }

    public void SetWaterProportionValue(int value)
    {
        waterProportionValue.GetComponent<TextMeshProUGUI>().text = value.ToString() + " %";
    }

    public void SetMapSizeValue(int value)
    {
        mapSizeValue.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }


    public void VolumeSliderFloat(float value)
    {
        SetVolumeValue((int)value);
    }

    public void WaterProportionSliderFloat(float value)
    {
        SetWaterProportionValue((int)value);
    }

    public void MapSizeSliderFloat(float value)
    {
        SetMapSizeValue((int)value);
        MenuSettingsManager.instance.SetMapSize((int)value);
    }

}
