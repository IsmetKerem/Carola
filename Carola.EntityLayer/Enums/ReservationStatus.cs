namespace Carola.EntityLayer.Enums
{
    public enum ReservationStatus
    {
        Pending = 1,      // Onay Bekleniyor
        Approved = 2,     // Onaylandı
        Rejected = 3,     // Reddedildi
        Cancelled = 4,    // İptal edildi
        Completed = 5     // Tamamlandı (araç teslim alındı ve iade edildi)
    }
}