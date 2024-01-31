using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;


 
public class RoboltInteractions : MonoBehaviour {

    private static readonly int pauseOffHashID = Animator.StringToHash("pauseOff");
    private static readonly int gameOverOffHashID = Animator.StringToHash("gameOverOff");

    public enum LifeUpdateType { INCREASE, DECRESE }

    // attributi classe
    public String[] lifesArray;
    [SerializeField] private int currentLifes = 0;
    [SerializeField] private int currentBoltKeys = 0;
    [SerializeField] private int currentBoltCoins = 0;
    [SerializeField] private int currentSteamTanks = 0;
    private bool isLifeUpable;
    private bool isPause;
    private bool isPopUp;
    private bool isGameOver;
    private bool isStartUpdateUI;
    private bool isPlayerInvulnerable;
    private bool isStartUpdateLife;
    private bool isLifeUpAnimation;
    private bool isLifeDownAnimation;
    private List<Image> lifeIconList;

    // references
    public Animator animPause;
    //public Animator animGameOver;
    //public Animator animCheckPoint;
    public GameObject gameOverScreen;
    public UniqueItemsManagerZone1 uniqueItemsManager;
    public DisableFirstSelectedSound disableFirstSelectedSound;
    public Transform followCamera;

    // Audio
    public AudioSource coins;
    public AudioSource coins2;
    public AudioSource coins3;
    public AudioSource pop;
    public AudioSource checkPoint;
    public AudioSource soundTrack;
    public AudioSource gameOverSound;
    public AudioSource shockSound;

    //public Animator blackScreenAnimator;
    private RoboltController roboltController;

    // elementi UI
    public GridLayoutGroup lifeGrid;

    public GameObject boltKeysCounter;
    private TextMeshProUGUI boltKeysText;
    private TextMeshProUGUI boltKeysTextShadow;

    public GameObject boltCoinsCounter;
    private TextMeshProUGUI boltCoinsText;
    private TextMeshProUGUI boltCoinsTextShadow;

    public GameObject steamTanksCounter;
    private TextMeshProUGUI steamTanksText;
    private TextMeshProUGUI steamTanksTextShadow;
    public Image LifeUpImage;
    public Image LifeUpDisableImage;

    public GameObject pauseMenu;
    public GameObject pauseEventSystem;

    // Lista mesh
    public List<GameObject> objectMeshList;

    // Input comandi
    [SerializeField] protected InputActionReference option;
    [SerializeField] protected InputActionReference r1;
    //[SerializeField] protected InputActionReference triangle;
    //[SerializeField] protected InputActionReference submit;

    private void Start()
    {
        isLifeUpable = false;
        isPause = false;
        isPopUp = false;
        isGameOver = false;
        isStartUpdateUI = true;
        isPlayerInvulnerable = false;
        isStartUpdateLife = true;
        isLifeUpAnimation = false;
        isLifeDownAnimation = false;
        lifeIconList = new List<Image>();

        lifesArray = new string[Const.MAX_LIFES];
        SetStartLife();
        InitUI();
        roboltController = transform.parent.GetComponent<RoboltController>();
        
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0.15f, 0.15f);

        InputSystem.PauseHaptics();

        // leggo eventuali boltCoins e boltKeys già presi dal player nella sessione prima del game over
        StartCoroutine(ReadKeyCoins());

    }

    void Update() {

        Cheats();
        CheckLifeUpable();
        CheckLifeUpCommand();
        CheckPause();
        HandleLifeAnimation();
    }

    // metodo che, se premuto "option" e non in gameOver, mette in pausa il gioco; se già in pausa, torna al gioco
    private void CheckPause() 
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || option.action.triggered) && !isGameOver) {
            //if ( (Input.GetKeyDown(KeyCode.Escape) || option.action.ReadValue<float>() != 0) && !isGameOver) {

            if (!isPause)
                PauseOnAnim();
            else
                PauseOffAnim();
        }
    }

    IEnumerator HandleGameOver() 
    {
        // scrivo nel file json per aggiornare lo stato prima del gameover
        uniqueItemsManager.WriteDataToJson();

        // nel caso in cui il player sia in animazione di shocked, aspetto che finisca
        yield return new WaitUntil(() => !roboltController.GetIsShocked());

        // nel caso in cui il player sia in animazione di caduta, aspetto che finisca
        yield return new WaitUntil(() => !roboltController.GetIsFalling());

        roboltController.StopAnimantionsAndMovements();

        bool isFallingToDeath = roboltController.GetIsFallingToDeath();

        if (!isFallingToDeath)
        {
            roboltController.DeathAnimation();
            //StartCoroutine(GameOverRumble());
        }

        if (isFallingToDeath)
            roboltController.SetIsStoppedPlayerSessions(true);

        if(isFallingToDeath)
            yield return new WaitForSeconds(Const.GAMEOVER_FALL_WAIT_DURATION);
        else
            yield return new WaitForSeconds(Const.GAMEOVER_HIT_WAIT_DURATION);

        // l'animazione di comparsa della schermata game over parte in automatico appena viene attivato l'oggetto
        gameOverScreen.SetActive(true);
        gameOverSound.Play();
        soundTrack.Stop();

        // dopo timeout, schermata gameover

        //GameOverOnAnim();
    }

    IEnumerator GameOverRumble()
    {
        yield return new WaitForSeconds(1.5f);
        Rumble(Const.RUMBLE_DEATH_DURATION);
    }

    ////////////////////////////////////// PAUSA

    // chimata dal bottone Resume della UI
    public void ResumeGameFromUI()
    {
        PauseOffAnim();
    }
    // mette il gioco in pausa
    private void PauseOnAnim()
    {
        pauseEventSystem.SetActive(true);

        // l'animazione di comparsa menu di pausa parte in automatico appena il menu viene attivato
        pauseMenu.SetActive(true);
    }

    // quando di preme di nuovo Option oppure dal menu si preme Riprendi, annulla la pausa e a fine animazione viene chiamata PauseOff dall'animator
    private void PauseOffAnim() 
    {
        disableFirstSelectedSound.ResetAudioListener();

        // riattivo il tempo qui in modo che l'animator possa leggere il trigger
        Time.timeScale = 1;
        animPause.SetTrigger(pauseOffHashID);
        pauseEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    // chimata dall'animator del menu pausa appena finisce la transizione di entrata
    public void PauseOn()
    {
        Time.timeScale = 0;

        // disabilito i controlli del player, ma rimangono attivi quelli della UI
        roboltController.GetComponent<PlayerInput>().enabled = false;
        isPause = true;
    }

    // chiamata dall'animator del menu pausa quando ha finito la transizione di uscita
    public void PauseOff()
    {
        pauseEventSystem.SetActive(false);
        pauseMenu.SetActive(false);
        roboltController.GetComponent<PlayerInput>().enabled = true;
        isPause = false;
    }

    ////////////////////////////////////// POPUP

    // chimata dall'Animator del Popup
    public void PopUpOn() 
    {
        Time.timeScale = 0;

        // disabilito i controlli del player, ma rimangono attivi quelli della UI
        roboltController.GetComponent<PlayerInput>().enabled = false;

        isPopUp = true;

    }

    ////////////////////////////////////// CONTROLLO VITE

    // metodo che gestisce l'incremento di vita consumando steamTanks
    private void CheckLifeUpCommand() {

        // se è stato premuto H e la vita è aumentabile
        if (r1.action.triggered && isLifeUpable) {

            currentSteamTanks -= Const.STEAM_TANK_NEEDED;
            UpdateSteamTanksUI();

            UpdateLife(1, LifeUpdateType.INCREASE, false);

            steamTanksCounter.GetComponent<Animator>().SetTrigger("GetSteamTank");

            // reset se sono rimasti meno di 10 steamTank
            if (currentSteamTanks < Const.STEAM_TANK_NEEDED) {
                isLifeUpable = false;
                LifeUpImage.enabled = false;
                LifeUpDisableImage.enabled = false;
            }
        }

    }

    // controlla le condizioni per consumare steamTank e aumentare la vita
    private void CheckLifeUpable() {

        if (currentSteamTanks >= Const.STEAM_TANK_NEEDED && !isLifeUpable) {

            if (currentLifes < Const.MAX_LIFES) {
                isLifeUpable = true;
                LifeUpImage.enabled = true;
                LifeUpDisableImage.enabled = false;
            } else {
                LifeUpImage.enabled = false;
                LifeUpDisableImage.enabled = true;
            }
        }
    }

    // metodo che imposta le vite iniziali nella UI e nel contatore interno, salvando in lifesArray[] i nomi di tutti le Image delle vite
    private void SetStartLife() {

        bool jumpParent = true;
        foreach (Transform child in lifeGrid.GetComponentsInChildren<Transform>()) {

            // salto il primo child che è il parent stesso
            if (jumpParent)
                jumpParent = false;
            else {
                lifesArray[currentLifes] = child.name;
                currentLifes++;
            }
        }

        // imposto il numero di vite iniziali 
        UpdateLife(Const.MAX_LIFES - Const.START_LIFES, LifeUpdateType.DECRESE, false);
        isStartUpdateLife = false;

        /* for (int i = 0; i < START_LIFES; i++) {
             GameObject lifeIcon = Instantiate(lifeIconPrefab, Vector3.zero, Quaternion.identity) as GameObject;
             lifeIcon.transform.SetParent(lifeGrid.transform);
             currentLifes++;
        } */
    }

    // metodo che aggiorna le vite nella UI e nel contatore interno
    public void UpdateLife(int amount, LifeUpdateType updateType, bool isRedPulse)
    {

        // se è un decremento di vita, non quello a inizio partita, e se il player non è in fase di shocked
        if (updateType == LifeUpdateType.DECRESE && !isStartUpdateLife && !roboltController.GetIsShocked())
            Rumble(Const.RUMBLE_HIT_DURATION);


        for (int i = 0; i < amount; i++)
        {

            // incrementa vita
            if (updateType == LifeUpdateType.INCREASE && currentLifes < Const.MAX_LIFES)
            {

                Transform t = lifeGrid.transform.Find(lifesArray[currentLifes]);
                /////////////////t.gameObject.SetActive(true);

                lifeIconList.Add(t.GetComponent<Image>());

                currentLifes++;

                if (i == amount - 1)              
                    isLifeUpAnimation = true;

            }

            // decrementa vita
            else if (updateType == LifeUpdateType.DECRESE && currentLifes > 1)
            {

                Transform t = lifeGrid.transform.Find(lifesArray[currentLifes - 1]);
                ////////////////t.gameObject.SetActive(false);

                lifeIconList.Add(t.GetComponent<Image>());

                //t.GetComponent<Image>().enabled = false;

                currentLifes--;

                // se è l'ultima iterazione
                if (i == amount - 1)
                {
                    isLifeDownAnimation = true;

                    if (!isStartUpdateLife && isRedPulse)
                        StartCoroutine(SetPlayerInvulnerable(Const.INVULNERABLE_SEC));
                }

            }
            else if (updateType == LifeUpdateType.DECRESE && currentLifes == 1)
            {

                Transform t = lifeGrid.transform.Find(lifesArray[currentLifes - 1]);                
                lifeIconList.Add(t.GetComponent<Image>());
                isLifeDownAnimation = true;
                currentLifes--;

                roboltController.SetIsDead(true);

                StartCoroutine(HandleGameOver());
                
            }
        }
    }

    private void HandleLifeAnimation()
    {
        if (lifeIconList.Count > 0)
        {
            // decremento vita, faccio sparire l'icona
            if (isLifeDownAnimation)
            {
                foreach (Image icon in lifeIconList)
                {
                    icon.fillAmount -= 1.0f / Const.FILL_LIFE_ICON_DURATION * Time.deltaTime;
                }

                // quando l'icona è sparita
                if (lifeIconList[0].fillAmount <= 0f)
                {
                    foreach (Image icon in lifeIconList)
                    {
                        icon.fillAmount = 0f;
                    }
                    isLifeDownAnimation = false;

                    lifeIconList.Clear();
                }
            }

            // incremento la vita, faccio comparire l'icona
            else if (isLifeUpAnimation)
            {
                foreach (Image icon in lifeIconList)
                {
                    icon.fillAmount += 1.0f / Const.FILL_LIFE_ICON_DURATION * Time.deltaTime;
                }

                // quando l'icona è piena
                if (lifeIconList[0].fillAmount >= 1f)
                {
                    foreach (Image icon in lifeIconList)
                    {
                        icon.fillAmount = 1f;
                    }
                    isLifeUpAnimation = false;

                    lifeIconList.Clear();
                }

            }
            else
                throw new Exception("Condizioni true/false non previste in HandleLifeAnimation()");
        }
        //fillAmount -= 1.0f / Const.FILL_BUTTON_TIME * Time.deltaTime;


        // reste Image = null;
    }

    ////////////////////////////////////// UI

    // Metodo che inizializza i text della UI relativi ai contatori boltKeys, boltCoins, steamTanks
    private void InitUI() {

        // inizializzo testo e ombra del contatore boltKeys
        Transform t = boltKeysCounter.transform.Find("Current");
        boltKeysText = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        t = boltKeysCounter.transform.Find("CurrentShadow");
        boltKeysTextShadow = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

        // inizializzo testo e ombra del contatore boltCoins
        t = boltCoinsCounter.transform.Find("Current");
        boltCoinsText = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        t = boltCoinsCounter.transform.Find("CurrentShadow");
        boltCoinsTextShadow = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

        // inizializzo testo e ombra del contatore steamTanks
        t = steamTanksCounter.transform.Find("Current");
        steamTanksText = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        t = steamTanksCounter.transform.Find("CurrentShadow");
        steamTanksTextShadow = t.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

    }

    // animazione zoom e aggiornamento contatore UI 
    private void UpdateBoltKeysUI() 
    {
        if(!isStartUpdateUI)
            boltKeysCounter.GetComponent<Animator>().SetTrigger("Zoom");
        boltKeysText.text = currentBoltKeys.ToString();
        boltKeysTextShadow.text = currentBoltKeys.ToString();
    }

    // animazione zoom e aggiornamento contatore UI 
    private void UpdateBoltCoinsUI() 
    {
        if(!isStartUpdateUI)
            boltCoinsCounter.GetComponent<Animator>().SetTrigger("Zoom");
        boltCoinsText.text = currentBoltCoins.ToString();
        boltCoinsTextShadow.text = currentBoltCoins.ToString();
    }

    // animazione zoom e aggiornamento contatore UI 
    private void UpdateSteamTanksUI() 
    {
        steamTanksCounter.GetComponent<Animator>().SetTrigger("Zoom");
        steamTanksText.text = currentSteamTanks.ToString();
        steamTanksTextShadow.text = currentSteamTanks.ToString();
    }

    ////////////////////////////////////// GETTER

    public int GetCurrentBoltKeys()
    {
        return currentBoltKeys;
    }

    ////////////////////////////////////// TRIGGER

    // gestione collisioni con oggetti trigger
    public void OnTriggerEnter(Collider other) {

        switch (other.tag) {

            case "BoltKey":

                currentBoltKeys++;
                other.GetComponent<Collider>().enabled = false;

                DoubleRumble(Const.RUMBLE_ITEM_DURATION);
                coins2.Play();

                UpdateBoltKeysUI();

                // leggo l'id del boltKey e aggiorno lo stato per il json di ripristino gameover
                int keyId = other.gameObject.GetComponent<UniqueItem>().GetId();
                uniqueItemsManager.UpdateBoltKeyStatus(keyId, false);

                // animazione boltkey accelera rotazione e sparisce
                other.gameObject.GetComponent<Animator>().SetTrigger("Disappear");

                break;

            case "BoltCoin":

                currentBoltCoins++;
                other.GetComponent<Collider>().enabled = false;

                DoubleRumble(Const.RUMBLE_ITEM_DURATION);
                coins3.Play();

                UpdateBoltCoinsUI();

                // leggo l'id del boltCoin e aggiorno lo stato per il json di ripristino gameover
                int coinId = other.gameObject.GetComponent<UniqueItem>().GetId();
                uniqueItemsManager.UpdateBoltCoinStatus(coinId, false);

                // animazione boltCoin accelera rotazione e si ingrandisce
                other.gameObject.GetComponent<Animator>().SetTrigger("GetCoin");

                break;

            case "SteamTank":

                currentSteamTanks++;
                other.GetComponent<Collider>().enabled = false;

                DoubleRumble(Const.RUMBLE_ITEM_DURATION);
                coins.Play();
                UpdateSteamTanksUI();

                // animazione boltCoin accelera rotazione e si ingrandisce
                other.gameObject.GetComponent<Animator>().SetTrigger("GetSteamTank");

                break;

            case "SteamTankX5":

                if (currentLifes < Const.MAX_LIFES)
                    UpdateLife(1, LifeUpdateType.INCREASE, false);
                else
                {
                    currentSteamTanks += 5;
                    UpdateSteamTanksUI();
                }

                other.GetComponent<Collider>().enabled = false;

                DoubleRumble(Const.RUMBLE_ITEM_DURATION);
                coins.Play();

                // animazione boltCoin accelera rotazione e si ingrandisce
                other.gameObject.GetComponent<Animator>().SetTrigger("GetSteamTank");

                break;

            case "TimeTrigger":

                // attivo la sfida a tempo
                other.GetComponent<TimeTriggerBase>().StartTimeTrial();

                other.GetComponent<Collider>().enabled = false;

                DoubleRumble(Const.RUMBLE_ITEM_DURATION);
                coins3.Play();

                other.gameObject.GetComponent<Animator>().SetTrigger("Disappear");

                break;

            case "WeakPoint":
 
                // colpisci il nemico
                other.GetComponent<WeakPointInterface>().HitEnemy();

                Rumble(Const.RUMBLE_HIT_DURATION);

                // controlli se ha ancora vita
                int enemyLife = other.GetComponent<WeakPointInterface>().GetEnemyCurrentLife();
                if (enemyLife <= 0)
                {
                    // distruggo il nemico
                    other.GetComponent<WeakPointInterface>().DestroyAnimation();
                }

                break;

            case "JumpNotAllowed":

                roboltController.SetIsJumpAllowed(false);
                break;

            case "JumpAllowed":

                roboltController.SetIsJumpAllowed(true);
                break;

            case "JetPack":

                roboltController.SetIsJetPackSession(true);
                roboltController.ResetInAirAnimation();
                roboltController.IdleAnimation();

                Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, other.transform.eulerAngles.y, transform.eulerAngles.z);

                transform.rotation = Quaternion.Euler(eulerRotation);

                Destroy(other.gameObject, 0);
                //other.GetComponent<Animator>().SetTrigger("PlayerControl");
                break;

            case "PlatformJumpJet":

                float jumpHeight = other.GetComponent<PlatformJumpJet>().GetJumpHeight();
                roboltController.SetJumpHeight(jumpHeight);

                float jumpTime = other.GetComponent<PlatformJumpJet>().GetMaxJumpTime();
                roboltController.SetMaxJumpTime(jumpTime);

                roboltController.SetCurrentPlatformJumpJet(other.gameObject.GetComponent<PlatformJumpJet>());
                break;

            case "FallCollider":

                if (!roboltController.GetIsFalling())
                {
                    roboltController.SetIsFalling(true);

                    if ((currentLifes - Const.FALL_DAMAGE) <= 0)
                        roboltController.SetIsFallingToDeath(true);

                    UpdateLife(Const.FALL_DAMAGE, LifeUpdateType.DECRESE, false);

                    roboltController.FallingAnimation();
                }

                break;

            case "DamageOutput":

                bool isShocked = roboltController.GetIsShocked();

                if (!isPlayerInvulnerable && !isShocked && !roboltController.GetIsDead())
                {
                    // leggo la quantità di danno dell'oggetto con cui ho colliso
                    int damage = other.GetComponent<DamageOutputInterface>().GetDamageOutputValue();

                    Vector3 position = other.GetComponent<DamageOutputInterface>().GetNewPlayerPosition();

                    DamageOutputInterface.DamageSource source = other.GetComponent<DamageOutputInterface>().GetDamageSource();

                    switch (source)
                    {
                        case DamageOutputInterface.DamageSource.ENEMY_DAMAGE:

                            // se il player muore
                            if ((currentLifes - damage) <= 0)
                            {
                                // decremento le vite senza redpulse perchè andrà in gameover
                                UpdateLife(damage, LifeUpdateType.DECRESE, false);
                            }
                            else
                            {
                                // decremento le vite con redPulse e animazione Hitted
                                UpdateLife(damage, LifeUpdateType.DECRESE, true);
                                StartCoroutine(HittedShocked());
                                StartCoroutine(Hitted());
                            }
                            break;

                        case DamageOutputInterface.DamageSource.SHOCK_DAMAGE:

                            roboltController.StopAnimantionsAndMovements();
                            roboltController.SetIsShocked(true);
                            UpdateLife(damage, LifeUpdateType.DECRESE, false);
                            roboltController.ShockedAnimation(true);
                            StartCoroutine(Shocked(position, other.GetComponent<DamageOutputInterface>().GetResetPositionSource()));
                            break;

                    }

                    // verifico se l'oggetto deve essere distrutto, ad esempio un proiettile che ha colpito il player
                    bool leaveAlive = other.GetComponent<DamageOutputInterface>().LeaveAlive();

                    // controllo se l'oggetto deve essere distrutto, ad esempio un proiettile
                    if (!leaveAlive)
                        other.GetComponent<DamageOutputInterface>().DestroyMe();
                    //}
                }

                break;

            case "PopUp":

                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<PopUpInterface>().PopUpOn();

                break;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "PlatformJumpJet":

            // quando esco dal trigger della PlatformJumpJet, resetto i parametri originali di salto
            roboltController.SetJumpHeight(RoboltBase.MAX_JUMP_HEIGHT);
            roboltController.SetMaxJumpTime(RoboltBase.MAX_JUMP_TIME);
            roboltController.SetCurrentPlatformJumpJet(null);
            break;
        }
    }

    public void Rumble(float rumbleDuration)
    {
        StartCoroutine(ExecuteRumble(rumbleDuration));
    }

    private IEnumerator ExecuteRumble(float rumbleDuration)
    {
        InputSystem.ResumeHaptics();
        yield return new WaitForSeconds(rumbleDuration);
        InputSystem.PauseHaptics();
    }

    public void DoubleRumble(float rumbleDuration)
    {
        StartCoroutine(ExecuteDoubleRumble(rumbleDuration));
    }

    private IEnumerator ExecuteDoubleRumble(float rumbleDuration)
    {
        InputSystem.ResumeHaptics();
        yield return new WaitForSeconds(rumbleDuration);
        InputSystem.PauseHaptics();
        yield return new WaitForSeconds(rumbleDuration);
        InputSystem.ResumeHaptics();
        yield return new WaitForSeconds(rumbleDuration);
        InputSystem.PauseHaptics();
    }

    // rende invulnerabile il player per il numero di secondi in input
    private IEnumerator SetPlayerInvulnerable(float invulnerableSec) 
    {
        isPlayerInvulnerable = true;
        StartCoroutine(RedPulsePlayer()); 
        yield return new WaitForSeconds(invulnerableSec);
        isPlayerInvulnerable = false;
    }

    IEnumerator RedPulsePlayer() {

        while (isPlayerInvulnerable) {

            foreach (GameObject obj in objectMeshList) 
                obj.GetComponent<SkinnedMeshRenderer>().materials[0].color = new Color(1f, 0.5f, 0.5f, 1.0f);
            
            yield return new WaitForSeconds(Const.PULSE_TIME);

            foreach (GameObject obj in objectMeshList)
                obj.GetComponent<SkinnedMeshRenderer>().materials[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(Const.PULSE_TIME);

        }
    }

    IEnumerator Hitted() 
    {
        roboltController.HittedAnimation(true);
        roboltController.SetIsHitted(true);

        yield return new WaitForSeconds(1f);

        roboltController.HittedAnimation(false);
        roboltController.SetIsHitted(false);
    }

    IEnumerator HittedShocked()
    {
        roboltController.SetIsShocked(true);
        shockSound.Play();
        yield return new WaitForSeconds(0.5f);
        shockSound.Stop();
        roboltController.SetIsShocked(false);
    }


    IEnumerator Shocked(Vector3 position, DamageOutputInterface.ResetPositionSource resetPositionSource)
    {
        Rumble(Const.RUMBLE_SHOCK_DURATION);
        yield return new WaitForSeconds(1.5f);

        if(!roboltController.GetIsDead())
        { 
            StartCoroutine(SetPlayerInvulnerable(Const.INVULNERABLE_SEC));
            roboltController.RestoreAnimationsAndMovements();
            roboltController.SetNewPlayerPosition(position, resetPositionSource);
        }

        roboltController.SetIsShocked(false);
        roboltController.ShockedAnimation(false);
    }

    IEnumerator ReadKeyCoins()
    {
        // mi assicuro che i dati siano stati letti dal json
        yield return new WaitUntil(() => uniqueItemsManager.GetIsKeyCoinsDataLoaded());

        ///////////////// BOLTKEY
        
        int boltKeyCount = 0;
        BoltKeyData[] boltKeyDataArray = uniqueItemsManager.GetBoltKeyData();

        foreach (BoltKeyData boltKeyData in boltKeyDataArray)
        {
            // se boltKeyData è false, allora il boltKey è già stato ottenuto dal player quindi aumento il contatore
            if (!boltKeyData.value)
                boltKeyCount++;
        }

        currentBoltKeys = boltKeyCount;
        UpdateBoltKeysUI();

        ///////////////// BOLTCOIN
        
        int boltCoinCount = 0;
        BoltCoinData[] boltCoinDataArray = uniqueItemsManager.GetBoltCoinData();

        foreach (BoltCoinData boltCoinData in boltCoinDataArray)
        {
            // se boltKCoinData è false, allora il boltCoin è già stato ottenuto dal player quindi aumento il contatore
            if (!boltCoinData.value)
                boltCoinCount++;
        }

        currentBoltCoins = boltCoinCount;
        UpdateBoltCoinsUI();

        isStartUpdateUI = false;
    }

    private void Cheats()
    {

        // VITE
        if (Input.GetKeyDown(KeyCode.P))
        {

            UpdateLife(1, LifeUpdateType.INCREASE, false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {

            UpdateLife(1, LifeUpdateType.DECRESE, false);
        }

        // BOLT KEYS
        if (Input.GetKeyDown(KeyCode.K) && currentBoltKeys < Const.MAX_BOLT_KEYS)
        {

            currentBoltKeys++;
            UpdateBoltKeysUI();
        }

        if (Input.GetKeyDown(KeyCode.L) && currentBoltKeys > 0)
        {

            currentBoltKeys--;
            UpdateBoltKeysUI();
        }

        // BOLT COINS
        if (Input.GetKeyDown(KeyCode.Home) && currentBoltCoins < Const.MAX_BOLT_COINS)
        {

            currentBoltCoins++;
            UpdateBoltCoinsUI();
        }

        if (Input.GetKeyDown(KeyCode.End) && currentBoltCoins > 0)
        {

            currentBoltCoins--;
            UpdateBoltCoinsUI();
        }

        // STEAM TANKS
        if (Input.GetKeyDown(KeyCode.PageUp))
        {

            currentSteamTanks++;
            UpdateSteamTanksUI();
        }

        if (Input.GetKeyDown(KeyCode.PageDown) && currentSteamTanks > 0)
        {

            currentSteamTanks--;
            UpdateSteamTanksUI();
        }


        // RESET POSIZIONE
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.parent.position = new Vector3 (-128.8f, 9.1f, -6.13f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.parent.position = new Vector3(-120.5f, 26.2f, -1.8f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.parent.position = new Vector3(-112.6f, 33.1f, 3.6f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.parent.position = new Vector3(-109.5f, 49.7f, -5.9f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            transform.parent.position = new Vector3(-97.6f, 49.5f, 28.2f);

        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            transform.parent.position = new Vector3(-86.8f, 63.7f, -2.9f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            transform.parent.position = new Vector3(-77.8f, 89.3f, 7.4f);
        }
    }
}