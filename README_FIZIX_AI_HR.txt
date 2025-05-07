# 📊 FIZIX AI HR

**FIZIX AI HR**, şirket içi insan kaynakları süreçlerini yönetmek amacıyla geliştirilmiş bir ASP.NET Core MVC uygulamasıdır. Uygulama, kullanıcı kimlik doğrulaması, rol yönetimi ve kullanıcı dostu bir arayüzle HR işlemlerini kolaylaştırmayı hedefler.

---

### 🚀 Özellikler

- **Rol Bazlı Yetkilendirme:** Her kullanıcıya sistemde bir rol atanır (örneğin: Admin, Viewer). Sayfalara ve işlemlere erişim bu role göre sınırlandırılır.
- **Sayfa Bazlı Erişim Kontrolü:** Uygulamadaki butonlar ve sayfalar sadece yetkili rollere görünür.
- **Toplantıları Takvime Yansıtma** Uygulamada oluşturulan toplantılar takvim üzerinde görüntülenir.
- **Özel Gün Etkinlikleri** Çalışanların yaklaşan özel günü (doğum günü, işe başlangıç yıl dönümü) yaklaşan en yakın tarihe göre sıralı olarak ekrana gelir.
- **Giriş İşlemi ve Oturum Yönetimi:** Cookie tabanlı kimlik doğrulama ile güvenli giriş yapılır.
- **FluentValidation ile Form Doğrulama**
- **Entity Framework Core ile PostgreSQL Entegrasyonu**
- **Katmanlı Mimari:** Veri, iş mantığı ve sunum katmanları ayrıdır.
- **Bootstrap 4 Arayüz:** Mobil uyumlu, sade ve kullanıcı dostu tasarım

---

### 🛠️ Kullanılan Teknolojiler

- ASP.NET Core 8.0
- Entity Framework Core 9.0
- PostgreSQL
- FluentValidation
- Cookie Authentication
- jQuery DataTables
- Bootstrap 4
- Summernote Text Editor
- FullCalendar 

---

### ⚙️ Kurulum

#### 1. Projeyi İndirin ve Açın
ZIP dosyasını çıkarın ve Visual Studio ile açın.

#### 2. Veritabanı Bağlantısını Yapılandırın
`appsettings.json` içindeki bağlantı cümlesini kendi PostgreSQL bilgilerinize göre düzenleyin:

ConnectionStrings:
  DefaultConnection: Host=localhost;Database=WiserSenseHRDb;Username=postgres;Password=yourpassword

#### 3. Veritabanı Yedeğini Yükleyin
`WiserSenseWebDB_backup.sql` dosyasını pgAdmin veya psql komut satırı ile yükleyin. (Talimatlar aşağıda)

#### 4. Uygulamayı Başlatın
dotnet run

---

### 🗄️ Veritabanı Yedeğini Yükleme (Backup)

Projeyle birlikte gelen `WiserSenseWebDB_backup.sql` dosyası, varsayılan kullanıcı ve rol bilgilerini içerir. Uygulamayı çalıştırmadan önce bu veritabanını PostgreSQL'e yüklemeniz gerekir.

#### 🔽 Yükleme Adımları (pgAdmin kullanarak):

1. **pgAdmin** uygulamasını açın.  
2. Sol menüden `Databases` bölümüne sağ tıklayın ve **Create > Database...** seçeneğiyle `WiserSenseHRDb` adında boş bir veritabanı oluşturun.
3. Yeni oluşturduğunuz veritabanına sağ tıklayın ve **Restore...** seçeneğine tıklayın.
4. Format olarak **Plain** seçin.
5. `Filename` kısmından projeyle birlikte gelen `WiserSenseWebDB_backup.sql` dosyasını seçin.
6. Restore işlemini başlatın. Yükleme tamamlandığında veritabanınız hazır olacaktır.

#### 🛠 Alternatif Yöntem (psql komutu ile):

```bash
psql -U postgres -d WiserSenseHRDb -f WiserSenseWebDB_backup.sql
```

> Not: `postgres` yerine kendi veritabanı kullanıcı adınızı yazın.  
> Dosya yolunu da `.sql` dosyanızın konumuna göre düzenleyin.

---

### 🔐 Örnek Giriş Bilgileri

E-posta: test@gmail.com  
Şifre: 123456  
Role: Admin
