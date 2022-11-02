namespace RaceGame.Title
{
    [System.Serializable]
    public struct JsonData
    {
        public string status;
        public ImageData[] data;

        [System.Serializable]
        public struct ImageData
        {
            public int id;
            public string URL;
        }
    }
}