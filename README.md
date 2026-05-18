# OtoPazar Araç ve Parça Alım Satım Sistemi 🚗

Bu proje, C# Windows Forms ve LINQ kullanılarak geliştirilmiş, alıcıların ve satıcıların bir araya gelerek ikinci el/sıfır araç veya araç parçası ticareti yapabildikleri modern bir masaüstü otomasyon uygulamasıdır. Projenin temel amacı, kullanıcı dostu ve akıcı bir arayüz ile e-ticaret süreçlerini (listeleme, mesajlaşma, filtreleme) simüle etmektir. 

Veritabanı işlemleri, **Entity Framework** mimarisi örnek alınarak sanal bellek (in-memory) üzerinde çalışan listelerle gerçekleştirilmiş olup, projeyi anında çalıştırılabilir hale getirmektedir. Herhangi bir ekstra SQL veya veritabanı kurulumuna gerek yoktur.

## 🚀 Özellikler

### 1. Dinamik Kullanıcı Girişi (Alıcı / Satıcı)
- Kullanıcılar sisteme kayıt olurken **Alıcı** veya **Satıcı** rolünü seçer.
- Giriş yapıldığında, sistem kullanıcı rolüne göre iki farklı ana panel sunar:
  - **Alıcı Paneli:** Tüm güncel ilanların sergilendiği ana vitrini (Dashboard) görürler.
  - **Satıcı Paneli:** Sadece kendi ekledikleri ilanları görüntüleyebilecekleri, silecekleri ve yeni ürün ekleyebilecekleri özel panele yönlendirilirler.

### 2. İlan ve Ürün Yönetimi (Satıcılara Özel)
- Satıcılar saniyeler içerisinde yepyeni ilanlar ekleyebilir.
- İlan detaylarında otomotiv sektörüne özel tüm kritik bilgiler eksiksiz girilebilir:
  - Fiyat
  - Durum (Sıfır / İkinci El)
  - Kilometre
  - Model Yılı
  - Şase No / Parça Kodu
- Satıcılar doğrudan kişisel bilgisayarlarından ürüne ait **görsel (.jpg, .png)** yükleyebilirler. Seçilen görsel, vitrindeki ilan kartlarında anında sergilenir.

### 3. Gelişmiş Filtreleme (Alıcılara Özel)
- Alıcılar ana sayfada yer alan dinamik arama çubuğunu kullanarak anlık (canlı) ilan başlığı filtrelemesi yapabilir. 
- Klavyeden basılan her tuşta ilan vitrini (FlowLayoutPanel) saniyesinde güncellenir.

### 4. Mesajlaşma Sistemi
- Uygulama içerisinde alıcı ve satıcı arasındaki iletişimi sağlayan güçlü, yerleşik bir mesajlaşma altyapısı bulunur.
- Alıcılar, ilgilendikleri bir ürünün detaylarını incelerken alt kısımdaki mesaj formunu doldurarak satıcıya anında mesaj iletebilir.
- **Hızlı Yanıt:** Satıcılar ve Alıcılar, kendi panellerinin hemen sağında sabitlenmiş "Gelen Mesajlar" kutusundan anlık takipleşebilir. Gelen mesaja tek tıkla tıklanarak açılan ufak ekrandan **hızlı yanıt** yazılıp gönderilebilir.

## 🛠️ Teknolojiler
- **C# / .NET (Windows Forms)**
- **LINQ (Language Integrated Query):** Ürün filtreleme, mesaj sorgulama ve kullanıcı doğrulama işlemleri için.
- **In-Memory Mock Database:** `List<T>` yapısı kullanılarak tam bir Entity Framework DbSet davranışı taklit edilmiştir.

## 💻 Kurulum ve Çalıştırma
1. Projeyi bilgisayarınıza klonlayın veya indirin.
2. Visual Studio üzerinden `OtoPazarProjesi.sln` veya `OtoPazarProjesi.csproj` dosyasını açın.
3. Projeyi derleyip (**F5** veya **Başlat**) tuşuna basarak hemen kullanmaya başlayabilirsiniz.
*(İçerisinde başlangıç verileri hazır yüklü gelir, herhangi bir ayar yapmanıza gerek yoktur.)*

**Örnek Kullanıcılar (Sisteme Yüklü Gelen):**
- **Örnek Alıcı Hesabı:** Kullanıcı Adı: `alici_demo` | Şifre: `1234`
- **Örnek Satıcı Hesabı:** Kullanıcı Adı: `satici_demo` | Şifre: `1234`

## 🎨 Arayüz Tasarımı (UI/UX)
Proje, klasik Windows Forms görünümünün çok ötesine geçerek tamamen modern fontlar (`Segoe UI`), dinamik tam ekran hizalaması, estetik ürün kartları (FlowLayoutPanel) yapıları ile profesyonel bir web platformu (E-Ticaret Sitesi) standartlarında tasarlanmıştır. Tüm bileşenlerde özel kenar boşlukları ve yüksek çözünürlüklü metin yapılandırması kullanılmıştır.
