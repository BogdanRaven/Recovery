using System.Collections;
using TMPro;
using UnityEngine;

public class TooltipItem : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI txt_text = null;
   [SerializeField] private CanvasGroup canvas_group = null;
   
   private float time_move = 1.0f;
   private float cur_time_move = 0;
   private float speed = 0.10f;
   
   public void init( string text )
   {
      txt_text.text = text;
      cur_time_move = time_move;
      transform.localPosition = Vector3.zero;
      
      StartCoroutine(coroutineMove());
   }

   private IEnumerator coroutineMove()
   {
      while (true && cur_time_move > 0.0f)
      {
         transform.Translate(Vector3.up * speed);
         canvas_group.alpha = cur_time_move / time_move;
         cur_time_move -= Time.deltaTime;
         yield return null;
      }
      
      Destroy(gameObject);
   }
}
