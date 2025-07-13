using MJRecords.Model;

namespace MJRecords.Web.Models
{
    public class EmployeeSearchVM
    {
        public EmployeeSearchDTO empSearchParms { get; set; } = new();
        public List<EmployeeSearchResultDTO>? empSearchResult {  get; set; }
    }
}
