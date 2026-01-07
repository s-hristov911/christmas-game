using System;

namespace SantaGiftListManager.Models
{
    internal enum GiftDisposition
    {
        Nice,
        Naughty
    }

    internal class GiftItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string GiftName { get; set; } = string.Empty;
        public string ChildName { get; set; } = string.Empty;
        public GiftDisposition Disposition { get; set; } = GiftDisposition.Nice;

        public GiftItem Clone() => new()
        {
            Id = Id,
            GiftName = GiftName,
            ChildName = ChildName,
            Disposition = Disposition
        };
    }
}
