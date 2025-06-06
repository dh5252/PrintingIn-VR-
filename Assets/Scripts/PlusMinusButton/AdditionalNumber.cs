using UnityEngine;
using TMPro;

public class AdditionalNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro worldText;
    private int num;
    private void Start()
    {
        num = 0;
        worldText.text = "0";
    }

    public void PlusNumber()
    {
        if (num == 9) return;
        ++num;
        worldText.text = num > 0 ? "+" + num.ToString() : num.ToString();
        Debug.Log(worldText.text);
    }

    public void MinusNumber()
    {
        if (num == -9) return;
        --num;
        worldText.text = num > 0 ? "+" + num.ToString() : num.ToString();
    }
}
