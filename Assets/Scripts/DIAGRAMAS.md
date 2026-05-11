 classDiagram

class Entity {

<<Abstract>>

+int health

+float speed

+TakeDamage(int amount)

+Die()* }

class Player {

+Inventory skills

+Dash()

+Attack()

}

class Enemie {

+IA_State currentState

+DetectionRange

+AttackPlayer()

}

Entity <|-- Player

Entity <|-- Enemie



stateDiagram-v2

[*] --> Idle

Idle --> Persecucion: Jugador detectado

Persecucion --> Ataque: Distancia < Umbral

Ataque --> Persecucion: Jugador se aleja

Persecucion --> Idle: Jugador fuera de rango

Ataque --> Muerte: Vida == 0

Persecucion --> Muerte: Vida == 0

Muerte --> [*]



graph TD

A[Inicio Run] --> B{¿Partida Nueva?}

B -- Sí --> C[CharacterSelection: Elegir Hurtadilla]

B -- No --> D[GameSelector: Cargar PlayerPrefs]

D --> E[GameManager: Instanciar Prefab + Stats]

E --> F[Gameplay]

F --> G[Muerte]

G --> H[Actualizar PlayerPrefs: Desbloqueos]

H --> A

C --> I[Inicia unos PlayerPrefs]

I --> F



stateDiagram-v2

[*] --> MenuScene


state MenuScene {

MainMenu --> GameSelectorUI : Jugar

MainMenu --> ControlsConfigUI : Ajustes

ControlsConfigUI --> ControlsConfigUI : Guardar (KeyBindings)

}



GameSelectorUI --> CharacterScreenUI : Nueva Partida

GameSelectorUI --> BaseScene : Cargar Partida

CharacterScreenUI --> BaseScene : Confirmar Personaje


state InGame {

BaseScene --> Mapa1_MP1 : Explorar

Mapa1_MP1 --> Mapa1_MP2 : Teleport

}



InGame --> DeadScreen : Vida == 0

DeadScreen --> MenuScene : Reiniciar Run



sequenceDiagram

participant Jugador as Player

participant TP as Teleport (Trigger)

participant Gestor as SceneGestor

participant Unity as SceneManager



Jugador->>TP: Entra en contacto (OnTriggerEnter2D)

TP->>Gestor: SolicitarCambio(id_destino)

Gestor->>Gestor: Comprueba array MapScene (¿Sala visitada?)

Gestor->>Unity: LoadSceneAsync(id_destino)

Unity-->>Gestor: Escena cargada

Unity->>Gestor: SetLastScene() - Guarda enemigos muertos y TP usado

alt Sala ya visitada

Gestor->>Unity: Destruye/Desactiva enemigos del mapa

else Sala nueva

Gestor->>Unity: Mantiene Spawners activos

end

Gestor->>Jugador: Reposiciona en las coordenadas de entrada



graph TD

subgraph Core ["Capa Lógica y Estado (Core)"]

GM[GameManager]

SG[SceneGestor]

PE[PersistEventSys - Singleton]

KB[KeyBindings]

end



subgraph Interfaz ["Capa de Interfaz (UI)"]

MM[MainMenu]

HUD[HUDController]

CS[CharacterSelection]

end



subgraph Entidades ["Capa de Entidades (Entities)"]

ENT[Entity]

PLY[Player]

ENM[Enemie]

PRJ[Projectile]

end



%% Relaciones

MM -.->|Inicia/Carga datos| GM

HUD -.->|Lee Vida/Stats| PLY

GM -->|Controla el flujo de| SG

SG -->|Instancia| PLY

SG -->|Instancia| ENM

ENT -->|Instancia| PRJ


style Core fill:#f9f,stroke:#333,stroke-width:2px

style Interfaz fill:#bbf,stroke:#333,stroke-width:2px

style Entidades fill:#dfd,stroke:#333,stroke-width:2px