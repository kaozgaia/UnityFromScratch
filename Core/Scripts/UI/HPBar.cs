using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

    private Image _bar;
    [SerializeField]
    private CanvasGroup parentCG;

	// Use this for initialization
	void Start () {
        _bar = GetComponent<Image>();
        
	}

    public void ResetBar()
    {
        if (parentCG != null) parentCG.alpha = 0f;
        if (_bar)
        {
            _bar.type = Image.Type.Filled;
            _bar.fillAmount = 1f;
        }
    }
	
    public void ReduceHealthBar(int currentHealth, int totalHealth)
    {
        if (parentCG != null) parentCG.alpha = 1f;
        if (totalHealth == 0) return;
        float healthPercentage = (float)currentHealth / (float)totalHealth;
        if(_bar)
            _bar.fillAmount = healthPercentage;

    }

}
