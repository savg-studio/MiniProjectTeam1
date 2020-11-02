using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorDisplay : MonoBehaviour
{
    // Cache
    private Text text;

    // Value
    private uint maxArmor;
    private uint currentArmor;
    private string textFormat = "Armor: {0}/{1}";

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentArmor(uint amount)
    {
        currentArmor = amount;
        UpdateText();
    }

    public void SetMaxArmor(uint amount)
    {
        maxArmor = amount;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = string.Format(textFormat, currentArmor, maxArmor);
    }
}
