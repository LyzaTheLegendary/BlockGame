namespace ExodiumEngine.Content
{
    public interface IResource
    {
        public Stream GetReadStreamFrom(string filename);
        public byte[] Fetch(string filename);
        public bool Has(string filename);
        public void Add(string filename, byte[] data);
        public void Remove(string name);
    }
}
