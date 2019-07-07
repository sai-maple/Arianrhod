namespace Arianrhod
{
    public enum GamePhase
    {
        OutGame,
        StageInitialize,
        
        Standby, // カメラ移動,行動キャラ指定UI表示
        Move, //キャラ移動、スキップ
        Attack,　//攻撃選択、スキップ
        Dice, //サイコロ投擲
        Damage,　//攻撃アニメーション　ダメージステップ
        End,　//　勝敗判定、自行動キャラ取得
        StageClear,
        GameOver,
    }
}