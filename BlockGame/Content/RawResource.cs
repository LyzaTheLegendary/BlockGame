namespace BlockGame.Content
{
    internal class RawResource : IResource
    {
        const string DIR = "Resource";
        public void Add(string name, byte[] data)
            => File.WriteAllBytes(Path.Combine(DIR,name), data);
        public byte[] Fetch(string name)
            => File.ReadAllBytes(Path.Combine(DIR, name));
        public bool Has(string name)
            => File.Exists(Path.Combine(DIR, name));
        public void Remove(string name)
            => File.Delete(Path.Combine(DIR, name));
    }
}
