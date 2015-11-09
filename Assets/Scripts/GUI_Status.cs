using UnityEngine;
using System.Collections;

public class GUI_Status : MonoBehaviour {

    /* Essa classe é responsavel por exibir uma tela onde o jogador poderá evoluir o personagem*/

    public PlayerBehavior player;

    private int forca;
    private int inteligencia;
    private int vitalidade;

    private bool exibir = false;
    private int altura = 300;
    private int largura = 500;

    private bool flag = false;

    private int pontosParaDistribuir;

	// Use this for initialization
	void Start () {
        this.forca = player.getStatusOriginais()[0];
        this.vitalidade = player.getStatusOriginais()[1];
        this.inteligencia = player.getStatusOriginais()[2];

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U)) {
            this.exibir = true;
        }
    }

    void OnGUI()
    {
        if (this.exibir) {
            this.evoluirPersonagem(0, this.player.getPontosParaDistribuir());
        }
    }
        


    public void evoluirPersonagem(int windowID, int pontosParaDistribuir)
    {
        if (this.flag==false) {
            this.pontosParaDistribuir = this.player.getPontosParaDistribuir();
        }
        this.flag = true;
        GUI.Box(new Rect(15, 35, 100, 25), "Pontos");
        GUI.Box(new Rect(115, 35, 50, 25), this.pontosParaDistribuir + "");

        GUI.Box(new Rect(15, 70, 100, 25), "Força");
        telaIncremento(115, 70, this.pontosParaDistribuir, "forca");

        GUI.Box(new Rect(15, 105, 100, 25), "Inteligencia");
        telaIncremento(115, 105, this.pontosParaDistribuir, "inteligencia");

        GUI.Box(new Rect(15, 140, 100, 25), "Vitalidade");
        telaIncremento(115, 140, this.pontosParaDistribuir,"vitalidade");

        GUI.Box(new Rect(230, 35, 100, 25), "HP");
        GUI.Box(new Rect(330, 35, 50, 25), this.player.getStatus().hp + "");
        GUI.Box(new Rect(230, 70, 100, 25), "MP");
        GUI.Box(new Rect(330, 70, 50, 25), this.player.getStatus().mp + "");
        GUI.Box(new Rect(230, 105, 100, 25), "Ataque");
        GUI.Box(new Rect(330, 105, 50, 25), this.player.getStatus().ataque + "");
        GUI.Box(new Rect(230, 140, 100, 25), "Magia");
        GUI.Box(new Rect(330, 140, 50, 25), this.player.getStatus().magia + "");
        GUI.Box(new Rect(230, 175, 100, 25), "Defesa");
        GUI.Box(new Rect(330, 175, 50, 25), this.player.getStatus().defesa + "");

        if (GUI.Button(new Rect(15, 200, 100, 30), "Salvar")) {//altera os atributos do player
            this.player.getStatus().distribuirPontos(this.forca, this.vitalidade, this.inteligencia);
            this.player.setStatusOriginais(this.forca, this.vitalidade, this.inteligencia);
            this.player.setPontosParaDistribuir(this.pontosParaDistribuir);
        }
        if (GUI.Button(new Rect(115, 200, 100, 30), "Cancelar")) { //volta tela selecao de personagens
            this.exibir = false;
            this.flag = false;
        }
    }

    void telaIncremento(int W, int H, int pontosParaDistribuir , string atributo)
    {
        switch (atributo) {
            case "forca":
                {
                    GUI.Box(new Rect(W, H, 50, 25), this.forca + "");
                    if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                        if (this.pontosParaDistribuir > 0) {
                            this.forca++;
                            this.pontosParaDistribuir--;
                        }
                    }
                    if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                        if (this.forca > this.player.getStatusOriginais()[0]) {
                            this.forca--;
                            this.pontosParaDistribuir++;
                        }
                    }
                }
                break;
            case "inteligencia":
                {
                    GUI.Box(new Rect(W, H, 50, 25), this.inteligencia + "");
                    if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                        if (this.pontosParaDistribuir > 0) {
                            this.inteligencia++;
                            this.pontosParaDistribuir--;
                        }
                    }
                    if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                        if (this.inteligencia > this.player.getStatusOriginais()[2]) {
                            this.inteligencia--;
                            this.pontosParaDistribuir++;
                        }
                    }
                }
                break;
            case "vitalidade":
                {
                    GUI.Box(new Rect(W, H, 50, 25), this.vitalidade + "");
                    if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                        if (this.pontosParaDistribuir > 0) {
                            this.vitalidade++;
                            this.pontosParaDistribuir--;
                        }
                    }
                    if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                        if (this.vitalidade > this.player.getStatusOriginais()[1]) {
                            this.vitalidade--;
                            this.pontosParaDistribuir++;
                        }
                    }
                }
                break;
        }
        this.player.getStatus().distribuirPontos(this.forca, this.vitalidade, this.inteligencia);
    }
}
