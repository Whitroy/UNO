namespace UNO
{
    public enum CardGroup
    {
        Action, //24
        Number, //76
        Wild   //8
    }

    public enum CardColor
    {
        Red, //19 0 and two set of 1 to 9
        Blue, //19
        Yellow, //19
        Green, //19
        None
    }

    public enum CardType
    {
        //NumberCards
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        //Action Card Types
        DrawTwo,
        Reverse,
        Skip,
        //Wild Card Types
        DrawFour,
        WildCard,
        
    }

}