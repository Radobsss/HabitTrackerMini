namespace HabitTrackerMini.Data.Entities
{
    public class HabitEntry
    {
        public int Id { get; set; }

        public int HabitId { get; set; }

        public Habit Habit { get; set; } = null!;

        public DateOnly Date { get; set; }
    }
}
