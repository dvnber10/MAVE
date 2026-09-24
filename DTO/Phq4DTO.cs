namespace MAVE.DTO
{
    /// <summary>PHQ-4: 4 ítems (2 depresión + 2 ansiedad), cada uno 0-3.</summary>
    public class Phq4DTO
    {
        public short D1 { get; set; }
        public short D2 { get; set; }
        public short D3 { get; set; }
        public short D4 { get; set; }
    }

    public class Phq4PointDTO
    {
        public string Date { get; set; } = string.Empty;
        public int D { get; set; }
        public int A { get; set; }
        public int Total { get; set; }
    }
}
