namespace Game
{
    public interface IMessageUI
    {
        void ShowMessage(string message);

        void SetScore(int score);

        void PlayBanter(Banter banter);

        void StopBanter();
    }
}
