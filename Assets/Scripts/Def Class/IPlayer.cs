namespace UNO
{
    public interface IPlayer:IPlayerUtils
    {
        void ShowCard();

        void PickCard();

        void Shout();

        void Caught();

        void ThrowCard();

        void ChooseCard();
    }

}