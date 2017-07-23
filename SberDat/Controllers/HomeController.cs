using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTOs;
using System.Data.SqlClient;

namespace SberDat.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string ico)
        {
            Session["ico"] = ico;
            return RedirectToAction("Buildings");
        }

        public ActionResult Buildings()
        {
            string ico = (string) Session["ico"];
            if (String.IsNullOrEmpty(ico))
                return new HttpUnauthorizedResult("Access denied.");

            List<PasportDTO> pasports;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                pasports = Database.GetByICO(ico, connection);
            }
            List<BuildingDTO> buildings = new List<BuildingDTO>();
            pasports.ForEach(pasport => buildings.Add(pasport.Building));

            return View(buildings);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Form(string id)
        {
            string ico = (string) Session["ico"];
            if (String.IsNullOrEmpty(ico))
                return new HttpUnauthorizedResult("Access denied.");

            BuildingDTO building;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                List<PasportDTO> pasports = Database.GetByID(id, connection);
                List<BuildingDTO> buildings = new List<BuildingDTO>();
                pasports.ForEach(pasport => buildings.Add(pasport.Building));

                building = buildings.Count == 0 ? null : buildings[0];
                Session["id"] = id;
                ViewBag.id = id;
                ViewBag.elements = Session["specified_elements"] = Database.GetElements(connection);
            }
            return View(building);
        }

        public ActionResult SubmitForm(FormCollection form)
        {
            string ico = (string) Session["ico"];
            string id = (string) Session["id"];
            List<SpecifiedElementDTO> specified_elements = (List<SpecifiedElementDTO>) Session["specified_elements"];
            if (String.IsNullOrEmpty(ico))
                return new HttpUnauthorizedResult("Access denied.");
            if (String.IsNullOrEmpty(id))
                return new HttpUnauthorizedResult("Access denied.");
            if (specified_elements == null)
            {
                Session["id"] = null;
                return RedirectToAction("Buildings");
            }
            
            List<string> names = new List<string>();
            specified_elements.ForEach(element => names.Add(element.Name));

            List<ElementDTO> elementDTOs = new List<ElementDTO>();
            foreach (string element_name in names)
            {
                ElementDTO elementDTO = new ElementDTO()
                {
                    Name = element_name,
                    State = Request.Form[element_name + "_state"],
                    NeedOfInvestment = Request.Form[element_name + "_need of investment"],
                    AmountOfInvestment = Request.Form[element_name + "_amount of investment"],
                    Notes = Request.Form[element_name + "_notes"]
                };
                elementDTOs.Add(elementDTO);
            }

            PasportDTO pasport = new PasportDTO()
            {
                FinalInspection = form["final_inspection"],
                ID = id,
                Elements = elementDTOs
            };

            Session["id"] = null;
            Session["specified_elements"] = null;
            int state;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                state = Database.InsertPasport(pasport, connection);
            }
            return RedirectToAction("FormSent", new { state = state });
        }

        public ActionResult FormSent(int? state)
        {
            string ico = (string) Session["ico"];
            if (String.IsNullOrEmpty(ico))
                return new HttpUnauthorizedResult("Access denied.");
            return View(state);
        }
    }
}