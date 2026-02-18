## ECommerceCleanArch

Modern mimari prensiplerini uygulayarak geliştirilen, eğitim ve kişisel gelişim amaçlı bir **E-Ticaret** projesidir. Bu proje gerçek bir ürün ortamında çalışmak zorunda olmayan, ancak gerçek hayattaki senaryolara olabildiğince yakın kurgulanmış bir örnek uygulamadır.

### Projenin Amacı

- **Temel Amaç**:  
  - SOLID prensiplerini anlamak ve pratikte uygulamak  
  - Clean Architecture (Temiz Mimari) yaklaşımını deneyimlemek  
  - Katmanlı ve modüler bir mimariyle, ölçeklenebilir bir e-ticaret altyapısının nasıl kurgulanabileceğini görmek  

- **Hedeflenen Kazanımlar**:  
  - Bağımlılıkların doğru yönetilmesi  
  - Test edilebilir, genişletilebilir ve sürdürülebilir kod yazma alışkanlığı kazanmak  
  - Domain odaklı düşünme becerisini geliştirmek

### Genel Mimari Yaklaşım

Projede **Clean Architecture** ve **SOLID prensipleri** merkezde olacak şekilde bir yapı hedeflenmektedir:

- **Katmanlar (hedeflenen yapı)**:
  - **Domain**: İş kuralları, entity’ler, value object’ler, domain servisleri  
  - **Application**: Use-case’ler, DTO’lar, port/interface tanımları  
  - **Infrastructure**: Veritabanı erişimi, dış servis entegrasyonları, repository implementasyonları  
  - **Presentation**: API katmanı / UI, kullanıcıyla iletişime geçen son katman  

- **SOLID Uygulamaları (hedefler)**:
  - **S**ingle Responsibility: Her sınıf/katmanın tek bir sorumluluk alanı olmasına özen gösterilecek  
  - **O**pen/Closed: Yeni özellik eklerken mevcut kodu mümkün olduğunca değiştirmeden sistemi genişletebilmek  
  - **L**iskov Substitution: Arayüz/sözleşmelere uyan türev sınıfların sorunsuz şekilde yer değiştirebilmesi  
  - **I**nterface Segregation: İhtiyaçtan fazla metot içeren “God Interface” yapılarından kaçınmak  
  - **D**ependency Inversion: Üst seviye modüllerin alt seviye detaylara değil, soyutlamalara bağımlı olması  

### Projenin Mevcut Durumu

Bu proje **aktif geliştirme aşamasındadır** ve zamanla büyüyecek/geliştirilecektir. Şu an itibariyle:

- Proje yapısı ve temel mimari kurgusu üzerinde çalışılmaktadır.  
- Domain ve Application katmanları için temel modeller, arayüzler ve akışlar planlanmaktadır.  
- Repository ve servis yapıları için taslak mimari kararlar alınmaktadır.  
- Henüz son kullanıcıya yönelik tam fonksiyonel bir e-ticaret deneyimi sunma aşamasında değildir.

Bu depo daha çok **öğrenme notları**, **deneysel denemeler** ve **mimari denemeler** içerecektir.

### Planlanan Özellikler

Geliştirme ilerledikçe aşağıdaki alanlarda iyileştirme ve eklemeler hedeflenmektedir:

- **Ürün Yönetimi**  
  - Ürün listeleme, filtreleme, detay sayfaları  
  - Kategori/alt kategori yapısı  

- **Sepet & Sipariş Süreçleri**  
  - Sepete ürün ekleme/çıkarma/güncelleme  
  - Sipariş oluşturma akışları  

- **Kullanıcı Yönetimi**  
  - Kayıt/oturum açma (authentication & authorization)  
  - Kullanıcı rolleri (müşteri, admin vb.)  

- **Altyapı & Teknik Geliştirmeler**  
  - Veritabanı entegrasyonu  
  - Unit test / integration test senaryoları  
  - Logging, exception handling, validation katmanı  

### Katkı ve Kullanım Notları

- Bu proje öncelikli olarak **kişisel gelişim** ve **eğitim** amaçlıdır.  
- Kod yapısı zamanla değişebilir; mimari kararlar, öğrenme sürecine göre revize edilebilir.  
- İleride dokümantasyon genişletilerek, katmanlar ve kullanılan tasarım desenleri için daha detaylı açıklamalar eklenecektir.

### Son Söz

Bu proje, “önce doğru mimari ve prensipler” yaklaşımıyla, **temiz, anlaşılır ve sürdürülebilir** bir e-ticaret altyapısının nasıl kurulabileceğini keşfetmek için bir deneme alanıdır.  

Proje geliştikçe bu `README.md` dosyası da güncellenecek ve eklenen mimari kararlar, desenler ve önemli değişiklikler burada detaylandırılacaktır.

