using System.Collections;

namespace AppointmentIngestion.Tests.Data
{
    public class InvalidAppointmentTimesData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new DateTime(2024, 10, 23, 10, 12, 0, DateTimeKind.Utc) };
            yield return new object[] { new DateTime(2024, 9, 22, 11, 14, 0, DateTimeKind.Utc) };
            yield return new object[] { new DateTime(2023, 12, 11, 6, 56, 0, DateTimeKind.Utc) };
            yield return new object[] { new DateTime(2022, 4, 16, 4, 23, 0, DateTimeKind.Utc) };
            yield return new object[] { new DateTime(2024, 11, 3, 3, 44, 0, DateTimeKind.Utc) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
