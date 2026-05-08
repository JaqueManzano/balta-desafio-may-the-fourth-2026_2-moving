namespace Moving.Core.Models
{
    public class StoredItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StorageBoxId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemDescription { get; set; }
        public string? Keywords { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual StorageBox StorageBox { get; set; } = null!;
    }
}
