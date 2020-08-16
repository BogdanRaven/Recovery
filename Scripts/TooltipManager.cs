using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
  public static TooltipManager tooltip_manager = null;

  [SerializeField] private Transform root_tooltip = null;
  [SerializeField] private TooltipItem tooltip_item = null;

  private void Start()
  {
    tooltip_manager = this;
  }

  public void startTooltip(string text)
  {
    Instantiate(tooltip_item, root_tooltip).init(text);
  }
}
