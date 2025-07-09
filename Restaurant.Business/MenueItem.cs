
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Restaurant.DataAccess;
using Restaurant.DataAccess.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Business
{
    public class MenueItem

    {
        public enum enMode { AddNew, Update };
        public enMode Mode = enMode.AddNew;


        public MenueItemDTO menueItemDTO => new MenueItemDTO(ID, Name, Description, Price
            , Category, IsAvailable);



        public int ID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;

        public MenueItem(MenueItemDTO menueItemDTO, enMode mode = enMode.AddNew)
        {
            ID = menueItemDTO.ID;
            Name = menueItemDTO.Name;
            Description = menueItemDTO.Description;
            Price = menueItemDTO.Price;
            Category = menueItemDTO.Category;
             
            if (mode == enMode.AddNew)
                IsAvailable = true;
            else
                IsAvailable = menueItemDTO.IsAvailable;

            Mode = mode;
        }


        private async Task<BusinessResult<bool>> _AddNewMenueItem()
        {
            try
            {
                
                AddNewOutputDTO addNewOutputDTO = await MenueItemDataAccess.AddNewMenueItem(menueItemDTO);

                CreatedAt = addNewOutputDTO.CreatedAt;
                UpdatedAt = addNewOutputDTO.CreatedAt;

                ID = addNewOutputDTO.ID;

                if (ID > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Created Successfully.",true);

                else
                    return BusinessResult<bool>.FailureResult("Menu Item Creation Failed"
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException )
            {
                throw;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public static async Task<BusinessResult<PaginatedDTO<MenueItemDTO>>> GetAllMenueItems(int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<MenueItemDTO> paginatedDTO =
                    await MenueItemDataAccess.GetMenueItems(pageNumber, pageSize);

                return BusinessResult<PaginatedDTO<MenueItemDTO>>.
                    SuccessResult("Menu Items Retrieved Successfully",paginatedDTO);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving All Menu Items.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred All Menu Items.", ex);
            }

        }


        public static async Task<BusinessResult<MenueItem>> Find(int id)
        {
            try
            {
                MenueItemDTO menueItemDTO = await MenueItemDataAccess.GetMenueItemByID(id);

                if (menueItemDTO.ID != 0)
                    return BusinessResult<MenueItem>.SuccessResult("Menu Item  Found successfully",
                        new MenueItem(menueItemDTO, enMode.Update));

                else
                    return BusinessResult<MenueItem>.FailureResult($"Menu Item  with ID {id} Not Found"
                        ,BusinessResult<MenueItem>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Menu Item By ID.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Menu Item By ID.", ex);
            }

        }


        public static async Task<BusinessResult<MenueItem>> Find(string name)
        {
            try
            {
                MenueItemDTO menueItemDTO = await MenueItemDataAccess.GetMenueItemByName(name);


                if (menueItemDTO.ID != 0)
                    return BusinessResult<MenueItem>.SuccessResult("Menu Item  Found successfully",
                        new MenueItem(menueItemDTO, enMode.Update));

                else
                    return BusinessResult<MenueItem>.FailureResult($"Menu Item  with name {name} Not Found"
                        , BusinessResult<MenueItem>.enError.NotFound);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Find Menu Item By Name.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Find Menu Item By Name.", ex);
            }

        }

        public static async Task<BusinessResult<PaginatedDTO<MenueItemDTO>>> GetMenueItemsByCategory(string category,int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<MenueItemDTO> paginatedMenueItemDTO =
                    await MenueItemDataAccess.GetMenueItemsByCategory(category,pageNumber,pageSize);


                    return BusinessResult<PaginatedDTO<MenueItemDTO>>.SuccessResult("Menu Items  retrieved successfully",
                        paginatedMenueItemDTO);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Menu Items By Category.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Menu Items By Category.", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<MenueItemDTO>>> GetAvailableMenueItems( int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<MenueItemDTO> paginatedMenueItemDTO =
                     await MenueItemDataAccess.GetAvailableMenuItems( pageNumber, pageSize);


                    return BusinessResult<PaginatedDTO<MenueItemDTO>>.SuccessResult("Menu Items  retrieved successfully",
                        paginatedMenueItemDTO);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving  Available Menu Items..", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving  Available Menu Items..", ex);
            }

        }


        public static async Task<BusinessResult<PaginatedDTO<MenueItemDTO>>> GetUnAvailableMenueItems( int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<MenueItemDTO> paginatedMenueItemDTO =
                      await MenueItemDataAccess.GetUnAvailableMenuItems(pageNumber, pageSize);


                    return BusinessResult<PaginatedDTO<MenueItemDTO>>.SuccessResult("Menu Items  retrieved successfully",
                        paginatedMenueItemDTO);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Un Available Menu Items.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Un Available Menu Items..", ex);
            }

        }

       

        public static async Task<BusinessResult<PaginatedDTO<MenueItemDTO>>> GetMenueItemsByDescription(string? description, int pageNumber, int pageSize)
        {
            try
            {
                PaginatedDTO<MenueItemDTO> paginatedDTO =  
                    await MenueItemDataAccess.GetMenueItemsByDescription(description, pageNumber, pageSize);


                    return BusinessResult<PaginatedDTO<MenueItemDTO>>
                        .SuccessResult("Menu Items  retrieved successfully",paginatedDTO);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during retrieving Menu Items By Description.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during retrieving Menu Items By Description.", ex);
            }

        }

        private async Task<BusinessResult<bool>> _UpdateMenueItem()
        {
            try
            {
                int rowsAffected = await MenueItemDataAccess.Update(menueItemDTO);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Updated Successfully.",true);

                else
                    return BusinessResult<bool>.FailureResult("Update Menu Item Fail."
                        ,BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Menu Item.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Menu Item.", ex);
            }
        }

        public  async Task<BusinessResult<bool>> MakeItAvailable()
        {
            try
            {
                int rowsAffected = await MenueItemDataAccess.Update(ID,true);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Updated Successfully.",true);

                else
                    return BusinessResult<bool>.FailureResult("Update Menu Item Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Menu Item availability.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Menu Item availability..", ex);
            }
        }


        public async Task<BusinessResult<bool>> MakeItUnAvailable()
        {
            try
            {
                int rowsAffected = await MenueItemDataAccess.Update(ID, false);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Updated Successfully.",true);

                else
                    return BusinessResult<bool>.FailureResult("Update Menu Item Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Update Menu Item availability.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Update Menu Item availability.", ex);
            }
        }


        public async Task<BusinessResult<bool>> Delete()
        {
            try
            {

                int rowsAffected =
                    await MenueItemDataAccess.Delete(ID);

                if (rowsAffected > 0)
                    return BusinessResult<bool>.SuccessResult("Menu Item Deleted Successfully.");

                else
                    return BusinessResult<bool>.FailureResult("Delete Menu Item Fail."
                        , BusinessResult<bool>.enError.ServerError);

            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return BusinessResult<bool>.FailureResult("Menu Item Cannot Deleted because has Related Data."
                        , BusinessResult<bool>.enError.Conflict);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Deleting Menu Item .", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Deleting Menu Item .", ex);
            }

        }


        public async Task<BusinessResult<bool>> Save()
        {
            try
            {

                if (await MenueItemDataAccess.IsMenuItemNameUsed(ID, Name))
                    return BusinessResult<bool>.FailureResult
                                ("Menu Item Name already used ,please enter another one."
                                , BusinessResult<bool>.enError.Conflict);

                switch (Mode)
                {
                    case enMode.AddNew:

                       var result = await _AddNewMenueItem();

                        if(result.Success)
                            Mode = enMode.Update;

                      return  result;

                    case enMode.Update:

                        return await _UpdateMenueItem();

                }

                return BusinessResult<bool>.FailureResult("Add/Update Menu Item Operation Failed."
                    , BusinessResult<bool>.enError.ServerError);
            }
            catch (SqlHelper.DataAccessException ex)
            {
                throw new ApplicationException("A database error occurred during Add/Update Menu Item.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("A unexpected error occurred during Add/Update Menu Item.", ex);
            }

        }
    }
}
