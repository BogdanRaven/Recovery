using UnityEngine;
using UnityEngine.UI;

public class ItemLaba : MonoBehaviour
{
    [SerializeField] private ItemLaba[] unlock_items = null;
    [SerializeField] private Image img_back = null;
    [SerializeField] private Image img = null;
    private bool is_lock = true;

    private void Start()
    {
        setAlpha(0.1f);
    }

    public void unlock()
    {
        is_lock = false;
        setAlpha(1.0f);
    }

    public void upgrade()
    {
        if (GameManager.gameManager.upgradePoints > 0 )
        {
            if(is_lock)
                return;
            
            img_back.color = Color.green;
            setAlpha(1.0f);
            GameManager.gameManager.upgradePoints--;
            TooltipManager.tooltip_manager.startTooltip("Success!, but this upgrade in develop:(");
            foreach (ItemLaba item_laba in unlock_items)
            {
                item_laba.unlock();
            }
        }
        else
        {
            TooltipManager.tooltip_manager.startTooltip("Not enough upgrade points");
        }
    }

    private void setAlpha(float value)
    {
        var alpha = new Color(img.color.a,img.color.g,img.color.b, value);
        img.color = alpha;
    }
}
