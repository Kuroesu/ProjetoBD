using UnityEngine;
using System;
using System.Collections;

public enum Estado_Do_Inimigo {
    idle,
    attack,
    morrer,
}


public class EnemyBehavior : MonoBehaviour {
    public GameObject objStatus;// '' Status do inimigo
    public GameObject objArma;
    public GameObject objBarraHP;//guarda o gameObject da barra de hp do inimigo
    public int delayAtaque;//Determina o tempo em que o inimigo fica parado após atingir um determinado número de ataques
    private float timer = 0;
    public short numDeAtaquesMax;//Determina quantos ataques o inimigo pode realizar antes de se cansar 
    public float distanciaMinimaDoPlayer;
    public Animator enemyAnimator;
    public float velocidade;//determina a velocidade em que o inimigo vai ser movimentar
    public int baseXp;//determina um incremento de xp. Para que mobs deem quantidades diferentes de xp; 
    public int level = 1;//determina o level do personagem
    private Status status;
    private PlayerBehavior player;//variavel que guarda o jogador.
    private Estado_Do_Inimigo estadoAtual;
    private float posicaoDir;
    private int numDeGolpes;//responsavel por indicar quantos golpes foram realizados pelo inimigo.
    private bool atrasarAtaque = false;
    private Vector3 posicaoInicial;//Guarda a posição inicial do mob
    private float qtXpTotal;//Determina quanto de xp o personagem precisa para upar
    private float qtXpAtual = 0;//determina quanto de xp o personagem possui atualmente
    private byte contMorte = 0;//Serve como um controle para detectar apenas uma morte do player(evita loop de levelUp)
    float distanciaDoPlayer;//pega a distancia do inimigo para o jogador
                            // Use this for initialization
    private Boolean estaDefendendo = false;

    void Start()
    {
        this.status = objStatus.GetComponent("Status") as Status;
        this.atribuirPontos();
        Arma armaInimigo = objArma.GetComponent("Arma") as Arma;
        armaInimigo.setPortador(objStatus);
        setEstado(Estado_Do_Inimigo.idle);
        this.qtXpTotal = this.determinarXp();
        this.posicaoInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        comportamento();
        contarTempo();
    }

    public void tomarDano(int dano)
    {
        dano = dano - status.defesa;
        if (dano <= 0) {
            dano = 1;//Dano mínimo 
        }
        HP_Bar barraHP = objBarraHP.GetComponent("HP_Bar") as HP_Bar;
        if (dano >= status.hpAtual) {
            status.hpAtual = 0;
        }
        else {
            status.hpAtual = status.hpAtual - dano;
        }
        barraHP.alterarHP();
    }
    //faz o inimigo volta a sua posicao de origin(por enquanto só no eixo x)
    private void voltarPosicaoInicial()
    {
        Vector3 posicaoAtual = transform.position;
            if ((int)posicaoAtual.x > (int)this.posicaoInicial.x) {
                transform.localScale = new Vector3(2, 2, 2);//faz o inimigo olhar na direçao do player
                transform.Translate(Vector3.left * velocidade * Time.deltaTime);
                posicaoAtual = transform.position;
            }
            else if ((int)posicaoAtual.x < (int)this.posicaoInicial.x) {
                transform.localScale = new Vector3(-2, 2, 2);//faz o inimigo olhar na direçao do player
                transform.Translate(Vector3.right * velocidade * Time.deltaTime);
                posicaoAtual = transform.position;
            }
            else {
                transform.localScale = new Vector3(2, 2, 2);//faz o inimigo olhar na direçao do player
        }
            
        }
    private void comportamento()
    {
        if (player != null) {
            distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);//pega a distancia do inimigo para o jogador
            switch (estadoAtual) {
                case Estado_Do_Inimigo.idle:
                    {
                        distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);//pega a distancia do inimigo para o jogador
                        if (distanciaDoPlayer < 100 && distanciaDoPlayer > 0) {
                            if (distanciaDoPlayer < distanciaMinimaDoPlayer) {
                                setEstado(Estado_Do_Inimigo.attack);
                            }
                            else {
                                seguirPlayer(true);
                            }

                        }
                        else {
                            seguirPlayer(false);
                        }
                        if (status.hpAtual <= 0) {
                            setEstado(Estado_Do_Inimigo.morrer);
                        }
                    }
                    break;
                case Estado_Do_Inimigo.attack:
                    {
                        if (distanciaDoPlayer >= distanciaMinimaDoPlayer || player.getStatus().hpAtual == 0) {//caso o player se afaste d+ durante um ataque o mob vai voltar a segui-lo
                            enemyAnimator.SetBool("attack", false);
                            setEstado(Estado_Do_Inimigo.idle);
                        }
                        else if (numDeGolpes >= numDeAtaquesMax) {//para os ataques quando numero de golpes deferidos for igual ao número máximo de golpes
                            enemyAnimator.SetBool("attack", false);
                            atrasarAtaque = true;
                        }
                        else {
                            enemyAnimator.SetBool("attack", true);//inicia a animação de ataque
                        }
                        if (status.hpAtual <= 0) {
                            setEstado(Estado_Do_Inimigo.morrer);
                        }

                        if(this.player.getEstado() == "morto" && this.contMorte==0) {
                            this.receberXp(this.player.getXp());
                            Debug.Log(this.player.getXp());
                            this.contMorte = 1;
                        }
                    }
                    break;

                case Estado_Do_Inimigo.morrer:
                    {
                        enemyAnimator.SetBool("morrer", true);
                        this.player.receberXp(this.getXp());//passa o xp para o player
                        Destroy(gameObject,1);//faz o inimigo desaparecer ao morrer. 1 indica que deve possui um delay de 1 segundo.
                    }
                    break;
            }
        }
        else {
            this.voltarPosicaoInicial();//caso não esteja vendo o player volte a posição inicial;
        }
    }

    void setEstado(Estado_Do_Inimigo novoEstado)
    {
        estadoAtual = novoEstado;
    }

    void seguirPlayer(bool seguir)
    {
        if (player.gameObject.transform.position.x < transform.position.x) {
            transform.Translate(Vector3.left * velocidade * Time.deltaTime);
            transform.localScale = new Vector3(2, 2, 2);//faz o inimigo olhar na direçao do player
        }
        else if (player.gameObject.transform.position.x > transform.position.x) {
            transform.localScale = new Vector3(-2, 2, 2);
            transform.Translate(Vector3.right * velocidade * Time.deltaTime);
        }


    }

    public string getEstado()
    {
        return estadoAtual.ToString();
    }
    //serve como um "timer" para indicar o tempo que o inimigo deve ficar sem atacar
    private void contarTempo()
    {
        if (atrasarAtaque) {
            timer += Time.deltaTime;
            if (timer >= delayAtaque) {
                numDeGolpes = 0;
                timer = 0;
                atrasarAtaque = false;
            }
        }
    }
    //Esse método é responsavel pela visão do inimigo
    //quando o player "colide"(ou seja entra) no campo de visão
    //do inimigo o inimigo iniciará a perseguição ao player
    //caso o player saia do seru campo de visão, ele não 
    //o seguirá mais
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player") {
            player = coll.gameObject.GetComponent("PlayerBehavior") as PlayerBehavior;
        }
    
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        //detecta o fim da colisão APENAS quando o player não estiver mais no campo de visão
        PolygonCollider2D[] poli = GetComponentsInChildren<PolygonCollider2D>() as PolygonCollider2D[];//pega o pollygonCollider2D(campo de visaõ)
        if (coll.gameObject.tag == "Player" && poli[0].IsTouching(coll)==false) {//Se o player não estiver mais no campo de visão, pare de seguir 
            player = null;
        }
    }
    //determina a quantidade de xp que o inimigo vai dropar ao morrer
    public float getXp()
    {
        float xpDropado = (100 * (Mathf.Log(this.level + 1))) + this.baseXp;
        return xpDropado;
    }

    public void incrementarNumGolpes()
    {
        this.numDeGolpes = this.numDeGolpes+1;
    }
    //distribui os status do inimigo automaticamente, em função do level.
    //por enquanto o calcula para os atributos  é bastante simples.
    //multiplica-se os status básicos pelo level do inimigo
    //futuramente rever isso(trocar por alguma função)
    private void atribuirPontos()
    {
        this.status.forca = this.status.forca * this.level;
        this.status.hp = this.status.hp * this.level;
        this.status.inteligencia = this.status.inteligencia * this.level;
        this.status.vitalidade = this.status.vitalidade * this.level;
        this.status.defesa = this.status.defesa * this.level;
        this.status.hpAtual = this.status.hp;
    }
    public int getLevel()
    {
        return this.level;
    }
    public Status getStatus()
    {
        return this.status;
    }

    private void levelUp()
    {
        this.level = this.level + 1;
        this.qtXpTotal = this.determinarXp();
    }


    private float determinarXp()
    {
        return this.qtXpTotal = 50 * (this.level * Mathf.Log10(this.level)) + 25;
    }

    public void receberXp(float xp)
    {
        this.qtXpAtual = qtXpAtual + xp;
        while (this.qtXpAtual >= this.qtXpTotal) {
            this.levelUp();
        }
    }
    public Boolean getDefendendo()
    {
        return this.estaDefendendo;
    }
    public void setDefendendo(Boolean booleano)
    {
        this.estaDefendendo = booleano;
    }

}
