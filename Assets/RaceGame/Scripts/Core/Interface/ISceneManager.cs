namespace RaceGame.Core.Interface
{
    public interface ISceneManager
    {
        public bool StartFromTitle { get; }
        public void ToRace();
        public void ToTitle();
    }
}