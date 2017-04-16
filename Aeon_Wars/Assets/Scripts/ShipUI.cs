using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    public Text ShipName;//don't want it changed out of ShipUI
    public Text ScoreUI;
    public Text Timer;
    public Image ShieldUI;
    public Image EnergyUI;
    private int _scoreValue;

    private float ellapsedtime;
    // Use this for initialization
    private void Awake()
    {
        _scoreValue = 0;
        ScoreUI.text = string.Format("Score: {0:0000000}", _scoreValue);
        gameObject.SetActive(false);
    }

    public void Register(Ship ShipObject)
    {
        ShipObject.ShipInstantiated += OnShipInstantiated;
        ShipObject.ShipUpdate += OnShipUpdate;
        ShipObject.ShipDestroyed += OnShipDestroyed;
        gameObject.SetActive(true);
        if (ShipObject is Fighter)
            ShieldUI.color = new Color32(120, 200, 255, 255);//32-bit color
        else if (ShipObject is Bomber)
            ShieldUI.color = Color.yellow;
        else if (ShipObject is Recon)
            ShieldUI.color = new Color32(255,150,255,255);
        else
            ShieldUI.color = Color.white;
    }

    private void OnShipInstantiated(Ship ShipObject)
    {
        _scoreValue = 0;
        ScoreUI.text = string.Format("Score: {0:0000000}", _scoreValue);
        ShipName.text = ShipObject.ShipName;
        ShieldUI.fillAmount = ShipObject.ShieldRatio;
        EnergyUI.fillAmount = ShipObject.EnergyRatio;
        ShipObject.ShipInstantiated -= OnShipInstantiated;
        ellapsedtime = 0.0f;//reset timer
        Timer.text = string.Format("Time: {0:00}:{1:00}", (int)(ellapsedtime / 60.0f), (int)(ellapsedtime % 60.0f));
    }

    private void OnShipUpdate(Ship ShipObject)
    {
        ShieldUI.fillAmount = ShipObject.ShieldRatio;
        EnergyUI.fillAmount = ShipObject.EnergyRatio;
        ellapsedtime += Time.deltaTime;
        Timer.text = string.Format("Time: {0:00}:{1:00}", (int)(ellapsedtime / 60.0f), (int)(ellapsedtime % 60.0f));
    }

    private void OnShipDestroyed(Ship ShipObject)
    {
        ShipName.text = "Game Over";
        ShieldUI.fillAmount = 0.0f;
        EnergyUI.fillAmount = 0.0f;
        ShipObject.ShipUpdate -= OnShipUpdate;
        ShipObject.ShipDestroyed -= OnShipDestroyed;
    }

    public void OnEnemySpawned(Enemy e)
    {
        e.EnemyDestroyed += OnEnemyDestroyed;
    }

    public void OnEnemyDestroyed(Enemy e)
    {
        if (e.Killed)//if enemy is killed
        {
            _scoreValue += e.EnemyValue;//increment the score
            ScoreUI.text = string.Format("Score: {0:0000000}", _scoreValue);
        }
        e.EnemyDestroyed -= OnEnemyDestroyed;
    }
}