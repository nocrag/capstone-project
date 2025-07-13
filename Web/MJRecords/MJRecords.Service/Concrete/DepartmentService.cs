using MJRecords.Model;
using MJRecords.Repository;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _repo;

        public DepartmentService(IDepartmentRepo repo)
        {
            _repo = repo;
        }

        public Department Create(Department department)
        {
            Validate(department);

            if (department.Errors.Count == 0)
            {
                return _repo.Create(department);
            }

            return department;
        }

        public async Task<Department> CreateAsync(Department department)
        {
            Validate(department);

            if(department.Errors.Count == 0)
            {
                return await _repo.CreateAsync(department);
            }

            return department;
        }

        public Department Delete(Department department)
        {
            return _repo.Delete(department);
        }

        public async Task<Department> DeleteAsync(Department department)
        {
            return await _repo.DeleteAsync(department);
        }

        public DepartmentDTO Get(int id)
        {
            return _repo.Get(id);
        }

        public List<DepartmentDTO> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<List<DepartmentDTO>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<DepartmentDTO> GetAsync(int id)
        {
            return await _repo.GetAsync(id);
        }

        public Department GetDept(int id)
        {
            return _repo.GetDept(id);
        }

        public bool IsDepartmentNameUnique(string name)
        {
            return _repo.IsDepartmentNameUnique(name);
        }

        public Department PopulateDepartmentFromDTO(DepartmentDTO deptDtO)
        {
            return _repo.PopulateDepartmentFromDTO(deptDtO);
        }

        public Department Update(Department department)
        {
            Validate(department);

            if(department.Errors.Count == 0)
            {
                return _repo.Update(department);
            }
            
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            Validate(department);

            if(department.Errors.Count == 0)
            {
                return await _repo.UpdateAsync(department);
            }

            return department;
        }

        public void Validate(Department department)
        {
            // 1. Validate data annotations
            List<ValidationResult> results = new();
            Validator.TryValidateObject(department, new ValidationContext(department), results, true);
            var dbDept = _repo.Get(department.Id);

            foreach (ValidationResult e in results)
            {
                department.AddError(new(e.ErrorMessage, ErrorType.Model));
            }

            //if (department.InvocationDate != dbDept.InvocationDate || department.InvocationDate < DateTime.Today)
            //{
            //    department.AddError(new("Invocation Date can't be set in the past", ErrorType.Business));
            //}

            if (dbDept.Id != department.Id)
            {
                if (department.InvocationDate != dbDept.InvocationDate || department.InvocationDate < DateTime.Today)
                {
                    department.AddError(new("Invocation Date can't be set in the past", ErrorType.Business));
                }
            }

            if(dbDept.Id != department.Id)
            {
                if (!_repo.IsDepartmentNameUnique(department.Name))
                {
                    department.AddError(new("Department with this name already exists. Please try again.", ErrorType.Business));
                }
            }
            
        }

    }
}
