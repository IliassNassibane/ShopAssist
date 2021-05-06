namespace ShopAssist_SQLite.Models
{
    public class ShopListMember
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Icon { get; set; }

        public override string ToString() => $"{MemberID}: {Name}";
    }
}
