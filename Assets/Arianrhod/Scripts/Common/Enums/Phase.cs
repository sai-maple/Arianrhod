namespace Arianrhod
{
    public enum Phase
    {
        Standby, // カメラ移動,行動キャラ指定UI表示
        Move, //キャラ移動、スキップ
        Attack,　//攻撃選択、スキップ
        Dice, //サイコロ投擲
        Calculation, //サイコロ和計算
        Damage,　//攻撃アニメーション　ダメージステップ
        End,　//　勝敗判定、自行動キャラ取得
    }
}