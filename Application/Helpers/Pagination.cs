namespace Application.Helpers
{
    // <T> yapıyoruz çünkü bunu ileride Category veya Order için de kullanabiliriz.
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count; // Toplam veritabanındaki kayıt sayısı (Filtrelenmiş hali)
            Data = data;   // O sayfadaki veriler
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; } // Frontend bunu kullanarak toplam sayfa sayısını hesaplar (Count / PageSize).
        public IReadOnlyList<T> Data { get; set; }
    }
}