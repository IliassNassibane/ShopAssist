namespace ShopAssist_SQLite.Models
{
    public class AppUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserHash { get; set; }
        public string ImgPath { get; set; }

        public override string ToString() => $"{UserName}({ID})({UserHash})";
    }
}
