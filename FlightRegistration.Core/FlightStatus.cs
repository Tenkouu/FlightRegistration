namespace FlightRegistration.Core.Models // Or FlightRegistration.Core.Enums
{
    public enum FlightStatus
    {
        Scheduled,      // Default status before check-in opens
        CheckingIn,     // Бүртгэж байна
        Boarding,       // Онгоцонд сууж байна
        Departed,       // Ниссэн
        Delayed,        // Хойшилсон
        Cancelled       // Цуцалсан
    }
}