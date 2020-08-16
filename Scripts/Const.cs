using System;
using System.Collections.Generic;
using UnityEngine;

public class Const
{
  public static int getTimeForBuilding( TypeBuild type_build )
  {
    switch (type_build)
    {
      case TypeBuild.FACTORY_1: return 400;
      case TypeBuild.FACTORY_2: return 1000;
      case TypeBuild.FACTORY_3: return 1500;
      case TypeBuild.ORGANIC_TREES: return 10;
      case TypeBuild.ORGANIC_GREENHOUSE: return 2;
      case TypeBuild.ORGANIC_BUSHES: return 5;
      case TypeBuild.LABARATORY: return 3000;
      
      default: throw new ArgumentOutOfRangeException(nameof(type_build), type_build, null);
    }
  }

  public static bool isBuildDependsOfYear(TypeBuild type_build)
  {
    switch (type_build)
    {
      case TypeBuild.FACTORY_1:
      case TypeBuild.FACTORY_2:
      case TypeBuild.LABARATORY:
      case TypeBuild.FACTORY_3:return false;
      case TypeBuild.ORGANIC_TREES:
      case TypeBuild.ORGANIC_GREENHOUSE:
      case TypeBuild.ORGANIC_BUSHES:return true;
      
      default:
        throw new ArgumentOutOfRangeException(nameof(type_build), type_build, null);
    }
  }

  public static List<ValueClass> getFactoryBonus(TypeBuild type_build)
  {
    List<ValueClass> bonuses = new List<ValueClass>();
        
    switch (type_build)
    {
      case TypeBuild.FACTORY_1: 
        bonuses.Add(ValueClass.createBonus(TypeValues.ROBOTS, 10));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, -5));
        break;
      
      case TypeBuild.FACTORY_2:
        bonuses.Add(ValueClass.createBonus(TypeValues.ROBOTS, 25));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, -7.5f));
        break;
      
      case TypeBuild.FACTORY_3:
        bonuses.Add(ValueClass.createBonus(TypeValues.ROBOTS, 50));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, -25.0f));
        break;
            
      case TypeBuild.ORGANIC_TREES: 
        bonuses.Add(ValueClass.createBonus(TypeValues.MONET_ORGANIC, 10));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, 10.0f));
        break;

      case TypeBuild.ORGANIC_GREENHOUSE:
        bonuses.Add(ValueClass.createBonus(TypeValues.MONET_ORGANIC, 5));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, 5.0f));
        break;
      
      case TypeBuild.ORGANIC_BUSHES:
        bonuses.Add(ValueClass.createBonus(TypeValues.MONET_ORGANIC, 5));
        bonuses.Add(ValueClass.createBonus(TypeValues.ECOLOGY, 3.0f));
        break;
      
      case TypeBuild.LABARATORY:
        bonuses.Add(ValueClass.createBonus(TypeValues.UPGRADE, 1));
        break;
      
      default: throw new ArgumentOutOfRangeException(nameof(type_build), type_build, null);
    }

    return bonuses;
  }
  
  
  public static List<ValueClass> getPriceForBuilding(TypeBuild type_build)
  {
    List<ValueClass> prices = new List<ValueClass>();
    
    switch (type_build)
    {
      case TypeBuild.FACTORY_1: 
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 1000));
        break;
      
      case TypeBuild.FACTORY_2:
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 24000));
        break;
      
      case TypeBuild.FACTORY_3:
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 56000));
        break;
      
      case TypeBuild.ORGANIC_TREES:
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 100));
        prices.Add(new ValueClass(TypeValues.MONET_ORGANIC, 3));
        break;
      
      case TypeBuild.ORGANIC_GREENHOUSE:
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 10000));
        prices.Add(new ValueClass(TypeValues.MONET_ORGANIC, 25));
        break;
      
      case TypeBuild.ORGANIC_BUSHES:
        prices.Add(new ValueClass(TypeValues.MONET_ORGANIC, 5));
        break;
      
      case TypeBuild.LABARATORY:
        prices.Add(new ValueClass(TypeValues.MONEY_TRASH, 15000));
        prices.Add(new ValueClass(TypeValues.MONET_ORGANIC, 10));
        break;

      
      default: throw new ArgumentOutOfRangeException(nameof(type_build), type_build, null);
    }

    return prices;
  }

  public static Sprite getSpriteValue(TypeValues type_values)
  {
    string path = $"Sprites/TypeValues/{type_values.ToString()}";
    return Resources.Load<Sprite>(path);
  }

  public static Sprite getSpriteBuild(TypeBuild type_build)
  {
    string path = $"Sprites/Builds/{type_build.ToString()}";
    return Resources.Load<Sprite>(path);
  }

  public static string getBuildName(TypeBuild type_build)
  {
    switch (type_build)
    {
      case TypeBuild.NONE: return string.Empty;
      case TypeBuild.FACTORY_1: return "Factory lvl I";
      case TypeBuild.FACTORY_2: return "Factory lvl II";
      case TypeBuild.FACTORY_3: return "Factory lvl III";
      case TypeBuild.ORGANIC_TREES: return "Forest";
      case TypeBuild.ORGANIC_GREENHOUSE: return "GreenHouse";
      case TypeBuild.ORGANIC_BUSHES: return "Field of bushes";
      case TypeBuild.LABARATORY: return "Labaratory";
      
      default:
        throw new ArgumentOutOfRangeException(nameof(type_build), type_build, null);
    }
  }
}
