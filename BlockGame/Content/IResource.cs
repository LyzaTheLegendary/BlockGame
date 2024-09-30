namespace BlockGame.Content
{
    public interface IResource
    {
        public byte[] Fetch(string name);
        public bool Has(string name);
        public void Add(string name, byte[] data);
        public void Remove(string name);
    }
}
