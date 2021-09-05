using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class MaterialService : IMaterialService
    {
        private readonly IRepository<Material> materialRepository;

        public MaterialService(IRepository<Material> _materialRepository)
        {
            this.materialRepository = _materialRepository;
        }

        public void CreateNewMaterial(Material material)
        {
            this.materialRepository.Insert(material);
        }

        public List<Material> GetAllMaterials()
        {
            return this.materialRepository.GetAll().ToList();
        }

        public Material GetMaterial(long id)
        {
            return this.materialRepository.Get(id);
        }

        public void UpdateExistingMaterial(Material material)
        {
            this.materialRepository.Update(material);
        }

        public void DeleteMaterial(long id)
        {
            var Material = this.GetMaterial(id);
            this.materialRepository.Delete(Material);
        }
    }
}
