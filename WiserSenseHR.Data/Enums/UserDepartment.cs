using System.ComponentModel.DataAnnotations;

namespace WiserSenseHR.Data.Enums
{
    public enum UserDepartment
    {
        [Display(Name = "Hiçbiri")] None = 1,
        [Display(Name = "Yönetim")] Managament = 2,
        [Display(Name = "İnsan Kaynakları")] HumanResources = 3,
        [Display(Name = "Muhasebe")] Accounting = 4,
        [Display(Name = "Finans")] Finance = 5,
        [Display(Name = "Bilgi Teknolojileri")] InformationTechnology = 6,
        [Display(Name = "Yazılım Geliştirme")] SoftwareDevelopment = 7,
        [Display(Name = "Pazarlama")] Marketing = 8,
        [Display(Name = "Satış")] Sales = 9,
        [Display(Name = "Hukuk")] Legal = 10,
        [Display(Name = "Araştırma ve Geliştirme")] ResearchAndDevelopment = 11,
        [Display(Name = "Üretim")] Production = 12,
        [Display(Name = "Lojistik")] Logistics = 13,
        [Display(Name = "Müşteri Hizmetleri")] CustomerService = 14,
        [Display(Name = "Kalite Kontrol")] QualityControl = 15,
        [Display(Name = "Tedarik Zinciri")] SupplyChain = 16,
        [Display(Name = "Güvenlik")] Security = 17,
        [Display(Name = "Ürün Yönetimi")] ProductManagement = 18,
        [Display(Name = "İş geliştirme")] BusinessDevelopment = 19,
        [Display(Name = "Diğer")] Other = 20
    }
}
