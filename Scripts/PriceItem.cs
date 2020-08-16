using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceItem : MonoBehaviour
{
    [SerializeField] private Image img = null;
    [SerializeField] private TextMeshProUGUI txt_count = null;


    public void init(ValueClass value_class)
    {
        img.sprite = Const.getSpriteValue(value_class.type_values);
        txt_count.text = value_class.value.ToString();
    }
}
