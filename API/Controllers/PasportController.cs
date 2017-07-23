using System.Collections.Generic;
using System.Web.Http;
using DTOs;

namespace API.Controllers
{
    public class PasportController : ApiController
    {
        /// <summary>
        /// GET /api/pasport?ico=*** or GET /api/pasport?id=*** 
        /// </summary>
        /// <param name="ico">ICO (of a user), default is null</param>
        /// <param name="id">ID (of a building), default is null</param>
        /// <returns>If both ico and id are specified, returns null. If ico is specified returns a list of names of buildings that belong to the ICO or an empty list if there are
        /// no buildings linked to the ICO. If ID is specified returns PasportDTO instance of the building with specified ID. </returns>
        public IEnumerable<PasportDTO> Get(string ico = null, string id = null)
        {
            if ((ico == null && id == null) || (ico != null && id != null))
                return null;
            if (ico != null)
                return getBuildingIDs(ico);
            return getBuilding(id);
        }

        /// <summary>
        /// Creates a list of BuildingDTO buildings by getting all rows from the table 'SVERENE_BUDOVY' with ICO. Returns a list of their names (BuildingDTO.Name).
        /// There is no exception handling.
        /// </summary>
        /// <param name="ico">ICO</param>
        /// <returns>a list of names (BuildingDTO.Name)</returns>
        private IEnumerable<PasportDTO> getBuildingIDs(string ico)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            return Database.GetByICO(ico, connectionString);
        }

        /// <summary>
        /// Fetches a pasport (with all its elements, using BuildingDTO and ElementDTO constructors) with ID from the table "PASPORT" stored in an instance of PasportDTO. 
        /// Returns a list of one element, containing the mentioned instance. There is no exception handling.
        /// </summary>
        /// <param name="id">ID of building</param>
        /// <returns>a list of one element, containg an instance of PasportDTO with ID</returns>
        private IEnumerable<PasportDTO> getBuilding(string id)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            return Database.GetByID(id, connectionString);
        }
    }
}