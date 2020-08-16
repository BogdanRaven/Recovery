using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLickOnEarth : MonoBehaviour
{
  // Start is called before the first frame update

  public Camera camera;
  [SerializeField] private Transform earth = null;
  private float maxDistance = 10;// максимально возможная дистанция

  public float sensitivity = 3; // чувствительность мышки
  private float X, Y;

  Vector3 positionBegin = Vector3.zero;
  Vector3 positionEnd = Vector3.zero;
  Vector3 positionLast = Vector3.zero;

  private bool isHit;
  private bool click = false;
  private bool swipe = false;

  const float timer=0.3f;
  float press_time = 0.0f;
  

  // Update is called once per frame
  void Update()
  {
    if (GameManager.gameManager.isAnyPanelActive())
      return;
    
    if (click)
    {
      swipe = Vector3.Distance(Input.mousePosition, positionBegin) >= maxDistance;
      click = !swipe;
    }

    if (Time.time - press_time >= timer && click)
    {
      if (positionEnd == Vector3.zero)
        positionEnd = Input.mousePosition;
      
      TouchForLong();
      click = false;
    }

    if (swipe)
      Swipe();

    positionLast = Input.mousePosition;
  }

  void OnMouseDown()
  {
    if (GameManager.gameManager.isAnyPanelActive())
      return;
    
    click = true;
	
    swipe = false;
    positionBegin = Input.mousePosition;
    positionLast = Input.mousePosition;
    press_time = Time.time;
  }

  void OnMouseUp()
  {
    if (GameManager.gameManager.isAnyPanelActive())
      return;
    
    positionEnd = Input.mousePosition;
    if (click)
    {
      Touch();
    }
    click = false;
    swipe = false;
  }

  private void Swipe()
  {
    //Debug.Log("swipe");
    X = earth.localEulerAngles.y - Input.GetAxis("Mouse X") * sensitivity;
    earth.localEulerAngles = new Vector3(0.0f, X, 0);
  }

  private void Touch()
  {
    RaycastHit hit;
    Ray ray = camera.ScreenPointToRay(positionEnd);

    if (Physics.Raycast(ray, out hit))
    {
      Debug.Log(hit.transform.name);
      if (Time.time - press_time < timer)
      {
        if(hit.transform.CompareTag("Earth"))
          return;
      
        GameManager.gameManager.onTapRegion(getRegionTypeByName(hit.transform.tag));
      }
    }
  }
  
  private void TouchForLong()
  {
    RaycastHit hit;
    Ray ray = camera.ScreenPointToRay(positionEnd);

    if (Physics.Raycast(ray, out hit))
    {
      Debug.Log(hit.transform.name);
      if(hit.transform.CompareTag("Earth"))
        return;
      
      GameManager.gameManager.onLongClick(getRegionTypeByName(hit.transform.tag));
    }
  }

  private RegionType getRegionTypeByName( string tag )
  {
    switch (tag)
    {
      case "Eurasia":
        return RegionType.EURASIA;
      
      case "Africa":
        return RegionType.AFRICA;
      
      case "Avstralia":
        return RegionType.AUSTRALIA;

      case "North":
        return RegionType.NORTH_AMERICA;
      
      case "South":
        return RegionType.SOUTH_AMERICA;
      
      default: throw new ArgumentOutOfRangeException();
    }
  }
}