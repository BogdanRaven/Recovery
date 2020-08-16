using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI txt_title;
  [SerializeField] private Image img_icon;
  [SerializeField] private GameObject progress_bar;
  [SerializeField] private Image progress_bar_line;

  private Cell cell = null;
  private float length_bar_line;
  
  public Button btn => GetComponent<Button>();

  private void Start()
  {
    GameManager.onTick += updateBar;
  }

  public void init(Cell cell)
  {
    this.cell = cell;

    cell.onDone += onDoneBuilding;
    GetComponent<Button>().onClick.AddListener(onClick);

    if (cell.cell_status != CellStatus.EMPTY)
    {
      txt_title.text = Const.getBuildName(cell.type_build);
      img_icon.sprite = Const.getSpriteBuild(cell.type_build);
    }
    
    progress_bar.SetActive(cell.cell_status == CellStatus.IN_PROCESS);
    updateBar();
  }

  private void onDoneBuilding()
  {
    if(this != null)
      progress_bar.SetActive(false);
  }

  private void onClick()
  {
    if (cell.cell_status == CellStatus.EMPTY)
    {
      GameManager.gameManager.getBuildPanel().init(cell, this);
    }
  }

  private void updateBar()
  {
    if(cell.cell_status == CellStatus.IN_PROCESS)
      progress_bar_line.fillAmount = cell.curProgress / cell.max_progress;
  }

  private void OnDestroy()
  {
    GameManager.onTick -= updateBar;
    if (cell != null)
    {
      cell.onDone -= onDoneBuilding;
    }
  }
}
