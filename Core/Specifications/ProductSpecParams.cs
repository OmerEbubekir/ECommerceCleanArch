namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50; // Kullanıcı en fazla 50 ürün isteyebilir.

        // Varsayılan: 1. sayfa
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6; // Varsayılan: 6 ürün

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // İleride buraya Search, Sort, CategoryId gibi filtreler de gelecek.
    }
}