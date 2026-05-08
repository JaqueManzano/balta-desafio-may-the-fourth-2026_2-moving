namespace Moving.Core.Models
{
    public class StorageBox
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Description { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<StoredItem> Items { get; set; } = [];
    }
}
