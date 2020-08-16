using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionPanel : MonoBehaviour
{
  [SerializeField] private ItemCell item_cell;
  [SerializeField] private Transform item_root;
  [SerializeField] private TextMeshProUGUI txt_name_region;
  [SerializeField] private TextMeshProUGUI txt_count_robots;
  [SerializeField] private TextMeshProUGUI txt_region_eco;
  [SerializeField] private Image img_line;
  
  private Region region = null;
  private ItemCell[] item_cells = null;

  private void Start()
  {
    GameManager.onRobotsNew += () =>
    {
      if (gameObject.activeInHierarchy)
        updateCountRobots();
    };
  }

  public void init(Region region)
  {
    this.region = region;

    updateCountRobots();

    txt_name_region.text = region.region_type.ToString();
    updateEco();

    region.onUpdateEcology = updateEco;
    
    item_cells = new ItemCell[region.cells.Count];
    for (int i = 0; i < region.cells.Count; i++)
    {
      ItemCell item = Instantiate(item_cell, item_root);
      Cell cell = region.cells[i];
      item_cells[i] = item;
      item.init(cell);
    }
  }

  private void updateEco()
  {
    txt_region_eco.text = $"{region.regionEcology:F2}";
    img_line.fillAmount = region.regionEcology / 100.0f;
  }

  public void close()
  {
    gameObject.SetActive(false);
    foreach (ItemCell cell in item_cells)
    {
        cell.btn.onClick.RemoveAllListeners();
        Destroy(cell.gameObject);
    }
  }

  private void updateCountRobots()
  {
    txt_count_robots.text = $"{region.count_robots}";
  }
}
