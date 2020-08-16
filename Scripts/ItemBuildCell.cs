using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuildCell : MonoBehaviour
{
  [SerializeField] private Image img_icon = null;
  [SerializeField] private TextMeshProUGUI txt_name;
  [SerializeField] private Transform root;
  [SerializeField] private PriceItem price_item;

  public Button btn => GetComponent<Button>();
  
  public void init( TypeBuild type_build )
  {
    img_icon.sprite = Const.getSpriteBuild(type_build);
    txt_name.text = Const.getBuildName(type_build);

    List<ValueClass> price = Const.getPriceForBuilding(type_build);
    spawnPrice(price);
  }

  private void spawnPrice( List<ValueClass> price )
  {
    foreach (ValueClass value_class in price)
    {
      PriceItem item = Instantiate(price_item, root);
      item.init(value_class);
    }
  }
}
