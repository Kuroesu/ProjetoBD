using UnityEngine;
using System.Collections;

public class Escudo_Bar : MonoBehaviour {
    public Escudo escudo;
    public SpriteRenderer backgroudDefesa;
    public SpriteRenderer barraDefesa;
    private int defesaTotal;

    // Use this for initialization
    void Start () {
        this.defesaTotal = this.escudo.defesaBase;
        this.backgroudDefesa.enabled = false;
        this.barraDefesa.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
        this.atualizarBarra();
	}

    public float porcToX(float porcent)
    {
        float x = (porcent-1)*1.5f;
        return x;
    }
    public void atualizarBarra()
    {
        if (this.escudo.getDefesaAtual() < this.defesaTotal) {
            this.backgroudDefesa.enabled = true;
            this.barraDefesa.enabled = true;
            float porcent = this.escudo.getDefesaAtual() / (float)this.defesaTotal;
            float x = this.porcToX(porcent);
            this.barraDefesa.transform.localPosition = new Vector2(this.porcToX(porcent), this.barraDefesa.transform.localPosition.y);
            this.barraDefesa.transform.localScale = new Vector2(porcent, barraDefesa.transform.localScale.y);
        }
        else {
            this.backgroudDefesa.enabled = false;
            this.barraDefesa.enabled = false;
        }
    }
}
