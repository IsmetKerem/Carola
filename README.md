<div align="center">

# 🚗 Carola — Araç Kiralama Yönetim Sistemi

### ASP.NET Core 8 ile Geliştirilmiş, AI Destekli Tam Kapsamlı Kiralama Platformu

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/en-us/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-512BD4?style=flat-square)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)
[![Gemini AI](https://img.shields.io/badge/Gemini-AI-4285F4?style=flat-square&logo=google&logoColor=white)](https://ai.google.dev/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE)

[Özellikler](#-özellikler) • [Mimari](#-mimari) • [Kurulum](#-kurulum) • [Ekran Görüntüleri](#-ekran-görüntüleri) • [Teknolojiler](#-teknolojiler)

</div>

---

## 📖 Proje Hakkında

**Carola**, çok şubeli bir araç kiralama firmasının tüm operasyonlarını yöneten, **N-Tier mimari** üzerine inşa edilmiş tam kapsamlı bir kiralama platformudur. Müşterilere hızlı bir rezervasyon deneyimi sunarken, yöneticilere derinlemesine analiz ve kontrol imkanı sağlar.

Proje, M&Y Yazılım Eğitim Akademi Danışmanlık bünyesinde **Murat Yücedağ** hocanın "Case 4" ödevi kapsamında geliştirilmiştir.

### 🎯 Hedef

> Sadece bir CRUD uygulaması değil, **gerçek bir SaaS ürünü** seviyesinde — sürdürülebilir mimari, AI entegrasyonu, otomatik mail sistemi ve detaylı analiz dashboard'u ile tam fonksiyonel bir kiralama ekosistemi.

---

## ✨ Özellikler

### 👤 Müşteri Tarafı

- 🏠 **Dinamik Anasayfa** — Slider, kategoriler, öne çıkan araçlar, marka marquee, lokasyon kartları (9 farklı bölüm)
- 🔍 **Gelişmiş Araç Filtreleme** — Marka, kategori, fiyat, yakıt tipi, vites, tarih bazlı filtreleme + paging + sıralama
- 📋 **Detaylı Araç Sayfası** — 8 teknik özellik, müşteri yorumları, ortalama puan, sticky booking sidebar, ilgili araçlar
- 📝 **3 Adımlı Rezervasyon Wizard** — Ehliyet yükleme → İletişim bilgileri → Özet & onay
- 🤖 **AI Ehliyet OCR** — Gemini Vision API ile ehliyetten otomatik bilgi okuma
- 💬 **AI Chat Asistanı** — Doğal dilde araç önerisi (sağ alt floating widget)
- 📧 **İletişim Formu** — Admin'e mesaj gönderim sistemi

### 🛠️ Admin Paneli

- 🔐 **Identity ile Güvenli Login** — Rol bazlı yetkilendirme (Admin)
- 📊 **Dashboard 2.0** — Chart.js ile gelir trendi, rezervasyon durumu, top araçlar, lokasyon performansı
- 🚗 **12 Entity için Tam CRUD** — Marka, Kategori, Araç, Müşteri, Lokasyon, Slider, WhyUs, Yorum, Mesaj, Galeri, Video, Rezervasyon
- ✅ **Rezervasyon Onay Sistemi** — Onayla/Reddet butonları, otomatik mail tetikleme
- 📬 **Mail Bildirimi** — Onay sonrası HTML mail + embedded %30 indirim kuponu
- 📨 **Mesaj Yönetimi** — Okundu/okunmadı durumu

### 🤖 AI Entegrasyonları

| Servis | Model | Kullanım |
|---|---|---|
| **Gemini Vision** | gemini-2.5-flash | Ehliyet OCR — fotoğraftan ad, soyad, doğum tarihi, ehliyet no, sınıf, veriliş tarihi otomatik çıkarma |
| **Gemini Text** | gemini-2.5-flash | Chat asistanı — kullanıcının doğal dildeki isteğine göre filodan en uygun 3 aracı gerekçeli olarak önerme |

Her iki servise de **retry + fallback** mekanizması eklenmiştir (3 model × 3 deneme + exponential backoff).

---

## 🏗️ Mimari

Proje **N-Tier (5 katmanlı)** mimari ile geliştirilmiştir:

```
┌─────────────────────────────────────────────┐
│   Carola.WebUI (Sunum Katmanı)              │  ← Controllers, Views, ViewModels
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│   Carola.BusinessLayer (İş Katmanı)         │  ← Managers, AutoMapper, Gemini, Mail
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│   Carola.DataAccessLayer (Veri Katmanı)     │  ← DbContext, Repositories, Migrations
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│   Carola.EntityLayer (Veri Modeli)          │  ← Entities, Enums
└─────────────────────────────────────────────┘

         ┌──────────────────────┐
         │  Carola.DtoLayer     │  ← DTOs (her katman kullanır)
         └──────────────────────┘
```

### 🎨 Tasarım Desenleri

- **Generic Repository Pattern** — `IGenericDal<T>` + `GenericRepository<T>` ile tip bağımsız CRUD
- **Generic Service Pattern** — `IGenericService<TResultDto, TCreateDto, TUpdateDto, TGetByIdDto>` ile DTO bazlı iş katmanı
- **Repository Specialization** — Her entity için özel sorgular (`ICarDal : IGenericDal<Car>`)
- **Dependency Injection** — Constructor injection + extension method ile `ServiceRegistration`
- **Fluent API** — `Reservation` entity'sindeki çoklu FK Location ilişkisi için
- **Post-Redirect-Get** — Form submission sonrası double submit engelleme
- **Retry & Fallback Pattern** — Gemini API çağrılarında dayanıklılık

---

## 💻 Teknolojiler

### Backend

| Teknoloji | Versiyon | Kullanım |
|---|---|---|
| .NET | 8.0 LTS | Framework |
| ASP.NET Core MVC | 8.0 | Web framework |
| Entity Framework Core | 8.0 | ORM |
| ASP.NET Identity | 8.0 | Kullanıcı yönetimi |
| AutoMapper | 12.0.1 | DTO mapping |
| MailKit + MimeKit | Latest | Mail gönderimi |

### Veritabanı

- **Microsoft SQL Server** (VDS üzerinde)

### AI / Harici Servisler

- **Google Gemini API** (Vision + Text)
- **Gmail SMTP** (Mail gönderimi)

### Frontend

- **Bootstrap 5** — UI framework
- **Chart.js 4.4** — Dashboard grafikleri
- **jQuery** — Tema plugin'leri
- **Font Awesome 6** — İkonlar
- **Carola HTML Template** — Hazır tema entegrasyonu

---

## 📸 Ekran Görüntüleri

### Anasayfa
> Modern, geniş slider, dinamik içerik
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 46 03" src="https://github.com/user-attachments/assets/232f2bf6-e119-4121-b8f5-e7dfae7a1f8e" />


### Araç Listesi
> Filtreleme, sıralama, paging, 76 araç
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 46 20" src="https://github.com/user-attachments/assets/2940a181-598e-4e1a-bc99-0df1417e3486" />

### Araç Detay Sayfası
> 8 özellik kartı, yorumlar, sticky booking sidebar
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 46 54" src="https://github.com/user-attachments/assets/42a838d4-3f6c-445c-8c95-cfe4e95f4830" />

### AI Ehliyet OCR
> Gemini Vision ile fotoğraftan otomatik bilgi çıkarma
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 47 51" src="https://github.com/user-attachments/assets/be72ad1f-9b7a-4b6a-80f7-5f7955085793" />

### Otomatik Mail Bildirimi
> HTML mail + embedded %30 indirim kuponu
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 51 26" src="https://github.com/user-attachments/assets/a0354242-e657-4e5b-a6f1-d8994c248633" />

### Admin Dashboard 2.0
> Chart.js ile detaylı analiz
<img width="1494" height="829" alt="Screenshot 2026-05-01 at 11 52 14" src="https://github.com/user-attachments/assets/8de9bec7-9ddd-4fc9-b07e-648dbfda25f1" />

---

## 🚀 Kurulum

### Önkoşullar

- .NET SDK 8.0 LTS
- SQL Server 2019+ veya SQL Server Express
- Gemini API Key ([buradan alınabilir](https://aistudio.google.com/app/apikey))
- Gmail App Password (mail için)

### Adım Adım

1. **Repoyu klonla**
   ```bash
   git clone https://github.com/<USERNAME>/carola.git
   cd carola
   ```

2. **NuGet paketlerini yükle**
   ```bash
   dotnet restore
   ```

3. **`appsettings.json` dosyasını düzenle**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=CarolaNewDb;User Id=sa;Password=YOUR_PASS;TrustServerCertificate=True;"
     },
     "GeminiSettings": {
       "ApiKey": "YOUR_GEMINI_API_KEY",
       "BaseUrl": "https://generativelanguage.googleapis.com/v1beta"
     },
     "MailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": "587",
       "Username": "YOUR_EMAIL@gmail.com",
       "Password": "YOUR_GMAIL_APP_PASSWORD",
       "FromEmail": "YOUR_EMAIL@gmail.com",
       "FromName": "Carola"
     }
   }
   ```

4. **Veritabanı migration'larını uygula**
   ```bash
   cd Carola.WebUI
   dotnet ef database update --project ../Carola.DataAccessLayer --startup-project .
   ```

5. **Uygulamayı çalıştır**
   ```bash
   dotnet run
   ```

6. **Tarayıcıda aç**
   ```
   http://localhost:5031
   ```

### 🔐 Demo Login

```
Admin Panel: http://localhost:5031/Admin
Email:       admin@carola.com
Şifre:       Admin123!
```

> 💡 Müşteri tarafında kayıt/login yoktur. Müşteri rezervasyon yaparken email ile bulunur, varsa güncellenir, yoksa oluşturulur.

---

## 📁 Proje Yapısı

```
Carola/
├── Carola.EntityLayer/          # Entity sınıfları (Brand, Car, Reservation, vs.)
│   ├── Entities/
│   └── Enums/
│
├── Carola.DataAccessLayer/      # DbContext, Repository pattern
│   ├── Context/
│   ├── Configurations/          # Fluent API
│   ├── Repositories/
│   │   ├── GenericRepository/
│   │   └── [EntityName]Repository/
│   └── Migrations/
│
├── Carola.BusinessLayer/        # Service / Manager sınıfları
│   ├── Abstract/                # Interface'ler
│   ├── Concrete/                # Implementation
│   │   ├── GeminiOcrService.cs
│   │   ├── GeminiChatService.cs
│   │   └── SmtpMailService.cs
│   └── Mapping/                 # AutoMapper profile
│
├── Carola.DtoLayer/             # DTO sınıfları
│   └── [EntityName]Dtos/        # Result, Create, Update, GetById
│
└── Carola.WebUI/                # Web uygulama
    ├── Areas/Admin/             # Admin paneli
    ├── Controllers/             # Müşteri tarafı
    ├── Views/
    ├── Models/                  # ViewModels
    ├── Services/                # RazorViewToStringRenderer
    ├── Extensions/              # ServiceRegistration
    └── wwwroot/                 # Static dosyalar
```

---

## 📊 Demo Veri

Proje seed verisi ile birlikte gelir:

| Tablo | Kayıt Sayısı |
|---|---|
| Markalar | 18 |
| Kategoriler | 10 |
| Araçlar | 76 |
| Müşteriler | 60 |
| Rezervasyonlar | 180 |
| Yorumlar | 120 |
| Lokasyonlar | 16 |
| Mesajlar | 25 |

---

## 🎓 Öğrendiklerim

Bu projede pekiştirdiğim kavramlar:

- ✅ N-Tier mimari ile katman ayrımı ve bağımlılık yönetimi
- ✅ Generic Repository ve Service pattern ile DRY prensibi
- ✅ Entity Framework Core (Code-First, Migrations, Fluent API, Eager Loading)
- ✅ AutoMapper ile DTO ↔ Entity dönüşümü
- ✅ ASP.NET Identity ile rol bazlı yetkilendirme
- ✅ Gemini AI API entegrasyonu (Vision + Text)
- ✅ Retry + Fallback pattern ile resilience
- ✅ MailKit ile HTML mail + embedded image
- ✅ RazorViewToStringRenderer ile view'ı string'e çevirme
- ✅ Chart.js ile veri görselleştirme
- ✅ Tarih çakışma algoritması (interval overlap)
- ✅ Areas pattern ile uygulama içi alt yapı

---

## 🙏 Teşekkür

Bu projeyi geliştirme sürecindeki rehberlik ve "feature ekle değil, mimari kur" felsefesi için değerli hocam **Murat Yücedağ**'a sonsuz teşekkürlerimi sunarım.

**M&Y Yazılım Eğitim Akademi Danışmanlık** ailesine teşekkürler.

---

## 📜 Lisans

Bu proje eğitim amaçlıdır. MIT License altında dağıtılmaktadır.

---

<div align="center">

**⭐ Eğer projeyi beğendiyseniz star vermeyi unutmayın!**

[🔝 Başa Dön](#-carola--araç-kiralama-yönetim-sistemi)

</div>
