using System.Collections.Generic;

namespace DTOs
{

    public class PasportDTO
    {
        public string FinalInspection { get; set; }
        public string ID { get; set; }
        public BuildingDTO Building { get; set; } // BuildingDTO corresponding to the ID 
        public List<ElementDTO> Elements { get; set; }
    }
}