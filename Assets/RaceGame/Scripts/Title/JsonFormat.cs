namespace json
{
    [System.Serializable]
    public struct Jsondata
    {
        public string status;
        public Imagedata[] data;

        [System.Serializable]
        public struct Imagedata
        {
            public int id;
            public string URL;
        }
    }
}