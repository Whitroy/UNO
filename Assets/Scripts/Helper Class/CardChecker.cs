using UNO;
public static class CardChecker
{
    public static bool CheckValidCard(Card top, Card card,IPlayer player)
    {
        if (MatchManager.Instance.AbilityEffects)
        {
            return CheckValidAfterEffect(top, card,player);
        }
        else
        {
            return CheckVaidWithoutEffect(top, card,player);
        }

    }

    private static bool CheckValidAfterEffect(Card top, Card card,IPlayer player)
    {
        if (player.StackOfCard.Count < 1)
        {
            //numbers and action cards
            if (top.Color == card.Color)
            {
                return true;
            }
            //wild cards
            if (card.Group == CardGroup.Wild)
                return true;
        }
        else
        {
            Card stackTop = player.StackOfCard.Peek();
            return CheckVaidWithoutEffect(stackTop, card, player,true);
        }


        return false;
    }

    private static bool CheckVaidWithoutEffect(Card top, Card card,IPlayer player, bool afterAbility = false)
    {
        Card topCard;

        if (!afterAbility)
            topCard = MatchManager.Instance.GetTopCard();
        else
            topCard = top;

        if (top.Group == CardGroup.Number)
        {
            /* Valid options
             * Card.Type -> Same Number
             * Card.Color -> one different card with same color
             * Card.Group -> wild,action,number
             */
            if ((card.Type == topCard.Type || player.StackOfCard.Count == 0) && (top.Type == card.Type || (
                (top.Color == card.Color || card.Group == CardGroup.Wild) && player.StackOfCard.Count == 0)
                ))
                return true;
        }
        else if (top.Group == CardGroup.Action)
        {
            /* Valid options
             * Card.Group :- action card of same type
             * if top is Draw Two -> Draw four
             * if top is Skip -> Draw four and Draw two
             */

            if (top.Type == card.Type || (top.Type == CardType.DrawTwo && card.Type == CardType.DrawFour)
                || (top.Type == CardType.Skip && ((card.Type == CardType.DrawTwo && top.Color == card.Color)
                || card.Type == CardType.DrawFour)))
                return true;
        }
        else
        {
            /* Valid options
             * Card.Type :-  Same
             */
            if (top.Type == card.Type)
                return true;
        }
        return false;
    }
}
