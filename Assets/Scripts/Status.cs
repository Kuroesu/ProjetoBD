using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	private int id;
	public int forca;
	public int defesa;
	public int velocidade;
	public int inteligencia;
	public int magia;
	public int ataque;
	public int hp;
	public int hpAtual;
	public int mp;
	public int mpAtual;
    public static Status defaut = new Status(2,2,2,2,2,2,2,2);

    public Status(int forc, int def, int velo, int inte, int mag, int ataq, int MP, int HP)
    {
        forca = forc;
        defesa = def;
        velocidade = velo;
        inteligencia = inte;
        magia = mag;
        ataque = ataq;
        hp = HP;
        hpAtual = HP;
        mp = MP;
        mpAtual = MP;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
