# DIAGRAMAS PRIMIGENIA
# Todos los diagramas usan sintaxis Mermaid.
# Cada sección indica dónde va en la documentación.

# ──────────────────────────────────────────────
# 1. DIAGRAMA DE CLASES — ENTIDADES
# Sección: 4.4 Estructura del código
# ──────────────────────────────────────────────

classDiagram
    class Entity {
        <<Abstract>>
        +float MaxHealth
        +float CurrentHealth
        #float moveSpeed
        #float invulnerabilityTime
        #float damage
        +bool isInvencible
        #bool isDead
        +TakeDamage(float amount)
        +TakeDamage(float amount, string afinidad)
        +Heal(float amount)
        #OnDeath()*
    }

    class Player {
        <<Abstract>>
        +static Player Instance
        +string Armazon
        +string Afinidad
        +bool Armazon1, Armazon2, Armazon3
        +bool Afinidad0, Afinidad1, Afinidad2, Afinidad3
        #bool IsMelee
        #bool IsRanged
        +bool attacking
        #HandleMovement()
        #ExecAttack()
        #ExecAbility()*
        #MeleeAttack()
        #RangedAttack()
        +ActivateDesign()
        #OnDeath()
    }

    class Hurtadilla {
        -float abilitySpeed
        #ExecAbility()
    }

    class Enemie {
        <<Abstract>>
        #EnemyState state
        #float rangeOfPatrol
        #float rangeOfDetection
        #float patrolSpeed
        #float chaseSpeed
        #float knockback
        #HandleMovement()
        #Patrol()
        #Chase()
        #Attack(Player target)
        +TakeDamage(float amount, string afinidad)
        #OnDeath()
    }

    class GroundEnemy {
    }

    class RangedEnemy {
        -float rangeOfAttack
        -float attackCooldown
        -GameObject projectilePrefab
        #HandleMovement()
        #Attack(Player target)
        -ShootProjectile(Player target)
    }

    class Projectile {
        -Vector2 direction
        -float speed
        -float damage
        -string ownerTag
        +Setup(Vector2 dir, float spd, float dmg, GameObject owner)
        -OnCollisionEnter2D(Collision2D collision)
    }

    Entity <|-- Player
    Entity <|-- Enemie
    Player <|-- Hurtadilla
    Enemie <|-- GroundEnemy
    Enemie <|-- RangedEnemy
    Player ..> Projectile : instancia
    RangedEnemy ..> Projectile : instancia


# ──────────────────────────────────────────────
# 2. DIAGRAMA DE ESTADOS — FSM DE ENEMIGOS
# Sección: 4.7 IA de los enemigos
# ──────────────────────────────────────────────

stateDiagram-v2
    [*] --> Patrulla

    Patrulla --> Persecucion : distancia <= rangeOfDetection
    Persecucion --> Patrulla : distancia > rangeOfDetection
    Persecucion --> Ataque : distancia <= rangeOfAttack (RangedEnemy) / contacto (GroundEnemy)
    Ataque --> Persecucion : distancia > rangeOfAttack

    Patrulla --> Aturdido : TakeDamage con Tumba eterna
    Persecucion --> Aturdido : TakeDamage con Tumba eterna
    Ataque --> Aturdido : TakeDamage con Tumba eterna
    Aturdido --> Patrulla : Fin del stun (1s)

    Patrulla --> Muerte : CurrentHealth <= 0
    Persecucion --> Muerte : CurrentHealth <= 0
    Ataque --> Muerte : CurrentHealth <= 0
    Aturdido --> Muerte : CurrentHealth <= 0

    Muerte --> [*]


# ──────────────────────────────────────────────
# 3. DIAGRAMA DE FLUJO — GAME LOOP
# Sección: 4.6 Flujo de juego (Game Loop)
# ──────────────────────────────────────────────

graph TD
    A[Menu Principal] --> B{Partida existente?}

    B -- No --> C[CharacterSelection: Elegir personaje]
    C --> D["StartGameWithCharacter(): Guarda PlayerPrefs iniciales"]
    D --> E[Base]

    B -- Si --> F["CreateOnPlayerPrefs(): Carga armazones y afinidades"]
    F --> E

    E --> G["MagicBook: Elegir armazon y afinidad"]
    G --> H[Entra a la mazmorra]

    H --> I[Explorar sala]
    I --> J{Enemigos eliminados?}
    J -- No --> I
    J -- Si --> K{Tipo de TP?}

    K -- "Sala normal" --> L[SceneGestor: Carga sala aleatoria]
    L --> I
    K -- Boss --> M[Sala del Boss]

    M --> N{Boss derrotado?}
    N -- Si --> O[VictoryScreen]
    O --> P[Volver a Base]
    P --> E

    I --> Q{Jugador muere?}
    Q -- Si --> R[DeadScreen]
    R --> S{Opcion?}
    S -- "Volver a Base" --> P
    S -- "Menu Principal" --> A


# ──────────────────────────────────────────────
# 4. DIAGRAMA DE ESTADOS — NAVEGACION DE UI
# Sección: 4.3 Implementación de la interfaz de usuario
# ──────────────────────────────────────────────

stateDiagram-v2
    [*] --> MenuScene

    state MenuScene {
        MainMenu --> GameSelectorUI : Jugar
        MainMenu --> SettingsConfigUI : Ajustes
        SettingsConfigUI --> ControlsConfigUI : Ver controles
        ControlsConfigUI --> SettingsConfigUI : Volver
    }

    GameSelectorUI --> CharacterSelectionUI : Nueva Partida
    GameSelectorUI --> BaseScene : Cargar Partida existente
    CharacterSelectionUI --> GameSelectorUI : Volver
    CharacterSelectionUI --> BaseScene : Confirmar Personaje

    state InGame {
        BaseScene --> MagicBook : Trigger con Libro
        MagicBook --> BaseScene : Cerrar libro
        BaseScene --> Mazmorra : Explorar (TP)
        Mazmorra --> Mazmorra : Cambiar sala (TP)
        Mazmorra --> BossSala : TP al Boss
    }

    state Pausa {
        SettingsPausa --> ControlsPausa : Ver controles
        ControlsPausa --> SettingsPausa : Volver
        SettingsPausa --> BaseScene : Volver a Base
    }

    InGame --> Pausa : Tecla Pausa
    Pausa --> InGame : Reanudar

    Mazmorra --> DeadScreen : Vida <= 0
    BossSala --> VictoryScreen : Boss derrotado
    BossSala --> DeadScreen : Vida <= 0
    DeadScreen --> BaseScene : Volver a Base
    DeadScreen --> MenuScene : Menu Principal
    VictoryScreen --> BaseScene : Volver a Base


# ──────────────────────────────────────────────
# 5. DIAGRAMA DE SECUENCIA — TELEPORT ENTRE SALAS
# Sección: 4.5 Persistencia de datos / 4.1 Arquitectura
# ──────────────────────────────────────────────

sequenceDiagram
    participant J as Player
    participant TP as Teleport
    participant GM as GameManager
    participant SG as SceneGestor
    participant U as SceneManager (Unity)
    participant SP as EnemieSpawner

    J->>TP: OnTriggerEnter2D
    TP->>GM: ChangeScene(sceneName, targetRoomId, index)
    GM->>SG: ChangeScene(sceneName, roomId, index)

    SG->>SG: Busca roomId en mpVisited
    alt Sala ya visitada
        SG->>SG: SavedMap = existente
    else Sala nueva / aleatoria
        SG->>SG: SavedMap = new MapScene(sceneName)
    end

    SG->>U: SceneManager.LoadScene(sceneName)
    U-->>GM: OnSceneLoaded(scene)
    GM->>GM: Restaurar TPs desde mpVisited
    GM->>SG: SetLastScene(doorIndex)
    SG->>SG: Guarda conexiones bidireccionales en MapScene

    alt Sala NO visitada (isVisited == false)
        GM->>SP: SpawnEnemies()
        GM->>SG: SavedMap.isVisited = true
    else Sala ya visitada
        GM->>GM: No spawna enemigos
    end

    GM->>J: SpawnPlayerAtTP() - Reposiciona


# ──────────────────────────────────────────────
# 6. DIAGRAMA DE ARQUITECTURA POR CAPAS
# Sección: 4.4 Estructura del código (vision general)
# ──────────────────────────────────────────────

graph TD
    subgraph Core ["Capa Logica y Estado (Core)"]
        GM[GameManager - Singleton]
        SG[SceneGestor - estatico]
        MS[MapScene]
        PE[PersistEventSys - Singleton]
        KB[KeyBindings - Singleton]
        IH[InputHandler - Singleton]
        GC[GameConstants]
        VM[VolumeManager - Singleton]
        SP[SoundPlayer]
        ES[EnemieSpawner]
        SC[SeguimientoCamara]
        TP[Teleport]
    end

    subgraph Interfaz ["Capa de Interfaz (UI)"]
        MM[MainMenu]
        HUD[HUDController]
        CS[CharacterSelection]
        GS[GameSelector]
        MB[MagicBook]
        DS[DeadScreen]
        VS[VictoryScreen]
        CC[ControlsConfigUI]
        SS[SettingsConfigUI]
        VU[VolumeSliderUI]
    end

    subgraph Entidades ["Capa de Entidades (Entities)"]
        ENT[Entity - abstracta]
        PLY[Player - abstracta]
        HUR[Hurtadilla]
        ENM[Enemie - abstracta]
        GE[GroundEnemy]
        RE[RangedEnemy]
        PRJ[Projectile]
    end

    %% Relaciones Core
    GM -->|delega cambio escena| SG
    SG -->|gestiona| MS
    SG -->|usa| TP
    GM -->|instancia prefabs| PLY
    GM -->|coordina spawn| ES

    %% Relaciones UI - Core
    MM -.->|inicia juego| GM
    GS -.->|selecciona partida| GM
    CS -.->|elige personaje| GM
    CC -.->|guarda controles| KB
    VU -.->|ajusta volumen| VM
    MB -.->|cambia armazon/afinidad| PLY

    %% Relaciones UI - Entities
    HUD -.->|lee vida y cooldown| PLY
    DS -.->|volver a base| GM

    %% Relaciones Entities
    ENT --> PLY
    ENT --> ENM
    PLY --> HUR
    ENM --> GE
    ENM --> RE
    PLY ..->|dispara| PRJ
    RE ..->|dispara| PRJ

    %% Input
    IH -.->|lee teclas| KB
    PLY -.->|consulta input| IH

    %% Camara
    SC -.->|sigue a| PLY

    linkStyle default stroke:#b0b0b0,stroke-width:2px;

    style Core fill:#2d2d3d,stroke:#7c5cbf,stroke-width:2px,color:#e0e0e0
    style Interfaz fill:#1e3a5f,stroke:#4a9eff,stroke-width:2px,color:#e0e0e0
    style Entidades fill:#1e3d1e,stroke:#4caf50,stroke-width:2px,color:#e0e0e0


# ──────────────────────────────────────────────
# 7. DIAGRAMA DE PERSISTENCIA DE DATOS (NUEVO)
# Sección: 4.5 Persistencia de datos
# ──────────────────────────────────────────────

graph TD
    subgraph Volatil ["Persistencia volatil (durante la run)"]
        PES["PersistEventSys - DontDestroyOnLoad"]
        PI["Player Instance - Singleton + DontDestroyOnLoad"]
        SGD["SceneGestor.mpVisited - Lista estatica de MapScene"]
        SMD["SceneGestor.SavedMap - Sala actual"]
    end

    subgraph Permanente ["Persistencia permanente (PlayerPrefs)"]
        PP1["Partidas: Game1, Game2 - int 0/1"]
        PP2["Personaje: SelectedCharacter - string"]
        PP3["Armazones: Armazon1_G1..3 - int 0/1"]
        PP4["Afinidades: Afinidad0_G1..3 - int 0/1"]
        PP5["Controles: KeyBindings - int KeyCode"]
        PP6["Volumenes: MusicVolume, SFXVolume - float"]
    end

    PI --->|"Guarda al morir<br>o al recoger item"| PP3
    PI --->|"Guarda al morir<br>o al recoger item"| PP4
    SGD --->|"Se borra al morir/ganar"| SGD

    PP1 --->|"Al cargar partida"| PI
    PP2 --->|"Al cargar partida"| PI
    PP3 --->|"CreateOnPlayerPrefs()"| PI
    PP5 --->|"LoadBindings()"| PES

    linkStyle default stroke:#b0b0b0,stroke-width:2px;

    style Volatil fill:#3d2d1e,stroke:#ff9800,stroke-width:2px,color:#e0e0e0
    style Permanente fill:#1e3d3d,stroke:#00bcd4,stroke-width:2px,color:#e0e0e0


# ──────────────────────────────────────────────
# 8. DIAGRAMA DE SECUENCIA — COMBATE (NUEVO)
# Sección: 4.2 Implementación del sistema de combate
# ──────────────────────────────────────────────

sequenceDiagram
    participant IH as InputHandler
    participant P as Player
    participant SP as SoundPlayer
    participant E as Enemie
    participant PRJ as Projectile

    IH->>P: AttackPressed == true
    P->>SP: PlayAttack()
    P->>P: ExecAttack()

    alt Armazon = Verdugo de Titanes (Melee)
        P->>P: MeleeAttack() - isInvencible = true
        P->>E: OnCollisionEnter2D - TakeDamage(damage, Afinidad)
    else Armazon = Garras de Umbra (Ranged)
        P->>PRJ: FireSingle(lastFacingDirection)
        PRJ->>E: OnCollisionEnter2D - TakeDamage(damage, Afinidad)
    else Armazon = El Alambique (Placed)
        P->>PRJ: FirePlaced() - speed = 0
        PRJ->>E: OnCollisionEnter2D - TakeDamage(damage, Afinidad)
    end

    E->>E: base.TakeDamage(amount) - CurrentHealth -= amount
    E->>E: Knockback (AddForce)

    alt Afinidad = Pacto Carmesi
        E->>P: Player.Instance.CurrentHealth += 2
        E->>P: ShowHealFx()
    else Afinidad = Fiebre ceniza
        E->>E: Corrutina Burned(5s) - danio continuo
    else Afinidad = Tumba eterna
        E->>E: Corrutina Stuned(1s) - Stun = true
    end

    alt CurrentHealth <= 0
        E->>E: OnDeath() - Destroy(gameObject)
        opt Escena == Boss
            E->>GM: VictoryScreenCanvas.SetActive(true)
        end
    end


# ──────────────────────────────────────────────
# 9. DIAGRAMA DE SECUENCIA — CARGA DE PARTIDA (NUEVO)
# Sección: 4.5 Persistencia de datos
# ──────────────────────────────────────────────

sequenceDiagram
    participant U as Jugador (usuario)
    participant GS as GameSelector
    participant GM as GameManager
    participant PP as PlayerPrefs
    participant P as Player

    U->>GS: Selecciona "Partida 1"
    GS->>GS: gameSelected = 1
    GS->>PP: GetInt("Game1")

    alt Partida existe (== 1)
        GS->>GM: CreateOnPlayerPrefs(1)
        GM->>PP: GetString("SelectedCharacter1")
        GM->>GM: Instanciar prefab del personaje
        GM->>PP: GetInt("Armazon1_G1") ... GetInt("Afinidad3_G1")
        GM->>P: Asigna Armazon1..3, Afinidad0..3
        GM->>GM: ChangeScene(baseScene)
    else Partida nueva (== 0)
        GS->>GS: Mostrar CharacterSelection
        U->>CS: Selecciona personaje
        CS->>GM: StartGameWithCharacter(character, 1)
        GM->>PP: SetString("SelectedCharacter1", character)
        GM->>PP: SetInt("Armazon1_G1", 1) - desbloqueo inicial
        GM->>PP: SetInt("Game1", 1)
        GM->>GM: ChangeScene(baseScene)
    end


# ──────────────────────────────────────────────
# 10. DIAGRAMA DE CASOS DE USO UML (NUEVO)
# Sección: 1.4 Diagrama de casos de uso
# ──────────────────────────────────────────────

graph LR
    J((Jugador))

    J --> CU1[Crear partida nueva]
    J --> CU2[Cargar partida existente]
    J --> CU3[Eliminar partida]
    J --> CU4[Seleccionar personaje]
    J --> CU5[Moverse por el mapa]
    J --> CU6[Atacar con armazon]
    J --> CU7[Usar habilidad especial]
    J --> CU8[Cambiar armazon/afinidad en el Libro Magico]
    J --> CU9[Pausar la partida]
    J --> CU10[Configurar controles]
    J --> CU11[Ajustar volumen]
    J --> CU12[Derrotar al Boss]
    J --> CU13[Desbloquear armazones/afinidades]

    CU1 -->|include| CU4
    CU6 -->|extend| CU14[Aplicar efecto de afinidad]
    CU12 -->|include| CU15[Pantalla de victoria]
    CU5 -->|extend| CU16[Morir - Pantalla de muerte]