namespace DTOs
{
    public class BuildingDTO
    {
        // in this case there is no need to store anything as 'int'
        // converting only for possible use in the future

        public string ID { get; set; }
        public string Type { get; set; }
        public string Use { get; set; }
        public string Municipality { get; set; }
        public int HouseNumber { get; set; }
        public string Plat { get; set; }
        public bool Temporary { get; set; }
        public int FractionDenominator { get; set; }
        public int FractionNominator { get; set; }
        public string SubjectICO { get; set; }
        public string SubjectName { get; set; }
        public string SubjectStreet { get; set; }
        public int SubjectHouseNumber { get; set; }
        public int SubjectOrientationNumber { get; set; }
        public string SubjectMunicipality { get; set; }
        public string SubjectPostCode { get; set; }
        public string SubjectDistrict { get; set; }
        public string TelephoneNumber { get; set; }
    }
}