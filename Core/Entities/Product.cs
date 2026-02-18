using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        //Set icin private yaparak sadece constructor ve domain logic ile değiştirilebilecek şekilde tasarlıyoruz.
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public string ImageUrl { get; private set; }

        // Navigation Properties
        public int CategoryID { get; private set; }
        public Category Category { get; private set; }

        
        protected Product() { }

        // Zorunlu alanları Constructor'da istiyoruz.
        // Böylece "İsimsiz" veya "Fiyatsız" ürün oluşması imkansız hale gelir.
        public Product(string name, string description, decimal price, int categoryId)
        {
            // Guard Clauses (Koruyucu Kontroller)
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (price < 0) throw new ArgumentException("Fiyat 0'dan küçük olamaz.");

            Name = name;
            Description = description;
            Price = price;
            CategoryID = categoryId;
            Stock = 0; // Varsayılan stok
        }

        // Domain Logic: Stok düşme işlemi bir mantık içerir.
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Adet pozitif olmalı.");
            if ((Stock - quantity) < 0) throw new InvalidOperationException("Yetersiz stok.");

            Stock -= quantity;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0) throw new ArgumentException("Fiyat negatif olamaz.");
            Price = newPrice;
        }
    }
}