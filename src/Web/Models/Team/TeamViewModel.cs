using Services.Contracts.Mapping;

namespace Web.Models.Team
{
    public class TeamViewModel : IMapFrom<Data.Models.Team>
    {
        public string Id { get; set; }

        public string TeamTitle { get; set; }

        public string TeamYears { get; set; }

        public string PreDescription { get; set; }

        public string Descrtiption { get; set; }

        public string PhotoUrl { get; set; }
    }
}
