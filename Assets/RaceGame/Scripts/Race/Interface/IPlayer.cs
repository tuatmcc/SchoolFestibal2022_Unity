namespace RaceGame.Race.Interface
{
    public interface IPlayer
    {
        public bool IsLocalPlayer { get; }
        public float Speed { get; }
        public void CmdAccelerate();
    }
}