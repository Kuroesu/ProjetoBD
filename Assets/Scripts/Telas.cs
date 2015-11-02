using UnityEngine;
using System.Collections;
public enum TipoTelas {
    login,
    cadastro,
    selecaoPersonagem,
    criacaoPersonagem,
}

public class Telas : MonoBehaviour {
    private ConexaoBanco banco;
    private TipoTelas tipoTela;
    private Rect windowRect;
    public float altura;
    public float largura;
    //telas login
    private string _login = "";
    private string _senha = "";
    //tela cadastro
    private string _confirmaSenha = "";
    private bool _verificar;
    //tela selecao de personagens
    private ArrayList listaPersonagens;
    private ArrayList listaStatus;
    private float lastClick = 0;
    private string statusText;
    private string personagemSelecionado;
    public string jogador;
    //tela criacao de personagem
    private string nomePersonagem = "";
    private int maxPontos = 10;
    private int forca = 0;
    private int inteligencia = 0;
    private int vitalidade = 0;
    private int HP = 0;
    private int MP = 0;
    private int ataque = 0;
    private int magia = 0;
    private int defesa = 0;
    private Status status;

    void Start() {
        tipoTela = TipoTelas.login;
    }
    void Update() {
        windowRect = new Rect((Screen.width / 2) - (largura / 2), (Screen.height / 2) - (altura / 2), largura, altura);
    }

    void OnGUI() {
        switch (tipoTela) {
            case TipoTelas.login:
            {
                altura = 150;
                largura = 300;
                windowRect = GUI.Window(0, windowRect, Login, "Login");
            }
            break;
            case TipoTelas.cadastro:
            {
                altura = 200;
                largura = 350;
                windowRect = GUI.Window(0, windowRect, Cadastro, "Cadastro");
                if (_verificar) {
                    GUI.Label((new Rect((Screen.width / 2) - 180, Screen.height - 30, 360, 30)), "as senhas nao sao iguais ou login ja foi utilizado");
                }
            }
            break;
            case TipoTelas.criacaoPersonagem:
            {
                altura = 300;
                largura = 500;
                windowRect = GUI.Window(0, windowRect, criarPersonagem, "Criar Personagens");
            }
            break;
            case TipoTelas.selecaoPersonagem:
            {
                altura = 300;
                largura = 500;
                windowRect = GUI.Window(0, windowRect, listarPersonagens, "Seleção de Personagens");
            }
            break;
        }
    }

    void Login(int windowID) {

        GUI.Box(new Rect(10, 20, largura - 20, altura - 30), "");

        GUI.Box(new Rect(15, 35, 100, 25), "Login");
        GUI.Box(new Rect(15, 65, 100, 25), "Senha");

        _login = GUI.TextField(new Rect(largura - 175, 35, 160, 25), _login, (20));
        _senha = GUI.PasswordField(new Rect(largura - 175, 65, 160, 25), _senha, "*"[0], (10));

        // rect(mover horizontal,mover vertical,largura,altura)
        if (GUI.Button(new Rect(15, altura - 45, 100, 30), "Login")) {

            banco = new ConexaoBanco();
            if (banco.Logar(_login, _senha)) {
                print("login bem sucedido");
                jogador = _login;
                tipoTela = TipoTelas.selecaoPersonagem;
            }
        }

        if (GUI.Button(new Rect(largura - 115, altura - 45, 100, 30), "Cadastro")) {
            print("Cadastro");
            tipoTela = TipoTelas.cadastro;
            _login = "";
            _senha = "";
        }

    }

    void Cadastro(int windowID) {

        GUI.Box(new Rect(10, 20, largura - 20, altura - 30), "");
        //labels
        GUI.Box(new Rect(15, 35, 140, 25), "Login");
        GUI.Box(new Rect(15, 65, 140, 25), "Senha");
        GUI.Box(new Rect(15, 95, 140, 25), "Confirmar senha");

        //campos de texto
        _login = GUI.TextField(new Rect(largura - 175, 35, 160, 25), _login, (20));
        _senha = GUI.PasswordField(new Rect(largura - 175, 65, 160, 25), _senha, "*"[0], (10));
        _confirmaSenha = GUI.PasswordField(new Rect(largura - 175, 95, 160, 25), _confirmaSenha, "*"[0], (10));

        // rect(deslocamento horizontal,deslocamento vertical,largura,altura)
        if (GUI.Button(new Rect(15, altura - 45, 100, 30), "Voltar")) {
            tipoTela = TipoTelas.login;
            print("login");
        }

        if (GUI.Button(new Rect(largura - 115, altura - 45, 100, 30), "Cadastrar")) {
            if (_senha == _confirmaSenha) {
                banco = new ConexaoBanco();
                banco.inserirJogador(_login, _senha);
                _verificar = false;
                tipoTela = TipoTelas.login;
                print("cadastrado");
            } else {
                _verificar = true;
                print("nao cadastrado");
                _login = "";
                _senha = "";
                _confirmaSenha = "";
            }


        }

    }

    void listarPersonagens(int windowID) {
        nomePersonagem = "";
        banco = new ConexaoBanco();
        listaPersonagens = banco.recuperarPersonagem(jogador);

        if (listaPersonagens == null) {
            tipoTela = TipoTelas.criacaoPersonagem;
        } else {

            for (int i = 0; i < listaPersonagens.Count; i += 2) {

                if (GUI.Button(new Rect(10, 20 + 30 * (i / 2), 120, 30), listaPersonagens[i] + " - " + listaPersonagens[i + 1])) {
                    if (Time.time - lastClick < 0.3) {
                        //entra no jogo com o personagem selecionado se dar um "double click"
                        PlayerPrefs.SetString("Personagem", personagemSelecionado);
                        Application.LoadLevel(0);
                    } else {
                        //um click exibe o status do personagem e ativa botão de excluir personagem
                        personagemSelecionado = listaPersonagens[i].ToString();
                        print(personagemSelecionado);
                        listaStatus = banco.recuperarStatus(personagemSelecionado);
                        statusText = "Força:" + listaStatus[0] +
                                    "\nDefesa:" + listaStatus[1] +
                                    "\nVitalidade" + listaStatus[2] +
                                    "\nInteligencia" + listaStatus[3] +
                                    "\nMagia" + listaStatus[4] +
                                    "\nAtaque" + listaStatus[5] +
                                    "\nMP" + listaStatus[6] +
                                    "\nHP" + listaStatus[7];
                    }
                    lastClick = Time.time;

                }
            }
            GUI.Box(new Rect(200, 20, 200, 200), statusText);
        }

        if (GUI.Button(new Rect(15, altura - 45, 100, 30), "Criar")) {   //vai para a tela de criacao de personagens 
            tipoTela = TipoTelas.criacaoPersonagem;
        }
        if (GUI.Button(new Rect(120, altura - 45, 100, 30), "Deletar")) {   //vai para a tela de criacao de personagens
            banco.deletarPersonagem(personagemSelecionado);
            print("Personagem deletado");
        }
        if (GUI.Button(new Rect(largura - 115, altura - 45, 100, 30), "Voltar")) {   //volta tela de login
            _senha = "";
            tipoTela = TipoTelas.login;
        }
    }

    void criarPersonagem(int windowID) {
        GUI.Box(new Rect(15, 35, 100, 25), "nome");
        nomePersonagem = GUI.TextField(new Rect(120, 35, 100, 25), nomePersonagem, (20));

        GUI.Box(new Rect(15, 70, 100, 25), "Pontos");
        GUI.Box(new Rect(115, 70, 50, 25), this.maxPontos + "");

        GUI.Box(new Rect(15, 105, 100, 25), "Força");
        telaIncremento(115, 105, maxPontos,"forca");

        GUI.Box(new Rect(15, 140, 100, 25), "Inteligencia");
        telaIncremento(115, 140, maxPontos, "inteligencia");

        GUI.Box(new Rect(15, 175, 100, 25), "Vitalidade");
        telaIncremento(115, 175, maxPontos, "vitalidade");

        this.HP = this.vitalidade * 10;
        this.MP = this.inteligencia * 10;
        this.ataque = this.forca * 2;
        this.magia = this.inteligencia * 2;
        this.defesa = this.forca + this.vitalidade * 2;

        GUI.Box(new Rect(230, 35, 100, 25), "HP");
        GUI.Box(new Rect(330, 35, 50, 25),  this.HP+"");
        GUI.Box(new Rect(230, 70, 100, 25), "MP");
        GUI.Box(new Rect(330, 70, 50, 25),  this.MP+"");
        GUI.Box(new Rect(230, 105, 100, 25), "Ataque");
        GUI.Box(new Rect(330, 105, 50, 25), this.ataque+"");
        GUI.Box(new Rect(230, 140, 100, 25), "Magia");
        GUI.Box(new Rect(330, 140, 50, 25), this.magia+"");
        GUI.Box(new Rect(230, 175, 100, 25), "Defesa");
        GUI.Box(new Rect(330, 175, 50, 25),this.defesa+"");

        if (GUI.Button(new Rect(15, altura - 45, 100, 30), "Criar")) {//salva personagem criado no banco de dados;
            status = new Status(this.forca, this.vitalidade, this.inteligencia);
            banco.inserirPersonagem(nomePersonagem, jogador, status);
            tipoTela = TipoTelas.selecaoPersonagem;
        }
        if (GUI.Button(new Rect(largura - 115, altura - 45, 100, 30), "Voltar")) { //volta tela selecao de personagens
            tipoTela = TipoTelas.selecaoPersonagem;
        }
    }

    void telaIncremento(int W, int H, int maxPontos,string atributo) {
        switch (atributo) {
            case "forca":{
                GUI.Box(new Rect(W, H, 50, 25), this.forca + "");
                if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                    if (this.maxPontos > 0) {
                        this.forca++;
                        this.maxPontos--;
                    }
                }
                if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                    if (this.forca > 0) {
                        this.forca--;
                        this.maxPontos++;
                    }
                }
            }
            break;
            case "inteligencia": {
                GUI.Box(new Rect(W, H, 50, 25), this.inteligencia + "");
                if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                    if (this.maxPontos > 0) {
                        this.inteligencia++;
                        this.maxPontos--;
                    }
                }
                if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                    if (this.inteligencia> 0) {
                        this.inteligencia--;
                        this.maxPontos++;
                    }
                }
            }
            break;
            case "vitalidade":{
                GUI.Box(new Rect(W, H, 50, 25), this.vitalidade + "");
                if (GUI.Button(new Rect((W + 50), H, 25, 25), "+")) {
                    if (this.maxPontos > 0) {
                        this.vitalidade++;
                        this.maxPontos--;
                    }
                }
                if (GUI.Button(new Rect((W + 75), H, 25, 25), "-")) {
                    if (this.vitalidade > 0) {
                        this.vitalidade--;
                        this.maxPontos++;
                    }
                }
            }
            break;
        }
    }

 
}
