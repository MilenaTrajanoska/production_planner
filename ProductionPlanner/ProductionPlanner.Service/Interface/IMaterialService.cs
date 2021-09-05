using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IMaterialService
    {
        void CreateNewMaterial(Material material);
        List<Material> GetAllMaterials();
        Material GetMaterial(long id);
        void UpdateExistingMaterial(Material material);
        void DeleteMaterial(long id);
    }
}
