using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public RegionType region_type = RegionType.NONE;
    public CellStatus cell_status = CellStatus.EMPTY;
    public TypeBuild type_build = TypeBuild.NONE;
    private float cur_progress = 0.0f;
    public float max_progress = 100.0f;
    public List<ValueClass> cell_bonuses = null;
    
    public event Action onDone = delegate {};

    public float curProgress
    {
        get { return cur_progress; }
        set
        {
            cur_progress = value;
            if (cur_progress >= max_progress)
            {
                cell_status = CellStatus.DONE;
                GameManager.gameManager.setBonus(cell_bonuses, region_type);
                onDone();
            }
        }
    }

    public Cell( RegionType parent_region )
    {
        this.region_type = parent_region;
        cell_status = CellStatus.EMPTY;
        type_build = TypeBuild.NONE;
        cur_progress = 0.0f;
        max_progress = 100.0f;
    }

    public void startBuilding(TypeBuild type_build)
    {
        this.type_build = type_build;
        cell_status = CellStatus.IN_PROCESS;
        cur_progress = 0.0f;
        max_progress = Const.getTimeForBuilding(type_build);
        cell_bonuses = Const.getFactoryBonus(type_build);
    }


}

public class ValueClass
{
    public TypeValues type_values;
    public float value;

    public ValueClass( TypeValues type_values, float value)
    {
        this.type_values = type_values;
        this.value = value;
    }

    public static ValueClass createBonus(TypeValues type_values, float value)
    {
        return new ValueClass( type_values, value );
    }
}

public enum TypeValues
{
    NONE,
    ROBOTS,
    MONEY_TRASH,
    MONET_ORGANIC,
    ECOLOGY,
    UPGRADE
}
