using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     public static GameManager gameManager = null;

     [SerializeField] private RegionType current_region = RegionType.NONE;
     [Header("TIME")] 
     [SerializeField] public int global_time = 2200;
     [SerializeField] public int cur_progress_year = 0;

     [Header("MAIN VALUES")] 
     [SerializeField] private int money_trash = 0;
     public int moneyTrash
     {
          get => money_trash;
          set
          {
               money_trash = value;
               updateMoneyUI();
          }
     }
     
     [SerializeField] private int money_organic = 0;
     public int moneyOrganic
     {
          get => money_organic;
          set
          {
               money_organic = value;
               updateMoneyUI();
          }
     }
     [SerializeField] private float global_ecology = 0.0f;
     public float globalEcology
     {
          get => global_ecology;
          set
          {
               global_ecology = value;
               if(global_ecology < -5.0f)
                    finishGame(false);
               else
               if(global_ecology >= 80)
                    finishGame(true);
               
               updateMoneyUI();
          }
     }

     [SerializeField] private int upgrade_points = 5;

     public static event Action onChangePoints = null;
     public int upgradePoints
     {
          get => upgrade_points;
          set
          {
               upgrade_points = value;
               onChangePoints();
               icon_upgrade_points.gameObject.SetActive(upgrade_points > 0);
          }
     }
     [Header("SerializeField")]
     [SerializeField] private MeshRenderer[] mesh_renders = null;
     [SerializeField] private Material[] region_status = null;
     public RegionPanel panel_region = null;
     public NewBuildPanel panel_new_build = null;
     [Header("UI")]
     [SerializeField] private TextMeshProUGUI txt_global_time;
     [SerializeField] private TextMeshProUGUI txt_money_trash;
     [SerializeField] private TextMeshProUGUI txt_money_organic;
     [SerializeField] private TextMeshProUGUI txt_ecology;
     [SerializeField] private Animator earth_rot_animator = null;
     [SerializeField] private Animator animator_ui = null;
     [SerializeField] private Transform start_panel = null;
     [SerializeField] private Transform panel_laba = null;
     [SerializeField] private GameObject icon_upgrade_points = null;
     [SerializeField] private WinLosePanel win_lose_panel = null;
     [SerializeField] private TextMeshProUGUI txt_record = null;
     [SerializeField] private Hint hint = null;

     
     public static event Action onTick = delegate {};
     public static event Action onYearTick = delegate {};
     public static event Action onRobotsNew = delegate {};

     private List<TypeBuild> available_builds = new List<TypeBuild>() 
     {
          TypeBuild.FACTORY_1,
          TypeBuild.FACTORY_2,
          TypeBuild.FACTORY_3,
          TypeBuild.ORGANIC_TREES,
          TypeBuild.ORGANIC_BUSHES,
          TypeBuild.ORGANIC_GREENHOUSE,
          TypeBuild.LABARATORY
     };

     public List<TypeBuild> getAvailableBuild()
     {
          return available_builds;
     }
     
     private List<Region> regions = new List<Region>();
     private const int MAX_PROGRESS_YEAR = 4;

     private void Start()
     {
          if (gameManager == null)
               gameManager = this;
          
          setRegions();
          
          txt_record.gameObject.SetActive(PlayerPrefs.GetInt("record", 0) != 0);
          txt_record.text = $"{PlayerPrefs.GetInt("record", 0)}";
     }

     private void initialize()
     {
          hint.init();
          
          updateMoneyUI();
          
          //START MONEY
          moneyOrganic = 10;
          money_trash = 100;
          upgrade_points = 1;
          updateMoneyUI();
          
          /////TIME
          StartCoroutine(timeTick());
          updateGlobalTime();
     }

     private void setRegions()
     {
          regions.Add(new Region(RegionType.EURASIA, 8));
          regions.Add(new Region(RegionType.AFRICA, 5));
          regions.Add(new Region(RegionType.AUSTRALIA, 3));
          regions.Add(new Region(RegionType.NORTH_AMERICA, 6));
          regions.Add(new Region(RegionType.SOUTH_AMERICA, 4));

          foreach (Region reg in regions)
               setStatusRegion(reg);
     }

     private void onClickRegion( Region region )
     {
          if (!panel_region.gameObject.activeInHierarchy)
          {
               current_region = region.region_type;
               panel_region.gameObject.SetActive(true);
               panel_region.init(region);
          }
     }

     public NewBuildPanel getBuildPanel()
     {
          panel_new_build.gameObject.SetActive(true);
          return panel_new_build;
     }

     public void closePanel(GameObject panel)
     {
          if(panel.activeInHierarchy)
               panel.SetActive(false);
     }

     public void onTapRegion( RegionType region_type )
     {
          Region cur_tap_region = regions.FirstOrDefault(x => x.region_type == region_type);
          moneyTrash += cur_tap_region.count_robots;
          float bonus_eco = (float)cur_tap_region.count_robots / 1000;
          cur_tap_region.regionEcology += bonus_eco;
          string eco_value = string.Format("+{0:F2} ecology", bonus_eco);
          TooltipManager.tooltip_manager.startTooltip($"+{cur_tap_region.count_robots} trash, {eco_value}");
     }

     public void onLongClick( RegionType region_type )
     {
          Region cur_region = regions.FirstOrDefault(x => x.region_type == region_type);
          onClickRegion(cur_region);
     }

     public void setBonus(List<ValueClass> bonuses, RegionType parent_region)
     {
          if(bonuses == null || bonuses.Count == 0)
               return;

          foreach (ValueClass bonus in bonuses)
          {
               switch (bonus.type_values)
               {
                    case TypeValues.ROBOTS:
                         regions.FirstOrDefault(x => x.region_type == parent_region).count_robots += (int)bonus.value;
                         onRobotsNew();
                         break;
                    
                    case TypeValues.MONEY_TRASH:
                         moneyTrash += (int)bonus.value;
                         break;
                    
                    case TypeValues.MONET_ORGANIC:
                         moneyOrganic += (int)bonus.value;
                         break;
                    
                    case TypeValues.ECOLOGY:
                         regions.FirstOrDefault(x => x.region_type == parent_region).regionEcology += (int)bonus.value;
                         break;
                    
                    default: throw new ArgumentOutOfRangeException();
               }
          }
     }

     public void setStatusRegion(Region region)
     {
          getMeshByRegion(region.region_type).material = getMaterialByLvlEco(region.regionEcology);
     }

     private MeshRenderer getMeshByRegion( RegionType region_type )
     {
          return mesh_renders[(int)region_type - 1];
     }

     private Material getMaterialByLvlEco(float eco)
     {
          if (eco < 15.0f)
               return region_status[0];
          else if (eco >= 15 && eco < 45)
               return region_status[1];
          else 
               return region_status[2];
     }

     public bool enoughMoney(List<ValueClass> prices)
     {
          foreach (ValueClass value_class in prices)
          {
               switch (value_class.type_values)
               {
                    case TypeValues.MONEY_TRASH:
                         if (!enoughValue((int)value_class.value, money_trash))
                              return false;
                         break;
                    
                    case TypeValues.MONET_ORGANIC:
                         if (!enoughValue((int)value_class.value, moneyOrganic ))
                              return false;
                         break;
                    
                    default: throw new ArgumentOutOfRangeException();
               }
          }

          return true;

          bool enoughValue(int need_value, int has_value)
          {
               return has_value >= need_value;
          }
     }

     public void payMoney( List<ValueClass> prices )
     {
          foreach (ValueClass value_class in prices)
          {
               switch (value_class.type_values)
               {
                    case TypeValues.MONEY_TRASH:
                         moneyTrash -= (int)value_class.value;
                         break;
                    
                    case TypeValues.MONET_ORGANIC:
                         moneyOrganic -= (int)value_class.value;
                         break;
                    
                    default: throw new ArgumentOutOfRangeException();
               }
          }
     }

     private void updateMoneyUI()
     {
          txt_money_trash.text = $"{money_trash}";
          txt_money_organic.text = $"{money_organic}";
          txt_ecology.text = $"ECOLOGY: {global_ecology:F3}%";
     }

     private IEnumerator timeTick()
     {
          while (true)
          {
               yield return new WaitForSeconds(1.0f);

               cur_progress_year++;
               if (cur_progress_year >= MAX_PROGRESS_YEAR)
               {
                    cur_progress_year = 0;
                    onYearTick();
                    tick(true);
                    
                    global_time++;
                    updateGlobalTime();
               }
               
               onTick();
               tick();
          }
     }

     private void updateGlobalTime()
     {
          txt_global_time.text = $"{global_time} year";
     }

     public void updateGlobalEco()
     {
          globalEcology = regions.Sum(x => x.regionEcology) / regions.Count;
     }

     private void tick(bool is_depent_on_year = false)
     {
          foreach (Region region in regions)
          {
               int count_cell_in_process = region.cells.Count(x => x.cell_status == CellStatus.IN_PROCESS);
               
               int count_robot_this_cell = 0;
               if(count_cell_in_process != 0)
                    count_robot_this_cell = region.count_robots / count_cell_in_process;

               if (!is_depent_on_year)
               {
                    foreach (Cell cell in region.cells)
                    {
                         makeTickInCell( cell, count_robot_this_cell );
                    }
               }
               else
               {
                    foreach (Cell cell in region.cells)
                    {
                         if (cell.cell_status == CellStatus.IN_PROCESS)
                         {
                              if (Const.isBuildDependsOfYear(cell.type_build))
                              {
                                   cell.curProgress++;
                              }
                         }
                         makeTickInCell( cell, count_robot_this_cell );
                    }
               }
          }

          void makeTickInCell( Cell cell, int count_robots )
          {
               if (cell.cell_status == CellStatus.IN_PROCESS)
               {
                    if (!Const.isBuildDependsOfYear(cell.type_build))
                    {
                         cell.curProgress += count_robots;
                    }
               }
          }
     }

     public bool isAnyPanelActive()
     {
          return panel_region.gameObject.activeInHierarchy 
                 || start_panel.gameObject.activeInHierarchy 
                 || panel_laba.gameObject.activeInHierarchy;
     }

     public void onPressToStart(GameObject pressToStartPanel)
     {
          pressToStartPanel.SetActive(false);

          earth_rot_animator.enabled = false;
          animator_ui.Play("start_ui");
          
          initialize();
     }

     public void onClickLabaratory()
     {
          panel_laba.gameObject.SetActive(true);
          panel_laba.GetComponent<PanelLaba>().init();
     }

     private void finishGame( bool is_win )
     {
          win_lose_panel.init( is_win );
     }
}

public class Region
{
     public RegionType region_type = RegionType.NONE;
     public RegionStatus region_status = RegionStatus.LOCKED;
     public List<Cell> cells = null;
     public int count_robots = 50;
     private float region_ecology = 5.0f;
     public  Action onUpdateEcology = null;

     public float regionEcology
     {
          get => region_ecology;
          set
          {
               if(region_ecology > 100.0f)
                    return;
               
               region_ecology = value;
               GameManager.gameManager.setStatusRegion(this);
               onUpdateEcology?.Invoke();
               GameManager.gameManager.updateGlobalEco();
               if(region_ecology >= 100.0f)
                    TooltipManager.tooltip_manager.startTooltip($"Region ${region_type} is full ecologed");// TODO translate
          }
     }

     public Region(RegionType region_type, int count_cells)
     {
          this.region_ecology = 5.0f;
          this.region_type = region_type;
          
          cells = new List<Cell>();
          for (int i = 0; i < count_cells; i++)
               cells.Add( new Cell(region_type));
     }
}

public enum RegionStatus
{
     LOCKED,
     UNLOCKED
}

public enum TypeBuild
{
     NONE,
     FACTORY_1,
     FACTORY_2,
     FACTORY_3,
     ORGANIC_TREES,
     ORGANIC_GREENHOUSE,
     ORGANIC_BUSHES,
     LABARATORY
}

public enum CellStatus
{
     EMPTY,
     IN_PROCESS,
     DONE
}

public enum RegionType
{
     NONE,
     EURASIA,
     AFRICA,
     AUSTRALIA,
     NORTH_AMERICA,
     SOUTH_AMERICA
}