using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBuildPanel : MonoBehaviour
{
    [SerializeField] private GameObject item_build = null;
    [SerializeField] private Transform root = null;

    private ItemBuildCell[] item_build_cells = null;

    
    public void init( Cell cell, ItemCell item_cell )
    {
        List<TypeBuild> available = GameManager.gameManager.getAvailableBuild();
        int count_availab_builds = available.Count;
        item_build_cells = new ItemBuildCell[count_availab_builds];
        
        for (int i = 0; i < count_availab_builds; i++)
        {
            ItemBuildCell item_build_cell = Instantiate(item_build, root).GetComponent<ItemBuildCell>();

            TypeBuild type_build = available[i];

            item_build_cell.init( type_build );
            item_build_cell.btn.onClick.AddListener(() => onClickNewBuild( cell, type_build, item_cell ));
            
            item_build_cells[i] = item_build_cell;
        }
    }

    public void close()
    {
        gameObject.SetActive(false);
        
        if(item_build_cells == null)
            return;
        
        foreach (ItemBuildCell item in item_build_cells)
        {
            item.btn.onClick.RemoveAllListeners();
            Destroy(item.gameObject);
        }
    }

    private void onClickNewBuild( Cell cell, TypeBuild type_build, ItemCell item_cell )
    {
        List<ValueClass> price = Const.getPriceForBuilding(type_build);

        if (GameManager.gameManager.enoughMoney(price))
        {
            GameManager.gameManager.payMoney(price);
            
            cell.startBuilding( type_build );
        
            item_cell.init(cell);// update that item
        
            close();
        }
        else
        {
            TooltipManager.tooltip_manager.startTooltip("Not enough money"); // TODO what not enough?
        }
    }
}
