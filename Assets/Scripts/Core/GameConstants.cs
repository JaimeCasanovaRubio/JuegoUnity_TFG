using UnityEngine;

/// <summary>
/// Constantes globales del juego. Equivalente a Cons.java de LibGDX.
/// </summary>
public static class GameConstants
{
    // SCALE - En Unity normalmente no necesitamos escala manual,
    // pero lo mantenemos por compatibilidad con la lógica original
    public const float SCALE = 2.0f;

    // TILED
    public const int TILE_DEFAULT_SIZE = 32;
    public const int TILE_SIZE = (int)(TILE_DEFAULT_SIZE * SCALE);

    // PLAYER
    public const float PLAYER_SPEED = 5f;
    public const int PLAYER_WIDTH = 32;
    public const int PLAYER_HEIGHT = 32;

    // MONSTERS
    public const float MONSTER_SPEED = 2f;

    // GAME WINDOW
    public const int GAME_WIDTH = 1280;
    public const int GAME_HEIGHT = 720;

    // ANIMATION STATES (para referencia con el Animator)
    public static class AnimationStates
    {
        public const string IDLE = "Idle";
        public const string RUN = "Run";
        public const string JUMP = "Jump";
        public const string FALL = "Fall";
        public const string HIT = "Hit";
        public const string ATTACK = "Attack";
        public const string ABILITY = "Ability";
    }

    // PLAYER PREFS KEYS (para guardar configuración)
    public static class PlayerPrefsKeys
    {
        public const string MUSIC_VOLUME = "MusicVolume";
        public const string SFX_VOLUME = "SFXVolume";
        public const string FULLSCREEN = "Fullscreen";
    }
}
