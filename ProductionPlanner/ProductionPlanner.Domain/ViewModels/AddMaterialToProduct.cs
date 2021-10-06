using ProductionPlanner.Domain.Models;

namespace ProductionPlanner.Domain.ViewModels
{
    public class AddMaterialToProduct
    {
        public long ProductId { get; set; }
        public Material MaterialToAdd { get; set; }

        public Product SelectedProduct { get; set; }

        public string MaterialName { get; set; }
        public double Quantity { get; set; }

    }
}
