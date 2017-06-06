public enum Cible
{
    NimporteQui,
    ToutLesJoueurs,
    APorteeDeTire,
    APorteeFixe,
    SoiMeme
}

public enum Couleur
{
    Pique,
    Carreau,
    Coeur,
    Trefle
}

public enum Value
{
    As,
    Deux,
    Trois,
    Quatre,
    Cinq,
    Six,
    Sept,
    Huit,
    Neuf,
    Dix,
    Valet,
    Dame,
    Roi    
}

public enum Role
{
    Sherif,
    Adjudant,
    HorsLaLoi,
    Renegat,
    None
}

public enum CardType
{
    Bang,
    Discard,
    Draw,
    Duel,
    Heal,
    Miss,
    Jail,
    ModRange,
    Stash,
    Weapon
}

public enum Command
{
    StringToDraw,
    NbPlayer,
    GetCards,
    Quit,
    EndTurn,
    PlayCard,
    PlayersInfo,
    PlayerInfo
}